namespace StyleCopWrapper.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// A set of test that check the passing of setting files into stylecop
    /// These rely on the paths beinbg correct for the source and setting files,
    /// that the StyleCop.CSharp and StyleCopCShare.Rules are in the same folder as the activity or the additionalAddInPath is set to their location
    /// and that the max violations are set to > 0 (defaults to 100000)
    /// </summary>
    [TestClass]
    public class StyleCopTests
    {
        [TestMethod]
        public void Check_a_single_file_with_default_rules_shows_violations_and_fails()
        {
            // arrange
            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWith7Errors.cs" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"\bin\Debug" }
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(false, target.Succeeded);
            Assert.AreEqual(7, target.ViolationCount);
        }

        [TestMethod]
        public void Check_a_maxviolationscount_used_when_violations_are_errors()
        {
            // arrange
            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWith7Errors.cs" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"\bin\Debug" },
                MaximumViolationCount = 3,
                TreatViolationsErrorsAsWarnings = false
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(false, target.Succeeded);
            Assert.AreEqual(3, target.ViolationCount);
        }

        [TestMethod]
        public void Check_a_maxviolationscount_used_when_violations_are_warnings()
        {
            // arrange
            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWith7Errors.cs" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"\bin\Debug" },
                MaximumViolationCount = 3,
                TreatViolationsErrorsAsWarnings = true
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(true, target.Succeeded);
            Assert.AreEqual(3, target.ViolationCount);
        }


        [TestMethod]
        public void Check_a_single_file_with_default_rules_shows_no_violations_and_passes()
        {
            // arrange
            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWith0Errors.cs" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"\bin\Debug" }
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(true, target.Succeeded);
            Assert.AreEqual(0, target.ViolationCount);

        }

        [TestMethod]
        public void Check_a_single_file_with_some_rules_disabled_shows_less_violations_and_fails()
        {

            // arrange
            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWith7Errors.cs" },
                SettingsFile = @"TestFiles\SettingsDisableSA1200.StyleCop",
                AdditionalAddInPaths = new string[] { @"\bin\Debug" }
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(false, target.Succeeded);
            Assert.AreEqual(3, target.ViolationCount);



        }

        [TestMethod]
        public void Check_a_directory_with_defaults_rules_shows_violations_and_fails()
        {

            // arrange
            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"\bin\Debug" }
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(false, target.Succeeded);
            Assert.AreEqual(20, target.ViolationCount);

        }

        [TestMethod]
        public void Check_a_directory_with_defaults_rules_will_creating_a_text_logfile_showing_violations()
        {
            // arrange
            var fileName = "LogFile.Txt";
            System.IO.File.Delete(fileName);


            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"\bin\Debug" },
                LogFile = fileName
            };

            // act
            target.Scan();

            // assert
            Assert.IsTrue(System.IO.File.Exists(fileName));
            Assert.AreEqual(20, System.IO.File.ReadAllLines(fileName).Length);
        }

        [TestMethod]
        public void Check_a_directory_with_limit_on_violation_count_shows_only_first_few_violations()
        {
            // arrange
            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"\bin\Debug" },
                MaximumViolationCount = 2
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(false, target.Succeeded);
            Assert.AreEqual(2, target.ViolationCount);
        }

        [TestMethod]
        public void Check_a_single_file_with_local_function_shows_no_violations_and_passes()
        {
            // arrange
            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWithLocalFunction.cs" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"\bin\Debug" }
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(true, target.Succeeded);
            Assert.AreEqual(0, target.ViolationCount);

        }
    }
}
