namespace StyleCopWrapper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Xml.XPath;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LoggingTests
    {
        [TestMethod]
        public void Check_a_file_with_no_issues_and_defaults_rules_will_not_create_a_text_logfile()
        {
            // arrange
            var fileName = "LogFile.Txt";
            System.IO.File.Delete(fileName);

            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWith0Errors.cs" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"C:\Program Files (x86)\StyleCop 4.7" },
                LogFile = fileName
            };

            // act
            target.Scan();

            // assert
            Assert.IsFalse(System.IO.File.Exists(fileName));
        }

#if DEBUG
    // test relies on Debug.WriteLine
        [TestMethod]
        public void Can_choose_to_list_a_file_added_in_the_build_log()
        {
            // arrange
            var monitor = new DebugMonitor("Adding file to check");
            Trace.Listeners.Add(monitor);

            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWith7Errors.cs" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"C:\Program Files (x86)\StyleCop 4.7" },
                ShowOutput = true
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(1, monitor.Writes);
        }

        [TestMethod]
        public void Can_choose_to_not_list_a_file_added_in_the_build_log()
        {
            // arrange
            var monitor = new DebugMonitor("Adding file to check");
            Trace.Listeners.Add(monitor);

            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWith7Errors.cs" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"C:\Program Files (x86)\StyleCop 4.7" },
                ShowOutput = false
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(0, monitor.Writes);
        }

        [TestMethod]
        public void Can_choose_to_not_list_a_directory_of_files_added_in_the_build_log()
        {
            // arrange
            var monitor = new DebugMonitor("Adding file to check");
            Trace.Listeners.Add(monitor);

            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"C:\Program Files (x86)\StyleCop 4.7" },
                ShowOutput = false
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(0, monitor.Writes);
        }

        [TestMethod]
        public void Can_choose_to_list_a_directory_of_files_added_in_the_build_log()
        {
            // arrange
            var monitor = new DebugMonitor("Adding file to check");
            Trace.Listeners.Add(monitor);

            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"C:\Program Files (x86)\StyleCop 4.7" },
                ShowOutput = true
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(3, monitor.Writes);
        }
#endif

        [TestMethod]
        public void Can_set_the_name_of_the_name_of_output_XML_file()
        {
            // arrange
            var resultsFile = "out.xml";
            System.IO.File.Delete(resultsFile);

            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWith7Errors.cs" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"C:\Program Files (x86)\StyleCop 4.7" },
                XmlOutputFile = resultsFile
            };

            // act
            target.Scan();

            // assert
            Assert.IsTrue(System.IO.File.Exists(resultsFile));
            var document = new XPathDocument(resultsFile);
            var nav = document.CreateNavigator();
            Assert.AreEqual(7d, nav.Evaluate("count(/StyleCopViolations/Violation)"));
        }
    }
}
