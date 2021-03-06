﻿# Make sure we are in 32bit mode
if ($env:Processor_Architecture -ne "x86")   
{ write-warning 'Launching x86 PowerShell'
&"$env:windir\syswow64\windowspowershell\v1.0\powershell.exe" -noninteractive -noprofile -file $myinvocation.Mycommand.path -executionpolicy bypass
exit
}

$VerbosePreference ='Continue' # equiv to -verbose
write-verbose "Running in 32bit PowerShell at this point as dictionaries loaded by StyleCop are 32bit only."

# Make sure we have a log folder
New-Item -ItemType Directory -Force -Path "$PSScriptRoot\logs" >$null 2>&1
Add-Type -Path "$PSScriptRoot\bin\debug\StyleCopWrapper.dll" 


Describe "PowerShell - StyleCop Spelling Test" {

	$scanner = new-object StyleCopWrapper.Wrapper
  
	$scanner.MaximumViolationCount = 1000
	$scanner.ShowOutput = $true
	$scanner.CacheResults = $false
	$scanner.ForceFullAnalysis = $true
	$scanner.XmlOutputFile = "$PSScriptRoot\logs\out2.xml"
	$scanner.LogFile = "$PSScriptRoot\logs\log2.txt"
	$scanner.SourceFiles =  @("$PSScriptRoot\TestFiles\FileWithSA1650Errors.cs" )
	$scanner.SettingsFile = "$PSScriptRoot\TestFiles\SettingsOnlySA1650.StyleCop"
	$scanner.AdditionalAddInPaths = "$PSScriptRoot\bin\debug"
	$scanner.TreatViolationsErrorsAsWarnings = $false
  
    It "File has 1 issue" {
		$scanner.Scan()
		$scanner.Succeeded | Should be $false
		$scanner.ViolationCount | Should be 1
		}
}
