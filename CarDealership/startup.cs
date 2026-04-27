using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CarDealership.Startup))]

namespace CarDealership
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfiguration = new Microsoft.AspNet.SignalR.HubConfiguration();
            hubConfiguration.EnableDetailedErrors = true;
            app.MapSignalR(hubConfiguration);
        }
    }
}