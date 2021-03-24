using System;

namespace PipelineMonitoring.Services
{
    public class EventService
    {
        public EventService()
        {
            // Sending of an event fails if nothing listens to that event
            FilterChanged += (sender, args) => { };
        }

        public void SendFilterChanged()
        {
            FilterChanged(this, new FilterEventArgs());
        }

        public event EventHandler<FilterEventArgs> FilterChanged;
    }
}
