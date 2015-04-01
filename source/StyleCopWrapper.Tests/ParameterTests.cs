namespace StyleCopWrapper.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParameterTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Exception_violations_found_if_required_parameters_missing()
        {
            // arrange
            // create the activity
            var target = new StyleCopWrapper.Wrapper() ;

            // act
            target.Scan();

            // assert
            // trapped by exception handler
        }
    }
}
