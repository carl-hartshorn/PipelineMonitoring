using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PipelineMonitoring.AzureDevOps.Releases;
using PipelineMonitoring.Shared;
using Environment = PipelineMonitoring.AzureDevOps.Releases.Environment;

namespace PipelineMonitoring.UnitTests;

[TestClass]
public class ReleasesListShould
{
    private readonly Fixture _fixture = new();
    
    [TestMethod]
    public void ReturnTheExpectedCardClassesWhenAllReleaseEnvironmentsHaveAStatusOfNotStarted()
    {
        var environments = _fixture
            .Build<Environment>()
            .With(environment => environment.Status, EnvironmentStatus.NotStarted)
            .CreateMany();

        var release = _fixture
            .Build<Release>()
            .With(release => release.Environments, environments)
            .Create();

        var cardClasses = ReleasesList.GetCardClasses(release);

        Assert.AreEqual("bg-secondary text-white", cardClasses);
    }

    [DataTestMethod]
    [DataRow(EnvironmentStatus.Unknown, "bg-light text-dark")]
    [DataRow(EnvironmentStatus.Canceled, "bg-warning text-white")]
    [DataRow(EnvironmentStatus.InProgress, "bg-info text-white")]
    [DataRow(EnvironmentStatus.PartiallySucceeded, "bg-warning text-white")]
    [DataRow(EnvironmentStatus.Queued, "bg-info text-white")]
    [DataRow(EnvironmentStatus.Rejected, "bg-danger text-white")]
    [DataRow(EnvironmentStatus.Scheduled, "bg-info text-white")]
    [DataRow(EnvironmentStatus.Succeeded, "bg-success text-white")]
    [DataRow(EnvironmentStatus.Undefined, "bg-light text-dark")]
    public void ReturnTheExpectedCardClassesForTheLastReleaseEnvironmentWhichDoesNotHaveAStatusOfNotStarted(
        EnvironmentStatus status,
        string expectedCardClasses)
    {
        var environments = _fixture
            .Build<Environment>()
            .With(environment => environment.Status, EnvironmentStatus.NotStarted)
            .CreateMany();

        var environmentWithoutNotStartedStatus = _fixture
            .Build<Environment>()
            .With(environment => environment.Status, status)
            .Create();

        environments = environments.Prepend(environmentWithoutNotStartedStatus);

        var release = _fixture
            .Build<Release>()
            .With(release => release.Environments, environments)
            .Create();

        var cardClasses = ReleasesList.GetCardClasses(release);

        Assert.AreEqual(expectedCardClasses, cardClasses);
    }
}