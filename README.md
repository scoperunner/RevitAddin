# CRHRevitCRH

This project generates an addin to Revit and made using revit 2024.
As such if you are using an older version of revit you may have to manually move the "RevitAddinCRH.addin" and "RevitAddinCRH.dll" to you addin-folder.
which is usually located at %AppData%\Roaming\Autodesk\Revit\addins\"versionNumber".

if you are using one of the following versions, Then the Build events should automaticly move newly compiled addin to the specified folder.
supported versions:
Revit 2024,
Revit 2023,
Revit 2019,
Revit 2018,
Revit 2017,
Revit 2016


otherwise you will find a copy of the .addin and .dll files at $(ProjectDir)\Output