
namespace StyleCopWrapper
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using global::StyleCop;

    public class Wrapper
    {
        /// <summary>
        /// The default maximum number of violations that can be discovered
        /// </summary>
        private const int DefaultViolationLimit = 10000;

        /// <summary>
        /// The status of the analysis
        /// </summary>
        private bool exitCode = true;

        /// <summary>
        /// The files that vioaltions encountered
        /// </summary>
        private List<string> violations = new List<string>();

        /// <summary>
        /// The maximum violation count
        /// </summary>
        private int violationLimit;

        /// <summary>
        /// Sets the maximum violation count before scanning is halted.
        /// </summary>
        public int MaximumViolationCount { get; set; }

        /// <summary>
        /// Gets the number of violations found.
        /// </summary>
        public int ViolationCount { get; set; }

        /// <summary>
        /// Sets a value indicating whether to show names of files scanned to the build log
        /// </summary>
        public bool ShowOutput { get; set; }

        /// <summary>
        /// Gets whether the scan succeeded.
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Sets a value indicating whether StyleCop should write cache files to disk after performing an analysis. Default is false.
        /// </summary>
        public bool CacheResults { get; set; }

        /// <summary>
        /// Sets a value indicating whether StyleCop should ignore cached results and perform a clean analysis. 
        /// </summary>
        public bool ForceFullAnalysis { get; set; }

        /// <summary>
        /// Sets the name for the XML log file produced by the StyleCop runner
        /// </summary>
        public string XmlOutputFile { get; set; }

        /// <summary>
        /// Sets the text log file that list the violation 
        /// </summary>
        public string LogFile { get; set; }

        /// <summary>
        /// Sets the source files path or list of specific files
        /// </summary>
        public string[] SourceFiles { get; set; }

        /// <summary>
        /// Sets the path to the settings file that defines the rules
        /// </summary>
        public string SettingsFile { get; set; }

        /// <summary>
        /// Set the location of any custom addins
        /// </summary>
        public string[] AdditionalAddInPaths { get; set; }

        /// <summary>
        /// Set to true to treat all stylecop violations as warnings
        /// </summary>
        public bool TreatViolationsErrorsAsWarnings { get; set; }

        public void Scan()
        {
            // Clear the violation count and set the violation limit for the project.
            this.violations = new List<string>();
            this.violationLimit = 0;

            if (this.MaximumViolationCount != 0)
            {
                this.violationLimit = this.MaximumViolationCount;
            }

            if (this.violationLimit == 0)
            {
                this.violationLimit = DefaultViolationLimit;
            }

            // Get settings files (if null or empty use null filename so it uses default).
            string settingsFileName = string.Empty;
            if (string.IsNullOrEmpty(this.SettingsFile) == false)
            {
                settingsFileName = this.SettingsFile;
            }

            // Get addin paths.
            List<string> addinPaths = new List<string>();
            if (this.AdditionalAddInPaths != null)
            {
                addinPaths.AddRange(this.AdditionalAddInPaths);
            }

            // Create the StyleCop console. But do not initialise the addins as this can cause modal dialogs to be shown on errors
            var console = new StyleCopConsole(settingsFileName, this.CacheResults, this.XmlOutputFile, null, false);

            // make sure the UI is not dispayed on error
            console.Core.DisplayUI = false;

            // declare the add-ins to load
            console.Core.Initialize(addinPaths, true);

            // Create the configuration.
            Configuration configuration = new Configuration(new string[0]);

            // Create a CodeProject object for these files. we use a time stamp for the key and the current directory for the cache location
            CodeProject project = new CodeProject(DateTime.Now.ToLongTimeString().GetHashCode(), @".\", configuration);

            // Add each source file to this project.
            if (this.SourceFiles == null)
            {
                throw new ArgumentException("The list of source files must be specified");
            } else
            {
                foreach (var inputSourceLocation in this.SourceFiles)
                {
                    // could be a path or a file
                    if (System.IO.File.Exists(inputSourceLocation))
                    {
                        if (this.ShowOutput)
                        {
                            Console.WriteLine("Adding file to check [{0}]", inputSourceLocation);
                        }

                        console.Core.Environment.AddSourceCode(project, inputSourceLocation, null);
                    }
                    else if (System.IO.Directory.Exists(inputSourceLocation))
                    {
                        foreach (var fileInDirectory in System.IO.Directory.GetFiles(inputSourceLocation, "*.cs", SearchOption.AllDirectories))
                        {
                            if (this.ShowOutput)
                            {
                                Console.WriteLine("Adding file to check [{0}]", fileInDirectory);
                            }

                            console.Core.Environment.AddSourceCode(project, fileInDirectory, null);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Cannot add file to check [{0}]", inputSourceLocation);
                    }
                }

                try
                {
                    // Subscribe to events
                    console.OutputGenerated += this.OnOutputGenerated;
                    console.ViolationEncountered += this.OnViolationEncountered;

                    // Analyze the source files
                    CodeProject[] projects = new[] { project };
                    console.Start(projects, this.ForceFullAnalysis);
                }
                finally
                {
                    // Unsubscribe from events
                    console.OutputGenerated -= this.OnOutputGenerated;
                    console.ViolationEncountered -= this.OnViolationEncountered;
                }
            }

            // log the results to disk as a simple list if there have been failures AND LogFile is specified
            if (string.IsNullOrEmpty(this.LogFile) == false && this.exitCode == false)
            {
                using (StreamWriter streamWriter = new StreamWriter(this.LogFile, false, Encoding.UTF8))
                {
                    foreach (string i in this.violations)
                    {
                        streamWriter.WriteLine(i);
                    }
                }
            }

            this.Succeeded = this.exitCode;
            this.ViolationCount = this.violations.Count;
        }

        private void OnOutputGenerated(object sender, OutputEventArgs e)
        {
            lock (this)
            {
                Console.WriteLine(e.Output.Trim());
            }
        }

        private void OnViolationEncountered(object sender, ViolationEventArgs e)
        {
            if (this.violationLimit < 0 || this.violations.Count < this.violationLimit)
            {
                // Does the violation qualify for breaking the build?
                if (!(e.Warning || this.TreatViolationsErrorsAsWarnings))
                {
                    this.exitCode = false;
                }

                string file = string.Empty;
                if (e.SourceCode != null && !string.IsNullOrEmpty(e.SourceCode.Path))
                {
                    file = e.SourceCode.Path;
                }
                else if (e.Element != null &&
                    e.Element.Document != null &&
                    e.Element.Document.SourceCode != null &&
                    e.Element.Document.SourceCode.Path != null)
                {
                    file = e.Element.Document.SourceCode.Path;
                }

                file += string.Format(CultureInfo.CurrentUICulture, ". LineNumber: {0}, ", e.LineNumber.ToString(CultureInfo.CurrentCulture));
                file += string.Format(CultureInfo.CurrentUICulture, "CheckId: {0}, ", e.Violation.Rule.CheckId ?? string.Empty);
                file += string.Format(CultureInfo.CurrentUICulture, "Message: {0}, ", e.Message);
                this.violations.Add(file);

                // Prepend the rule check-id to the message.
                string message = string.Concat(e.Violation.Rule.CheckId ?? "NoRuleCheckId", ": ", e.Message);

                lock (this)
                {
                    if (e.Warning || this.TreatViolationsErrorsAsWarnings)
                    {
                        Console.WriteLine("{0} [{1}] Line {2}", message, file, e.LineNumber);
                    }
                    else
                    {
                        Console.WriteLine("{0} [{1}] Line {2}", message, file, e.LineNumber);
                    }
                }
            }
        }

    }
}
