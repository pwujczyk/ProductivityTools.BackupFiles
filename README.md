# ProductivityTools.Backup PowerShell Module

This PowerShell module provides a set of functions to facilitate directory backups, primarily using Robocopy for the underlying file transfer operations. It allows for targeted backups based on indicator files and master configurations.

## Features

*   Backup individual directories based on a local `.backup.pt` configuration file.
*   Backup multiple subdirectories if they contain a `.backup.pt` indicator file.
*   Utilize a master configuration for defining global backup source and destination paths.
*   Easily create the `.backup.pt` indicator files.

## Installation

1.  Ensure the `ProductivityTools.Backup` module directory (containing `ProductivityTools.Backup.psd1` and `ProductivityTools.Backup.psm1`) is placed in one of your PowerShell module paths (e.g., `C:\Users\YourUser\Documents\WindowsPowerShell\Modules`).
2.  Or, you can import it directly in your script:
    ```powershell
    Import-Module -Name "path\to\ProductivityTools.Backup.psm1" -Force
    ```

## Indicator File: `.backup.pt`

The `.backup.pt` file is central to how this module identifies directories for backup and, in some cases, their specific backup destinations.

*   **Presence as an Indicator**: For `Backup-Folders`, the mere presence of this file in a subdirectory signals that the subdirectory should be backed up.
*   **Destination Configuration**: For `Backup-Directory`, the first line of this file can specify the backup destination if it's uncommented and formatted as `DestinationPath:your\target\path`.

You can create this file using the `Create-BackupFileIndicator` cmdlet. By default, it's created with a commented-out `#DestinationPath:c:\trash` line.

## Functions

Below are the details of the exported functions available in this module.

---

### `Backup-Directory`

**SYNOPSIS**

Backs up a single directory if it contains a .backup.pt file with an uncommented DestinationPath.

**DESCRIPTION**

This function checks for a .backup.pt file in the specified source directory (or the current directory if -Path is not provided). It reads the first line of this file. If the first line starts with "DestinationPath:" (case-insensitive and not commented out with a '#'), the function will parse the destination path from this line.
It then invokes an internal BackupDirectory function (which uses Robocopy) to back up the source directory to the parsed destination path.
The function supports -WhatIf and -Confirm parameters due to SupportsShouldProcess.

**PARAMETERS**

*   `Path`
    *   Specifies the source directory path that contains the .backup.pt file and is to be backed up. This parameter is optional and defaults to the current directory. It can accept input from the pipeline.

**EXAMPLES**

```powershell
Backup-Directory -Path "C:\Projects\MyProject" -Verbose
```
This command attempts to back up "C:\Projects\MyProject". It will look for "C:\Projects\MyProject\\.backup.pt". If this file exists and its first line is, for example, "DestinationPath:D:\Backups\MyProjectBackup", then "C:\Projects\MyProject" will be backed up to "D:\Backups\MyProjectBackup".

```powershell
Get-Location | Backup-Directory -Confirm
```
This command pipes the current location to Backup-Directory and will prompt for confirmation before proceeding with the backup, provided the .backup.pt file is correctly configured.

**NOTES**

For this function to perform a backup, the .backup.pt file in the source directory must exist, not be empty, and its very first line must be in the format: `DestinationPath:Your\Target\Path`.
The Robocopy command used for backup is configured with `/MIR`, meaning it mirrors the source to the destination, including deleting files in the destination if they no longer exist in the source.

---

### `Backup-Folders`

**SYNOPSIS**

Backs up subdirectories from a source location to a destination if they contain a .backup.pt indicator file.

**DESCRIPTION**

The Backup-Folders function iterates through all immediate subdirectories within the specified -SourceDirectory. For each subdirectory, it checks for the existence of a .backup.pt file (as created by Create-BackupFileIndicator). If this indicator file is found, the entire subdirectory is backed up to a corresponding subdirectory (with the same name) under the -DestinationDirectory. The backup is performed using Robocopy with mirroring enabled (/MIR), ensuring the destination is an exact copy of the source subdirectory.

**PARAMETERS**

*   `SourceDirectory`
    *   The path to the main directory whose immediate subdirectories will be scanned for the .backup.pt file.
*   `DestinationDirectory`
    *   The path to the main directory where the backups of the qualifying subdirectories will be created. Each backed-up subdirectory will reside in a folder of the same name under this destination.

**EXAMPLES**

```powershell
Backup-Folders -SourceDirectory "C:\Users\YourUser\Projects" -DestinationDirectory "D:\Backups\Projects" -Verbose
```
This command will scan subdirectories within "C:\Users\YourUser\Projects". If a subdirectory like "C:\Users\YourUser\Projects\MyProject1" contains a .backup.pt file, it will be backed up to "D:\Backups\Projects\MyProject1".

**NOTES**

This function relies on the presence of a .backup.pt file in a subdirectory to mark it for backup. The actual backup of individual directories is performed by Robocopy with /MIR, /DCOPY:T, /e, /copy:DAT, /mt switches. This means the destination subdirectory will be a mirror of the source subdirectory, including deletions. The .backup.pt file itself is not used for destination path configuration by this function; its presence is merely an indicator.

---

### `Backup-FoldersWithMasterConfiguration`

**SYNOPSIS**

Backs up multiple folders based on a master configuration.

**DESCRIPTION**

This function retrieves a source directory and a destination directory from a master configuration (using an assumed Get-MasterConfiguration function). It then calls the Backup-Folders function to process subdirectories within the source, backing up those that contain a .backup.pt file.

**EXAMPLES**

```powershell
Backup-FoldersWithMasterConfiguration -Verbose
```
This command initiates the backup process using paths defined in the master configuration. It will provide verbose output detailing the operations.

**NOTES**

This function relies on an external `Get-MasterConfiguration` function to retrieve "BackupSource" and "BackupDestination" paths. Ensure this configuration is properly set up. Subdirectories in the "BackupSource" path will only be backed up if they contain a .backup.pt file (created by Create-BackupFileIndicator). The actual backup of individual directories is performed by Robocopy with /MIR, /DCOPY:T, /e, /copy:DAT, /mt switches. This means the destination will be a mirror of the source, including deletions.

---

### `Create-BackupFileIndicator`

**SYNOPSIS**

Creates a .backup.pt indicator file in a specified directory.

**DESCRIPTION**

This function creates a file named .backup.pt in the target directory. This file serves two main purposes:
1. It signals to the Backup-Folders function that the directory it resides in (or its parent, depending on the logic of Backup-Folders) should be included in backup operations.
2. It contains a commented-out #DestinationPath: line. If uncommented and a valid path is provided, the Backup-Directory cmdlet can use this file to back up the directory to the specified path.

The file is pre-populated with comments explaining its purpose.

**PARAMETERS**

*   `Path`
    *   Specifies the directory path where the .backup.pt file will be created. If not provided, it defaults to the current directory.

**EXAMPLES**

```powershell
Create-BackupFileIndicator
```
This command creates the .backup.pt file in the current working directory.

```powershell
Create-BackupFileIndicator -Path "D:\MyImportantProject"
```
This command creates the .backup.pt file in "D:\MyImportantProject".

**NOTES**

The content of the created .backup.pt file includes a commented-out `#DestinationPath:c:\trash` line.

---

## Contributing

Contributions are welcome! Please feel free to submit pull requests or open issues.



## Tech Corners:
/copy:DAT: means you will copy the source folder to destination folder with D (Data), A (Attributes), T (Time stamps), and you could modify these copied files. If you don’t want to modify these folders and files, you could replace /copy:DAT to /copyall or /copy:DATSOU.

/mt: Creates multi-threaded copies with 8 threads.

/V - verbose