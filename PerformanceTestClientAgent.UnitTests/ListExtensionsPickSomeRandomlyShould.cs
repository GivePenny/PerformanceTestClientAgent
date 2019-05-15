using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceTestClientAgent.UnitTests
{
    [TestClass]
    public class ListExtensionsPickSomeRandomlyShould
    {
        [TestMethod]
        public void ThrowAnArgumentExceptionIfTheListIsEmpty()
        {
            var list = new List<int>();

            Assert.ThrowsException<ArgumentException>(
                () => list.PickSomeRandomly(1).ToList(),
                "List is empty.");
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void ThrowAnArgumentExceptionWhenZeroOrFewerElementsAreRequested(int minimumNumberOfItemsToPick)
        {
            var list = new List<int> { 111 };

            Assert.ThrowsException<ArgumentException>(
                () => list.PickSomeRandomly(minimumNumberOfItemsToPick).ToList(),
                "minimumNumberOfItemsToPick must be greater than zero.");
        }

        [TestMethod]
        public void ThrowAnArgumentExceptionWhenMoreElementsAreRequestedThanAreInTheList()
        {
            var list = new List<int> { 111 };

            Assert.ThrowsException<ArgumentException>(
                () => list.PickSomeRandomly(2).ToList(),
                "Asked to pick at least 2 elements but the list only contains 1.");
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        public void PickAtLeastTheRequestedNumberOfElementsFromAList(int minimumNumberOfItemsToPick)
        {
            var values = new[] { 111, 222, 333 };
            var list = new List<int>(values);

            Assert.IsTrue(
                list.PickSomeRandomly(minimumNumberOfItemsToPick).Count() >= minimumNumberOfItemsToPick);
        }

        [TestMethod]
        public void NotPickAValueFromTheListMoreThanOnce()
        {
            var values = new[] { 111, 222, 333 };
            var list = new List<int>(values);

            var pickedValues = list.PickSomeRandomly(2);

            Assert.AreEqual(
                pickedValues.Count(),
                pickedValues.Distinct().Count());
        }
    }
}
