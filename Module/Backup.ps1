function BackupDirectory{
       param (
        [Parameter()]
        [string]
        $SourceDirectory,

        [Parameter()]
        [string]
        $DestinationDirectory
    )

    Write-Verbose "[BackupDirectory] Source directory: $SourceDirectory Destination directory: $DestinationDirectory"

    Robocopy.exe $SourceDirectory $DestinationDirectory /MIR /DCOPY:T /e /copy:DAT /mt 

}

function BackupFolders{
    [CmdletBinding()]
    param (
        [Parameter()]
        [string]
        $SourceDirectory,

        [Parameter()]
        [string]
        $DestinationDirectory
    )

    Write-Verbose "Source directory: $SourceDirectory Destination directory: DestinationDirectory"
    $mainLevelDirectories=Get-ChildItem -Path $SourceDirectory -Directory
    Write-Output $mainLevelDirectories
    foreach($mainLevelDirectory in $mainLevelDirectories)
    {
        $diFullrName=$mainLevelDirectory.FullName
        $dirName=$mainLevelDirectory.Name
        $backupDir=Test-Path "$diFullrName\.backup.pt"
        if ($backupDir)
        {
            BackupDirectory -SourceDirectory $diFullrName -DestinationDirectory "$DestinationDirectory\$dirName"

        }
    }

    

}

BackupFolders -SourceDirectory "D:\Trash\x1" -DestinationDirectory "D:\Trash\x2" -Verbose