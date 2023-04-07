using System.IO;
using NUnit.Framework;

namespace Asap2.Tests
{
    [TestFixture]
    public class ParserTests
    {
        [Test]
        [TestCase("testFile.a2l")]
        [TestCase("ASAP2_Demo_V161.a2l")]
        public void ParseA2LFiles(string fileName)
        {
            string fullPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\..", fileName);
            var errorReporter = new UnitTestErrorReporter();
            var parser = new Asap2.Parser(fullPath, errorReporter);
            var tree = parser.DoParse();

            Assert.That(errorReporter.Errors.Count, Is.Zero, () => "Had errors: " + string.Join("\r\n - ", errorReporter.Errors));
            Assert.That(errorReporter.Warnings.Count, Is.Zero, () => "Had warnings: " + string.Join("\r\n - ", errorReporter.Warnings));
            Assert.That(tree, Is.Not.Null);
        }

        [Test]
        [TestCase("testFile.a2l")]
        [TestCase("ASAP2_Demo_V161.a2l")]
        public void ValidateA2LFiles(string fileName)
        {
            string fullPath = Path.Combine(TestContext.CurrentContext.TestDirectory, @"..\..\..\..", fileName);
            var errorReporter = new UnitTestErrorReporter();
            var parser = new Asap2.Parser(fullPath, errorReporter);
            var tree = parser.DoParse();

            Assert.That(tree, Is.Not.Null);
            errorReporter = new UnitTestErrorReporter();
            tree.Validate(errorReporter);

            Assert.That(errorReporter.Errors.Count, Is.Zero, () => "Had errors: " + string.Join("\r\n - ", errorReporter.Errors));
            Assert.That(errorReporter.Warnings.Count, Is.Zero, () => "Had warnings: " + string.Join("\r\n - ", errorReporter.Warnings));
            Assert.That(tree, Is.Not.Null);
        }
    }
}