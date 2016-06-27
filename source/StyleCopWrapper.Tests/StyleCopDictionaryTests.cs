using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StyleCopWrapper.Tests
{
    [TestClass]
    public class StyleCopDictionaryTests
    {
        [TestMethod]
        public void Check_a_file_with_spelling_mistake_finds_error()
        {
            // arrange
            var target = new StyleCopWrapper.Wrapper()
            {
                SourceFiles = new string[] { @"TestFiles\FileWithSA1650Errors.cs" },
                SettingsFile = @"TestFiles\SettingsOnlySA1650.StyleCop",
                AdditionalAddInPaths = new string[] { @"\bin\Debug" }
            };

            // act
            target.Scan();

            // assert
            Assert.AreEqual(false, target.Succeeded);
            Assert.AreEqual(1, target.ViolationCount);
        }
    }
}
