#
# Script.ps1
#
Add-Type -Path "C:\tmp\StyleCopCmdLine\StyleCopWrapper\bin\Debug\StyleCopWrapper.dll"

$scanner = new-object StyleCopWrapper.Wrapper
  
$scanner.MaximumViolationCount = 1000
$scanner.ShowOutput = $true
$scanner.CacheResults = $false
$scanner.ForceFullAnalysis = $true
$scanner.XmlOutputFile = "$pwd\out.xml"
$scanner.LogFile = "$pwd\log.txt"
$scanner.SourceFiles =  @("C:\projects\github\CustomActivities\Source\Tests\Activities.StyleCop.Tests\TestFiles\FileWith7Errors.cs" )
$scanner.SettingsFile = "C:\projects\github\CustomActivities\Source\Tests\Activities.StyleCop.Tests\TestFiles\AllSettingsEnabled.StyleCop"
$scanner.AdditionalAddInPaths = @("C:\Program Files (x86)\StyleCop 4.7" )
$scanner.TreatViolationsErrorsAsWarnings = $false

$scanner.Scan()

write-host ("Succeeded [{0}]" -f $scanner.Succeeded)
write-host ("Violation count [{0}]" -f $scanner.ViolationCount)