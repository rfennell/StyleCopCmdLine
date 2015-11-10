namespace StyleCopWrapper.Tests
{
    using System; 
    using System.Collections.Generic;
    using System.Xml.XPath;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CacheTests
    {
        [TestMethod]
        public void Setting_the_cache_option_causes_the_results_to_be_cached_in_the_default_directory()
        {
            // arrange
            var resultsFile = "StyleCop.Cache";
            System.IO.File.Delete(resultsFile);

            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWith7Errors.cs" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"\bin\Debug" },
                CacheResults = true,
            };

            // act
            target.Scan();

            // assert
            Assert.IsTrue(System.IO.File.Exists(resultsFile));
            var document = new XPathDocument(resultsFile);
            var nav = document.CreateNavigator();
            Assert.AreEqual(7d, nav.Evaluate("count(/stylecopresultscache/sourcecode/violations/violation)"));
        }

        [TestMethod]
        public void Not_setting_the_cache_option_causes_the_results_to_not_be_cached_in_the_default_directory()
        {
            // arrange
            var resultsFile = "StyleCop.Cache";
            System.IO.File.Delete(resultsFile);

            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWith7Errors.cs" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"\bin\Debug" },
                CacheResults = false,
            };

            // act
            target.Scan();

            // assert
            Assert.IsFalse(System.IO.File.Exists(resultsFile));
        }
    
    }
}
