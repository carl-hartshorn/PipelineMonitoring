using System;
using System.Threading.Tasks;

namespace PipelineMonitoring.Services;

public class AzureDevOpsSettingsService
{
    private const string _localStorageKey = "AzureDevOpsSettings";

    private readonly LocalStorageService _localStorageService;
    private string _organisation;
    private string _project;

    public AzureDevOpsSettingsService(
        LocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;

        AzureDevOpsSettingsChanged += new EventHandler<AzureDevOpsSettingsChangedEventArgs>(
            async (sender, args) =>
            {
                await SaveToLocalStorage().ConfigureAwait(false);
            });
    }

    public virtual string Organisation
    {
        get => _organisation;
        set
        {
            _organisation = value;
            AzureDevOpsSettingsChanged(this, new AzureDevOpsSettingsChangedEventArgs());
        }
    }

    public virtual string Project
    {
        get => _project;
        set
        {
            _project = value;
            AzureDevOpsSettingsChanged(this, new AzureDevOpsSettingsChangedEventArgs());
        }
    }

    public virtual bool HasOrganisationAndProject
        => !string.IsNullOrWhiteSpace(Organisation)
           && !string.IsNullOrWhiteSpace(Project);

    public async Task InitialiseFromLocalStorage()
    {
        var settings = await _localStorageService.GetItem(_localStorageKey).ConfigureAwait(false);
        if (!string.IsNullOrWhiteSpace(settings))
        {
            Organisation = settings.Split(':')[0];
            Project = settings.Split(':')[1];
        }
    }

    private async Task SaveToLocalStorage()
    {
        await _localStorageService
            .SetItem(
                _localStorageKey,
                $"{Organisation}:{Project}")
            .ConfigureAwait(false);
    }

    public event EventHandler<AzureDevOpsSettingsChangedEventArgs> AzureDevOpsSettingsChanged;
}