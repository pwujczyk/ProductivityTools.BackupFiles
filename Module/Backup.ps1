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

function Backup-WithMasterConfiguration {
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

#BackupFolders -SourceDirectory "D:\Trash\x1" -DestinationDirectory "D:\Trash\x2" -Verbose
Backup-WithMasterConfiguration -Verbose