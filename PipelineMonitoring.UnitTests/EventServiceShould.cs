using PipelineMonitoring.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PipelineMonitoring.UnitTests
{
    [TestClass]
    public class EventServiceShould
    {
        [TestMethod]
        public void SendFilterChangedWhenFilterChangedIsCalled()
        {
            var handlerTriggered = false;
            var service = CreateEventService();
            service.FilterChanged += (sender, eventArgs) => handlerTriggered = true ;

            service.SendFilterChanged();

            Assert.IsTrue(handlerTriggered);
        }

        private EventService CreateEventService()
            => new EventService();
    }
}
