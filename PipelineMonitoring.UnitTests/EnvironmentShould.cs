using PipelineMonitoring.Model.Releases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PipelineMonitoring.UnitTests
{
    [TestClass]
    public class EnvironmentShould
    {
        [DataTestMethod]
        [DataRow("cancelled", "bg-warning text-white")]
        [DataRow("inProgress", "bg-info text-white")]
        [DataRow("notStarted", "bg-secondary text-white")]
        [DataRow("partiallySucceeded", "bg-warning text-white")]
        [DataRow("queued", "bg-info text-white")]
        [DataRow("rejected", "bg-danger text-white")]
        [DataRow("scheduled", "bg-info text-white")]
        [DataRow("succeeded", "bg-success text-white")]
        [DataRow("undefined", "bg-light text-dark")]
        [DataRow("SOMETHINGUNEXPECTED", "bg-light text-dark")]
        public void ReturnTheExpectedCardClassesWhenGetCardClassesIsCalled(string status, string expectedCardClasses)
        {
            var environment = new Environment
            {
                Status = status
            };

            Assert.AreEqual(expectedCardClasses, environment.GetCardClasses());
        }
    }
}
