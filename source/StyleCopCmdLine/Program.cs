namespace StyleCopCmdLine
{
    using System;

    class CommandLineWrapper
    {
        static void Main(string[] args)
        {
            var scanner = new StyleCopWrapper.Wrapper()
            {
                MaximumViolationCount = 1000,
                ShowOutput = true,
                CacheResults = false,
                ForceFullAnalysis = true,
                XmlOutputFile = "out.xml",
                LogFile = "log.txt",
                SourceFiles = new string[] { @"C:\projects\github\CustomActivities\Source\Tests\Activities.StyleCop.Tests\TestFiles\FileWith7Errors.cs" },
                SettingsFile = @"C:\projects\github\CustomActivities\Source\Tests\Activities.StyleCop.Tests\TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"C:\Program Files (x86)\StyleCop 4.7" },
                TreatViolationsErrorsAsWarnings = false
            };

            scanner.Scan();

            Console.WriteLine("Succeeded [{0}]", scanner.Succeeded);
            Console.WriteLine("Violation count [{0}]", scanner.ViolationCount);
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
    }
     
}
