using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceTestClientAgent.UnitTests
{
    [TestClass]
    public class ListExtensionsPickOneRandomlyShould
    {
        [TestMethod]
        public void PickTheOnlyElementFromAListOfOne()
        {
            var list = new List<int>(new[] { 111 });
            Assert.AreEqual(111, list.PickOneRandomly());
        }

        [TestMethod]
        public void ThrowAnArgumentExceptionIfTheListIsEmpty()
        {
            var list = new List<int>();
            Assert.ThrowsException<ArgumentException>(
                () => list.PickOneRandomly(),
                "List is empty.");
        }

        [TestMethod]
        public void PickAnyElementFromAListOfMoreThanOne()
        {
            var values = new[] { 111, 222 };
            var list = new List<int>(values);
            Assert.IsTrue(
                values.Contains(
                    list.PickOneRandomly()));
        }
    }
}
