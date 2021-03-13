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

### Find-BackupFiles
Command finds .Backup files in given directory


```powershell
Find-BackupFiles -Source d:\ 
```

### New-Backup

```powershell
New-Backup -Source d: -Destination \\x\G\BackupPawelPC\ -Verbose -VerbosityLevel Detailed
```

Module first will download **GPSBabel** application which is used to perform conversion. application is stored directly in the GitHub.

<!--og-image-->
![Download and extract Babel](Images/DownloadAndExtract.png)

Next it will extract it and start the conversion.

![Download and extract Babel](Images/Convert.png)