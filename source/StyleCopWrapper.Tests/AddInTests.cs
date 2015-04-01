namespace StyleCopWrapper.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    [TestClass]
    public class AddInTests
    {
      

        [TestMethod]
        public void Extra_rules_can_loaded_from_a_directory_that_is_not_a_sub_directory_of_current_location()
        {
            // arrange
             var resultsFile = "StyleCop.Cache";
            System.IO.File.Delete(resultsFile);

            // create the activity
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWith7Errors.cs" },
                SettingsFile = @"TestFiles\AllSettingsEnabled.StyleCop",
                AdditionalAddInPaths = new string[] { @"C:\Program Files (x86)\StyleCop 4.7", @"..\..\..\CustomStyleCopRule\bin\Debug" }, // the directory cannot be a sub directory of current as this is automatically scanned
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(false, target.Succeeded);
            Assert.AreEqual(8, target.ViolationCount);// 7 core violations + the extra custom one
        }
    }
}
