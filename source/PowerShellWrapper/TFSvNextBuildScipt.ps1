#
# Script to allow StyleCop to be run as part of the TFS vNext build
#
[CmdletBinding()]
param
(
    # We have to pass this boolean flag as string, we cast it before we use it
    # have to use 0 or 1, true or false
    [string]$TreatStyleCopViolationsErrorsAsWarnings = 'False'
)

# local test values, should be commented out in production
#$Env:BUILD_STAGINGDIRECTORY = "c:\drops"
#$Env:BUILD_SOURCESDIRECTORY = "C:\code\MySolution"

if(-not ($Env:BUILD_SOURCESDIRECTORY -and $Env:BUILD_STAGINGDIRECTORY))
{
    Write-Error "You must set the following environment variables"
    Write-Error "to test this script interactively."
    Write-Host '$Env:BUILD_SOURCESDIRECTORY - For example, enter something like:'
    Write-Host '$Env:BUILD_SOURCESDIRECTORY = "C:\code\MySolution"'
    Write-Host '$Env:BUILD_STAGINGDIRECTORY - For example, enter something like:'
    Write-Host '$Env:BUILD_STAGINGDIRECTORY = "C:\drops"'
    exit 1
}

# pickup the build locations from the environment
$stagingfolder = $Env:BUILD_STAGINGDIRECTORY
$sourcefolder = $Env:BUILD_SOURCESDIRECTORY

# have to convert the string flag to a boolean
$treatViolationsErrorsAsWarnings = [System.Convert]::ToBoolean($TreatStyleCopViolationsErrorsAsWarnings)

Write-Host ("Source folder (`$Env)  [{0}]" -f $sourcefolder) -ForegroundColor Green
Write-Host ("Staging folder (`$Env) [{0}]" -f $stagingfolder) -ForegroundColor Green
Write-Host ("Treat violations as warnings (Param) [{0}]" -f $treatViolationsErrorsAsWarnings) -ForegroundColor Green
 
# the overall results across all sub scans
$overallSuccess = $true
$projectsScanned = 0
$totalViolations = 0


# load the StyleCop classes, this assumes that the StyleCop.DLL, StyleCop.Csharp.DLL,
# StyleCop.Csharp.rules.DLL in the same folder as the StyleCopWrapper.dll
Add-Type -Path "StyleCop\StyleCopWrapper.dll"
$scanner = new-object StyleCopWrapper.Wrapper

# Set the common scan options, 
$scanner.MaximumViolationCount = 1000
$scanner.ShowOutput = $true
$scanner.CacheResults = $false
$scanner.ForceFullAnalysis = $true
$scanner.AdditionalAddInPaths = @($pwd) # in in local path as we place stylecop.csharp.rules.dll here
$scanner.TreatViolationsErrorsAsWarnings = $treatViolationsErrorsAsWarnings

# look for .csproj files
foreach ($projfile in Get-ChildItem $sourcefolder -Filter *.csproj -Recurse)
{
   write-host ("Processing the folder [{0}]" -f $projfile.Directory)

   # find a set of rules closest to the .csproj file
   $settings = Join-Path -path $projfile.Directory -childpath "settings.stylecop"
   if (Test-Path $settings)
   {
        write-host "Using found settings.stylecop file same folder as .csproj file"
        $scanner.SettingsFile = $settings
   }  else
   {
       $settings = Join-Path -path $sourcefolder -childpath "settings.stylecop"
       if (Test-Path $settings)
       {
            write-host "Using settings.stylecop file in solution folder"
            $scanner.SettingsFile = $settings
       } else 
       {
            write-host "Cannot find a local settings.stylecop file, using default rules"
            $scanner.SettingsFile = "." # we have to pass something as this is a required param
       }
   }

   $scanner.SourceFiles =  @($projfile.Directory)
   $scanner.XmlOutputFile = (join-path $stagingfolder $projfile.BaseName) +".stylecop.xml"
   $scanner.LogFile =  (join-path $stagingfolder $projfile.BaseName) +".stylecop.log"
    
   # Do the scan
   $scanner.Scan()

    # Display the results
    Write-Host ("`n")
    write-host ("Base folder`t[{0}]" -f $projfile.Directory) -ForegroundColor Green
    write-host ("Settings `t[{0}]" -f $scanner.SettingsFile) -ForegroundColor Green
    write-host ("Succeeded `t[{0}]" -f $scanner.Succeeded) -ForegroundColor Green
    write-host ("Violations `t[{0}]" -f $scanner.ViolationCount) -ForegroundColor Green
    Write-Host ("Log file `t[{0}]" -f $scanner.LogFile) -ForegroundColor Green
    Write-Host ("XML results`t[{0}]" -f $scanner.XmlOutputFile) -ForegroundColor Green

    $totalViolations += $scanner.ViolationCount
    $projectsScanned ++
    
    if ($scanner.Succeeded -eq $false)
    {
      # any failure fails the whole run
      $overallSuccess = $false
    }

}

# the output summary
Write-Host ("`n")
if ($overallSuccess -eq $false)
{
   Write-Error ("StyleCop found [{0}] violations across [{1}] projects" -f $totalViolations, $projectsScanned)
} 
elseif ($totalViolations -gt 0 -and $treatViolationsErrorsAsWarnings -eq $true)
{
    Write-Warning ("StyleCop found [{0}] violations warnings across [{1}] projects" -f $totalViolations, $projectsScanned)
} 
else
{
   Write-Host ("StyleCop found [{0}] violations warnings across [{1}] projects" -f $totalViolations, $projectsScanned) -ForegroundColor Green
}