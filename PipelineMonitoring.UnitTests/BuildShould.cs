using PipelineMonitoring.Model.Builds;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PipelineMonitoring.UnitTests
{
    [TestClass]
    public class BuildShould
    {
        [TestMethod]
        public void ReturnTheExpectedCardClassesWhenTheStatusIsInProgressAndGetCardClassesIsCalled()
        {
            var build = new Build
            {
                Status = "inProgress"
            };

            Assert.AreEqual("bg-info text-white", build.GetCardClasses());
        }

        [DataTestMethod]
        [DataRow("succeeded", "bg-success text-white")]
        [DataRow("failed", "bg-danger text-white")]
        [DataRow("cancelled", "bg-warning text-white")]
        [DataRow("none", "bg-warning text-white")]
        [DataRow("partiallySucceeded", "bg-warning text-white")]
        [DataRow("SOMETHINGUNEXPECTED", "bg-light text-dark")]
        public void ReturnTheExpectedCardClassesWhenStatusIsNotInProgressAndGetCardClassesIsCalled(string result, string expectedCardClasses)
        {
            var build = new Build
            {
                Result = result
            };

            Assert.AreEqual(expectedCardClasses, build.GetCardClasses());
        }
    }
}
