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
            var service = new EventService();
            service.FilterChanged += (sender, eventArgs) => handlerTriggered = true ;

            service.SendFilterChanged();

            Assert.IsTrue(handlerTriggered);
        }
    }
}
