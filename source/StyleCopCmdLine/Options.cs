using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StyleCopCmdLine
{
    class Options
    {
        [OptionArray('f', "SourceFiles", HelpText = "The files or folders to scan. Multiple files or folders can be listed", Required = true)]
        public string[] SourceFiles { get; set; }


        [Option('s', "SettingsFile", Required = true, HelpText = "The settings to use.")]
        public string SettingsFile { get; set; }

        [Option("AdditionalAddInPaths", Required = false, HelpText = "The paths to rules files. Multiple folders can be listed", DefaultValue = new string[] { @"C:\Program Files (x86)\StyleCop 4.7" })]
        public string[] AdditionalAddInPaths { get; set; }

        [Option("ShowOutput", Required = false, DefaultValue = false, HelpText = "Show the addin of files to scan output in the log.")]
        public bool ShowOutput { get; set; }


        [Option("CacheResults", Required = false, DefaultValue = false, HelpText = "Cache scan results.")]
        public bool CacheResults { get; set; }

        [Option("XmlOutputFile", Required = false, DefaultValue = @".\stylecop.xml", HelpText = "Xml Output File.")]
        public string XmlOutputFile { get; set; }

        [Option("LogFile", Required = false, DefaultValue = @".\stylecop.log", HelpText = "Log File.")]
        public string LogFile { get; set; }

        [Option("ForceFullAnalysis", Required = false, DefaultValue = true, HelpText = "Force a full analysis.")]
        public bool ForceFullAnalysis { get; set; }

        [Option("TreatViolationsErrorsAsWarnings", Required = false, DefaultValue = true, HelpText = "Treat violation errors as warnings.")]
        public bool TreatViolationsErrorsAsWarnings { get; set; }


        

        [Option("MaximumViolationCount", Required = false, DefaultValue = 1000, HelpText = "Maximum violations before the scan is stopped.")]
        public int MaximumViolationCount { get; set; }

        [Option('v', "verbose", DefaultValue = true, HelpText = "Prints the configuration messages to standard output.")]
        public bool Verbose { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            return HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
        }
    }
}
