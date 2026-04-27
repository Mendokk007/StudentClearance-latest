using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;

namespace CarDealership
{
    [HubName("clearanceHub")]
    public class ClearanceHub : Hub
    {
        public void JoinStudentGroup(string username)
        {
            Groups.Add(Context.ConnectionId, username);
        }

        public void JoinAdminGroup(string department)
        {
            Groups.Add(Context.ConnectionId, $"admin_{department}");
        }

        public void NotifyNewSubmission(string department, string studentName)
        {
            Clients.Group($"admin_{department}").newSubmission(
                $"New submission from {studentName} for {department}");
        }

        public void NotifyStatusUpdate(string studentUsername, string department, string status)
        {
            Clients.Group(studentUsername).statusUpdated(department, status);
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
    }
}