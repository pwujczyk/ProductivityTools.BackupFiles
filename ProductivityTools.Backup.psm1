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
        $fileIndicator=".backup.pt"
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
}

function Create-BackupFileIndicator{
   [CmdletBinding()]
    param (
        [Parameter(Mandatory=$false, HelpMessage="Specify the directory path where the .backup.pt file will be created. Defaults to the current directory.")]
        [string]
        $Path = (Get-Location).Path
    )

    $fileName = ".backup.pt"
    $filePath = Join-Path -Path $Path -ChildPath $fileName
    $fileContent = "This is the file indicator that says that this directory should be taken into account during the backup operation performed by the ProductivityTools.Backup module"

    try {
        Set-Content -Path $filePath -Value $fileContent -ErrorAction Stop
        Write-Verbose "[Backup Module][Create-BackupFileIndicator] Successfully created $filePath"
        Write-Output "Successfully created $filePath"
    }
    catch {
        Write-Error "[Backup Module][Create-BackupFileIndicator] Failed to create $filePath. Error: $($_.Exception.Message)"
    }

}

Export-ModuleMember Backup-Folders
Export-ModuleMember Backup-FoldersWithMasterConfiguration 
Export-ModuleMember Create-BackupFileIndicator

#BackupFolders -SourceDirectory "D:\Trash\x1" -DestinationDirectory "D:\Trash\x2" -Verbose
#Backup-WithMasterConfiguration -Verbose