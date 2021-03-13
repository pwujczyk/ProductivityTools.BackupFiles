<!--Category:Powershell--> 
 <p align="right">
    <a href="https://www.powershellgallery.com/packages/ProductivityTools.ConvertTcx2Gpx/"><img src="Images/Header/Powershell_border_40px.png" /></a>
    <a href="http://productivitytools.tech/convert-tcx-to-gpx/"><img src="Images/Header/ProductivityTools_green_40px_2.png" /><a> 
    <a href="https://github.com/pwujczyk/ProductivityTools.ConvertTcx2Gpx"><img src="Images/Header/Github_border_40px.png" /></a>
</p>
<p align="center">
    <a href="http://productivitytools.tech/">
        <img src="Images/Header/LogoTitle_green_500px.png" />
    </a>
</p>


# Backup files

Module allows to perform backup of the files. 
<!--more-->
General idea is to store small **.Backup** file which defines which catalog should be copied.

![BackupFile](Images/BackupFile.png)

This approach make solution distributed. 

### Find-BackupFiles
Command finds .Backup files in given directory. This command could identify all directories which will be taken into account during backup process.


```powershell
Find-BackupFiles -Source d:\ 
```

### New-Backup

```powershell
New-Backup -Source d: -Destination \\x\G\BackupPawelPC\ -Verbose -VerbosityLevel Detailed
```

### New-BackupFile

Command creates new backup File.


## Verbosity Level
Module performs a lot of operations of copying and removing. It allows to display different level of verbose information. 
- General
- Detailed
- Dev

<!--og-image-->

## .BackupFile

File defines what cmdlet should do with directory in which file is placed.

```xml
<?xml version="1.0" encoding="utf-8"?>
<Backup>
  <!--Mode - It will decide how copying operation should be performed-->
  <!--Options:-->
  <!--NotDefined - NotDefined-->
  <!--DoNothing - DoNothing-->
  <!--CopyRecursively - It will copy files and directories recursively, until it will find another file which will override action [Default]-->
  <!--CopyJustFiles - CopyJustFiles-->
  <Mode>CopyRecursively</Mode>
  <!--CopyStrategy - It tells what to do if file in the target location will be found-->
  <!--Options:-->
  <!--OverrideAlways - OverrideAlways-->
  <!--OverideIfModificationDateIsNewer - OverideIfModificationDateIsNewer [Default]-->
  <!--BreakWhenFound - BreakWhenFound-->
  <!--Omit - Omit-->
  <CopyStrategy>OverideIfModificationDateIsNewer</CopyStrategy>
  <!--RedundantItems - It defines how to behave when in targed directory additional files or directories will be found. This situation will occur for example when in source you will move files.-->
  <!--Options:-->
  <!--Remove - Remove [Default]-->
  <!--Move - Move-->
  <!--Leave - Leave-->
  <RedundantItems>Remove</RedundantItems>
</Backup>
```
