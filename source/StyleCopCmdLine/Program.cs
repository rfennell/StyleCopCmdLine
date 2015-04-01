namespace StyleCopCmdLine
{
    using System;

    class CommandLineWrapper
    {
        static void Main(string[] args)
        {
            var options = new Options();
            try
            {
                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    // Values are available here
                    if (options.Verbose)
                    {
                        Console.WriteLine(options.GetUsage());
                        
                        Console.WriteLine("SourceFiles: {0}", options.SourceFiles);
                        Console.WriteLine("SettingsFile: {0}", options.SettingsFile);
                        Console.WriteLine("MaximumViolationCount: {0}", options.MaximumViolationCount);
                        Console.WriteLine("ShowOutput: {0}", options.ShowOutput);
                        Console.WriteLine("CacheResults: {0}", options.CacheResults);
                        Console.WriteLine("ForceFullAnalysis: {0}", options.ForceFullAnalysis);
                        Console.WriteLine("XmlOutputFile: {0}", options.XmlOutputFile);
                        Console.WriteLine("LogFile: {0}", options.LogFile);
                        Console.WriteLine("TreatViolationsErrorsAsWarnings: {0}", options.TreatViolationsErrorsAsWarnings);
                        Console.WriteLine("AdditionalAddInPaths: {0}", options.AdditionalAddInPaths);


                        Console.WriteLine(Environment.NewLine);

                    }

                    var scanner = new StyleCopWrapper.Wrapper()
                    {
                        MaximumViolationCount = options.MaximumViolationCount,
                        ShowOutput = options.ShowOutput,
                        CacheResults = options.CacheResults,
                        ForceFullAnalysis = options.ForceFullAnalysis,
                        XmlOutputFile = options.XmlOutputFile,
                        LogFile = options.LogFile,
                        SourceFiles = options.SourceFiles,
                        SettingsFile = options.SettingsFile,
                        AdditionalAddInPaths =options.AdditionalAddInPaths,
                        TreatViolationsErrorsAsWarnings = options.TreatViolationsErrorsAsWarnings
                    };

                    scanner.Scan();

                    Console.WriteLine("Succeeded [{0}]", scanner.Succeeded);
                    Console.WriteLine("Violation count [{0}]", scanner.ViolationCount);
                    
                }
            }
            catch (CommandLine.ParserException ex)
            {
                Console.WriteLine("StyleCopCmdLine: Parameter error");
                Console.WriteLine(ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine(ex.InnerException.Message);
                }
            }
          
        }
    }

}
