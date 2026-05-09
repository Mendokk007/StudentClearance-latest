using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace CarDealership
{
    [HubName("clearanceHub")]
    public class ClearanceHub : Hub
    {
        // =============================================
        // STUDENT GROUP METHODS
        // =============================================
        public async Task JoinStudentGroup(string username)
        {
            // Use unique group names to avoid conflicts
            string groupName = $"student_{username}";
            await Groups.Add(Context.ConnectionId, groupName);
            System.Diagnostics.Debug.WriteLine($"Student {username} joined group {groupName}");
        }

        public async Task LeaveStudentGroup(string username)
        {
            string groupName = $"student_{username}";
            await Groups.Remove(Context.ConnectionId, groupName);
            System.Diagnostics.Debug.WriteLine($"Student {username} left group {groupName}");
        }

        // =============================================
        // ADMIN GROUP METHODS (Department Clearance)
        // =============================================
        public async Task JoinAdminGroup(string department)
        {
            string groupName = $"admin_{department}";
            await Groups.Add(Context.ConnectionId, groupName);
            System.Diagnostics.Debug.WriteLine($"Admin for {department} joined group {groupName}");
        }

        public async Task LeaveAdminGroup(string department)
        {
            string groupName = $"admin_{department}";
            await Groups.Remove(Context.ConnectionId, groupName);
            System.Diagnostics.Debug.WriteLine($"Admin for {department} left group {groupName}");
        }

        // =============================================
        // INSTRUCTOR GROUP METHODS (Subject Clearance) - NEW
        // =============================================
        public async Task JoinInstructorGroup(string subject)
        {
            string groupName = $"instructor_{subject}";
            await Groups.Add(Context.ConnectionId, groupName);
            System.Diagnostics.Debug.WriteLine($"Instructor for {subject} joined group {groupName}");
        }

        public async Task LeaveInstructorGroup(string subject)
        {
            string groupName = $"instructor_{subject}";
            await Groups.Remove(Context.ConnectionId, groupName);
            System.Diagnostics.Debug.WriteLine($"Instructor for {subject} left group {groupName}");
        }

        // =============================================
        // NOTIFICATION METHODS
        // =============================================
        public async Task NotifyNewSubmission(string department, string studentName, string studentUsername)
        {
            string groupName = $"admin_{department}";
            await Clients.Group(groupName).newSubmission(
                $"New submission from {studentName} for {department}",
                studentUsername
            );
        }

        // NEW: Notify instructor of new subject submission
        public async Task NotifyNewSubjectSubmission(string subject, string studentName, string studentUsername)
        {
            string groupName = $"instructor_{subject}";
            await Clients.Group(groupName).newSubjectSubmission(
                $"New submission from {studentName} for {subject}",
                studentUsername
            );
        }

        public async Task NotifyStatusUpdate(string studentUsername, string department, string status)
        {
            string groupName = $"student_{studentUsername}";
            await Clients.Group(groupName).statusUpdated(department, status);
            System.Diagnostics.Debug.WriteLine($"Notified {studentUsername} that {department} status is {status}");
        }

        // NEW: Notify student of subject clearance status update
        public async Task NotifySubjectStatusUpdate(string studentUsername, string subject, string status)
        {
            string groupName = $"student_{studentUsername}";
            await Clients.Group(groupName).subjectStatusUpdated(subject, status);
            System.Diagnostics.Debug.WriteLine($"Notified {studentUsername} that {subject} status is {status}");
        }

        // =============================================
        // CONNECTION LIFECYCLE
        // =============================================
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