# StyleCopCmdLine
A wrapper project to make it possible to easily call StyleCop from the command line or a PowerShell script.

The core code is taken from ALM Rangers TFS custom build activities project, with the Windows Workflow logic removed. 
See https://github.com/tfsbuildextensions/CustomActivities

The intitial plan had been to do all the work in PowerShell but it was far easier to do the async envents in C# 
and just provide a wrapper

**Usage**

The basic usage of the command line tool is

> StyleCopCmdLine --f="File1.cs" "File2.cs" --s="AllSettingsEnabled.StyleCop"

The full set of parameters are 

  -f, --SourceFiles                    Required. The files or folders to scan.
                                       Multiple files or folders can be listed
                                       
  -s, --SettingsFile                   Required. The settings to use.
  
  --AdditionalAddInPaths               (Default: System.String[]) The paths to
                                       rules files. Multiple folders can be
                                       listed
                                       
  --ShowOutput                         (Default: False) Show the addin of files
                                       to scan output in the log.
                                       
  --CacheResults                       (Default: False) Cache scan results.
  
  --XmlOutputFile                      (Default: .\stylecop.xml) Xml Output
                                       File.
                                       
  --LogFile                            (Default: .\stylecop.log) Log File.
  
  --ForceFullAnalysis                  (Default: True) Force a full analysis.
  
  --TreatViolationsErrorsAsWarnings    (Default: True) Treat violation errors
                                       as warnings.
                                       
  --MaximumViolationCount              (Default: 1000) Maximum violations
                                       before the scan is stopped.
                                       
  -v, --verbose                        (Default: True) Prints the configuration
                                       messages to standard output.
                                       
