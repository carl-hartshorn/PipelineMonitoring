using PipelineMonitoring.Model.Releases;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PipelineMonitoring.UnitTests;

[TestClass]
public class ReleaseShould
{
    [TestMethod]
    public void ReturnTheExpectedCardClassesForWhenAllEnvironmentsHaveAStatusOfNotStartedWhenGetCardClassesIsCalled()
    {
        var release = new Release
        {
            Environments = new[]
            {
                new Environment
                {
                    Status = "notStarted"
                },
                new Environment
                {
                    Status = "notStarted"
                }
            }
        };

        Assert.AreEqual("bg-secondary text-white", release.CardClasses);
    }

    [DataTestMethod]
    [DataRow("cancelled", "bg-warning text-white")]
    [DataRow("inProgress", "bg-info text-white")]
    [DataRow("partiallySucceeded", "bg-warning text-white")]
    [DataRow("queued", "bg-info text-white")]
    [DataRow("rejected", "bg-danger text-white")]
    [DataRow("scheduled", "bg-info text-white")]
    [DataRow("succeeded", "bg-success text-white")]
    [DataRow("undefined", "bg-light text-dark")]
    [DataRow("SOMETHINGUNEXPECTED", "bg-light text-dark")]
    public void ReturnTheExpectedCardClassesForTheLastEnvironmentWhichDoesNotHaveAStatusOfNotStartedWhenGetCardClassesIsCalled(string status, string expectedCardClasses)
    {
        var release = new Release
        {
            Environments = new []
            {
                new Environment
                {
                    Status = status
                },
                new Environment
                {
                    Status = "notStarted"
                }
            }
        };

        Assert.AreEqual(expectedCardClasses, release.CardClasses);
    }
}