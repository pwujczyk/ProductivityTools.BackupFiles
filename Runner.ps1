Import-Module "$PSScriptRoot/ProductivityTools.Backup.psm1" -Force

cd $PSScriptRoot/
#Create-BackupFileIndicator
#Backup-Directory -Verbose
#Backup-FoldersWithMasterConfiguration
#cd d:\Trash
#Create-BackupFileIndicator

Backup-FoldersWithMasterConfiguration
