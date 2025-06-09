<<<<<<< HEAD
Import-Module "$PSScriptRoot/ProductivityTools.Backup.psm1"

#Backup-FoldersWithMasterConfiguration
cd d:\Trash
Create-BackupFileIndicator
=======
Import-Module "$PSScriptRoot/ProductivityTools.Backup.psm1" -Force

cd $PSScriptRoot/
#Create-BackupFileIndicator
Backup-Directory -Verbose
#Backup-FoldersWithMasterConfiguration
#cd d:\Trash
#Create-BackupFileIndicator
>>>>>>> detached
