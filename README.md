# StyleCopCmdLine
A wrapper project to make it possible to easily call StyleCop from the command line or a PowerShell script.

The core code is taken from ALM Rangers TFS custom build activities project, with the Windows Workflow logic removed. 
See https://github.com/tfsbuildextensions/CustomActivities

The intitial plan had been to do all the work in PowerShell but it was far easier to do the async envents in C# 
and just provide a wrapper
