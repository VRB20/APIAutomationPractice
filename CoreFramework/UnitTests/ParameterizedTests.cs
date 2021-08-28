using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace CoreFramework.UnitTests
{
    [TestFixture]
    public class ParameterizedTests
    {
        [TestCaseSource(nameof(TestStrings), new object[] { true })]
        public void LongNameWithEvenNumberOfCharacters(string name)
        {
            Assert.That(name.Length, Is.GreaterThan(5));
            bool hasEvenNumOfCharacters = (name.Length / 2) == 0;
        }

        [TestCaseSource(nameof(TestStrings), new object[] { false })]
        public void ShortName(string name)
        {
            Assert.That(name.Length, Is.LessThan(15));
        }

        static IEnumerable<string> TestStrings(bool generateLongTestCase)
        {
            if (generateLongTestCase)
                yield return "ThisIsAVeryLongNameThisIsAVeryLongName";
            yield return "SomeName";
            yield return "YetAnotherName";
        }



    }
}
