function Get-BackupIndicatorFileName {
    # Internal helper function to get the standard backup indicator file name.
    return ".backup.pt"
}

function BackupDirectory {
    param (
        [Parameter()]
        [string]
        $SourceDirectory,

        [Parameter()]
        [string]
        $DestinationDirectory
    )

    Write-Verbose "[Backup Module][BackupDirectory] Source directory: $SourceDirectory Destination directory: $DestinationDirectory"

    Robocopy.exe $SourceDirectory $DestinationDirectory /MIR /DCOPY:T /e /copy:DAT /mt 

}

function Remove-EmptyDestinatinonFolders {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param (
        [Parameter(Mandatory = $true)]
        [string]
        $DestinationDirectory
    )

    # Get all immediate subdirectories in the destination
    $subDirectories = Get-ChildItem -Path $DestinationDirectory -Directory -ErrorAction SilentlyContinue

    foreach ($dir in $subDirectories) {
        # Get all items in the current subdirectory (files and folders), including hidden ones
        $items = Get-ChildItem -Path $dir.FullName -Force

        # Check if there is exactly one item and it's the backup indicator file
        if ($items.Count -eq 1) {
            $item = $items[0]
            # Ensure it's a file and its name matches the backup indicator file name
            if (-not $item.PSIsContainer -and $item.Name -eq (Get-BackupIndicatorFileName)) {
                Write-Verbose "[Backup Module][Remove-EmptyDestinatinoFolders] Preparing to remove empty backup folder: $($dir.FullName)"
                if ($PSCmdlet.ShouldProcess("'$($dir.FullName)' because it only contains a backup indicator file.", "Remove Directory")) {
                    Remove-Item -Path $dir.FullName -Recurse -Force
                    Write-Verbose "[Backup Module][Remove-EmptyDestinatinoFolders] Removed folder: $($dir.FullName)"
                }
            }
        }
    }
}

function Backup-Folders {
    [CmdletBinding()]
    param (
        [Parameter()]
        [string]
        $SourceDirectory,

        [Parameter()]
        [string]
        $DestinationDirectory
    )

    Write-Verbose "[Backup Module][Backup-Folders]: Source directory: $SourceDirectory Destination directory: DestinationDirectory"
    $mainLevelDirectories = Get-ChildItem -Path $SourceDirectory -Directory
    Write-Output "[Backup Module][Backup-Folders][mainLevelDirectories]:"
    Write-Output $mainLevelDirectories

    foreach ($mainLevelDirectory in $mainLevelDirectories) {
        $fileIndicator = Get-BackupIndicatorFileName
        $diFullrName = $mainLevelDirectory.FullName
        $dirName = $mainLevelDirectory.Name
        $backupDir = Test-Path "$diFullrName\$fileIndicator"
        if ($backupDir) {
            Write-Verbose "[Backup Module][Backup-Folders] fileIndicator was found in the directory"
            BackupDirectory -SourceDirectory $diFullrName -DestinationDirectory "$DestinationDirectory\$dirName"
        }
        else {
            Write-Verbose "[Backup Module][Backup-Folders] fileIndicator was not found in the directory"

        }

    }

    

}

function Backup-FoldersWithMasterConfiguration {
    [CmdletBinding()]
    param ()

    $source = Get-MasterConfiguration "BackupSource"
    $destination = Get-MasterConfiguration "BackupDestination"

    Write-Verbose "[Backup Module]: Source from MasterConfiguration: $source, Destination from MasterConfiguration: $destination"
    if ($source -eq $null -or $destination -eq $null) {
        Write-Error "Master configuration not set"
    }
    else {
        Backup-Folders -SourceDirectory $source -DestinationDirectory $destination
        
    }
    Remove-EmptyDestinatinonFolders -DestinationDirectory $destination
}

function Create-BackupFileIndicator {
    [CmdletBinding()]
    param (
        [Parameter(Mandatory = $false, HelpMessage = "Specify the directory path where the .backup.pt file will be created. Defaults to the current directory.")]
        [string]
        $Path = (Get-Location).Path
    )

    $fileName = Get-BackupIndicatorFileName
    $filePath = Join-Path -Path $Path -ChildPath $fileName
    $fileContent = "#DestinationPath:c:\trash
#This is the file indicator that says that this directory should be taken into account during the backup operation performed by the ProductivityTools.Backup module\
#If you uncomment the first line you could use Backup-Directory cmdlet
    "

    try {
        Set-Content -Path $filePath -Value $fileContent -ErrorAction Stop
        Write-Verbose "[Backup Module][Create-BackupFileIndicator] Successfully created $filePath"
        Write-Output "Successfully created $filePath"
    }
    catch {
        Write-Error "[Backup Module][Create-BackupFileIndicator] Failed to create $filePath. Error: $($_.Exception.Message)"
    }

}

function Backup-Directory {
    [CmdletBinding(SupportsShouldProcess = $true)]
    param (
        [Parameter(Mandatory = $false, ValueFromPipeline = $true, ValueFromPipelineByPropertyName = $true, HelpMessage = "Specify the source directory path containing the .backup.pt file. Defaults to the current directory.")]
        [string]
        $Path = (Get-Location).Path
    )

    Write-Verbose "[Backup Module][Backup-Directory] Attempting to backup directory: $Path"

    $indicatorFileName = Get-BackupIndicatorFileName
    $indicatorFilePath = Join-Path -Path $Path -ChildPath $indicatorFileName

    if (-not (Test-Path $indicatorFilePath -PathType Leaf)) {
        Write-Warning "[Backup Module][Backup-Directory] Indicator file '$indicatorFileName' not found in '$Path'."
        return
    }

    Write-Verbose "[Backup Module][Backup-Directory] Found indicator file: $indicatorFilePath"

    # Read only the first line for efficiency
    $firstLine = Get-Content -Path $indicatorFilePath -TotalCount 1 | Select-Object -First 1

    if ($null -eq $firstLine) {
        Write-Warning "[Backup Module][Backup-Directory] Indicator file '$indicatorFilePath' is empty."
        return
    }

    $destinationPathKey = "DestinationPath:"
    # Check if the line starts with "DestinationPath:" (case-insensitive) and is not commented out
    if ($firstLine.TrimStart().StartsWith($destinationPathKey) -eq $true) {
        # Split by the first colon only, then trim
        $destinationDir = ($firstLine -split ':', 2)[1].Trim()

        if ([string]::IsNullOrWhiteSpace($destinationDir)) {
            Write-Warning "[Backup Module][Backup-Directory] '$destinationPathKey' found in '$indicatorFilePath', but no destination path is specified after the colon."
            return
        }

        Write-Verbose "[Backup Module][Backup-Directory] Parsed DestinationDirectory: '$destinationDir' from '$indicatorFilePath'"

        if ($PSCmdlet.ShouldProcess("source '$Path' to destination '$destinationDir'", "Backup Directory (using .backup.pt configuration)")) {
            BackupDirectory -SourceDirectory $Path -DestinationDirectory $destinationDir # Call the internal function
            Write-Output "[Backup Module][Backup-Directory] Backup initiated for '$Path' to '$destinationDir'."
        }
    }
    else {
        Write-Verbose "[Backup Module][Backup-Directory] First line of '$indicatorFilePath' does not contain an uncommented and valid '$destinationPathKey' entry."
        Write-Warning "[Backup Module][Backup-Directory] No uncommented '$destinationPathKey' found in '$indicatorFilePath'. To enable backup for this directory using Backup-Directory, edit the first line of '$indicatorFilePath' to be like 'DestinationPath:your\target\path'."
    }
}

Export-ModuleMember Backup-Folders
Export-ModuleMember Backup-FoldersWithMasterConfiguration 
Export-ModuleMember Create-BackupFileIndicator
Export-ModuleMember Backup-Directory
Export-ModuleMember Remove-EmptyDestinatinonFolders

#BackupFolders -SourceDirectory "D:\Trash\x1" -DestinationDirectory "D:\Trash\x2" -Verbose
#Backup-WithMasterConfiguration -Verbose