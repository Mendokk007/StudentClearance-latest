using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace CarDealership
{
    [HubName("clearanceHub")]
    public class ClearanceHub : Hub
    {
        public async Task JoinStudentGroup(string username)
        {
            // Use unique group names to avoid conflicts
            string groupName = $"student_{username}";
            await Groups.Add(Context.ConnectionId, groupName);
            System.Diagnostics.Debug.WriteLine($"Student {username} joined group {groupName}");
        }

        public async Task JoinAdminGroup(string department)
        {
            string groupName = $"admin_{department}";
            await Groups.Add(Context.ConnectionId, groupName);
            System.Diagnostics.Debug.WriteLine($"Admin for {department} joined group {groupName}");
        }

        public async Task LeaveStudentGroup(string username)
        {
            string groupName = $"student_{username}";
            await Groups.Remove(Context.ConnectionId, groupName);
            System.Diagnostics.Debug.WriteLine($"Student {username} left group {groupName}");
        }

        public async Task LeaveAdminGroup(string department)
        {
            string groupName = $"admin_{department}";
            await Groups.Remove(Context.ConnectionId, groupName);
            System.Diagnostics.Debug.WriteLine($"Admin for {department} left group {groupName}");
        }

        public async Task NotifyNewSubmission(string department, string studentName, string studentUsername)
        {
            string groupName = $"admin_{department}";
            await Clients.Group(groupName).newSubmission(
                $"New submission from {studentName} for {department}",
                studentUsername
            );
        }

        public async Task NotifyStatusUpdate(string studentUsername, string department, string status)
        {
            string groupName = $"student_{studentUsername}";
            await Clients.Group(groupName).statusUpdated(department, status);
            System.Diagnostics.Debug.WriteLine($"Notified {studentUsername} that {department} status is {status}");
        }

        public override async Task OnConnected()
        {
            System.Diagnostics.Debug.WriteLine($"Client connected: {Context.ConnectionId}");
            await base.OnConnected();
        }

        public override async Task OnDisconnected(bool stopCalled)
        {
            System.Diagnostics.Debug.WriteLine($"Client disconnected: {Context.ConnectionId}");
            await base.OnDisconnected(stopCalled);
        }

        public override async Task OnReconnected()
        {
            System.Diagnostics.Debug.WriteLine($"Client reconnected: {Context.ConnectionId}");
            await base.OnReconnected();
        }
    }
}