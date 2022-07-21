namespace PipelineMonitoring.Services;

public class EventService
{
    public EventService()
    {
        // Sending of an event fails if nothing listens to that event
        FilterChanged += (_, _) => { };
    }

    public void SendFilterChanged()
    {
        FilterChanged(this, new FilterEventArgs());
    }

    public event EventHandler<FilterEventArgs> FilterChanged;
}