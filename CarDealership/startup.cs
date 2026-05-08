using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(CarDealership.Startup))]

namespace CarDealership
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Enable CORS if your forms are on different ports
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            // Configure SignalR with detailed errors for debugging
            var hubConfiguration = new HubConfiguration
            {
                EnableDetailedErrors = true,
                EnableJavaScriptProxies = true
            };

            // Map SignalR hub
            app.MapSignalR(hubConfiguration);
        }
    }
}