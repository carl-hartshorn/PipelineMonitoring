using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineMonitoring.AzureDevOps.Builds;
using PipelineMonitoring.Shared;

namespace PipelineMonitoring.UnitTests;

[TestClass]
public class BuildsListShould
{
    private readonly Fixture _fixture = new();
    
    [TestMethod]
    public void ReturnTheExpectedCardClassesWhenTheBuildStatusIsInProgressAndGetCardClassesIsCalled()
    {
        var build = _fixture
            .Build<Build>()
            .With(build => build.Status, BuildStatus.InProgress)
            .Create();

        var cardClasses = BuildsList.GetCardClasses(build);

        Assert.AreEqual("bg-info text-white", cardClasses);
    }

    [DataTestMethod]
    [DataRow(BuildResult.Unknown, "bg-light text-dark")]
    [DataRow(BuildResult.Canceled, "bg-warning text-white")]
    [DataRow(BuildResult.Failed, "bg-danger text-white")]
    [DataRow(BuildResult.None, "bg-warning text-white")]
    [DataRow(BuildResult.PartiallySucceeded, "bg-warning text-white")]
    [DataRow(BuildResult.Succeeded, "bg-success text-white")]
    public void ReturnTheExpectedCardClassesWhenTheBuildStatusIsNotInProgressAndGetCardClassesIsCalled(
        BuildResult result,
        string expectedCardClasses)
    {
        var build = _fixture
            .Build<Build>()
            .With(build => build.Status, BuildStatus.Completed)
            .With(build => build.Result, result)
            .Create();

        var cardClasses = BuildsList.GetCardClasses(build);

        Assert.AreEqual(expectedCardClasses, cardClasses);
    }
}