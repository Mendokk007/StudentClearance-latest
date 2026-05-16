using System;
using System.Windows.Forms;

namespace CarDealership
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new AppContext());
        }
    }

    public class AppContext : ApplicationContext
    {
        private LoginForm _loginForm;
        private StudentDashboardForm _studentDashboardForm;
        private AdminForm _adminForm;
        private AdminFormSubjects _adminFormSubjects;
        private SuperAdminForm _superAdminForm;

        public AppContext()
        {
            OpenLoginForm();
        }

        public void OpenLoginForm()
        {
            CloseAndDisposeForm(ref _studentDashboardForm);
            CloseAndDisposeForm(ref _adminForm);
            CloseAndDisposeForm(ref _adminFormSubjects);
            CloseAndDisposeForm(ref _superAdminForm);

            _loginForm = new LoginForm();
            _loginForm.SetAppContext(this);
            _loginForm.FormClosed += OnLoginFormClosed;
            _loginForm.Show();
        }

        private void OnLoginFormClosed(object sender, FormClosedEventArgs e)
        {
            var login = sender as LoginForm;
            if (login != null && !login.NavigatedAway)
            {
                ExitThread();
            }
            _loginForm = null;
        }

        // =============================================
        // STUDENT DASHBOARD (Subjects + Departments)
        // =============================================
        public void OpenStudentDashboardForm(string connectionString, string username)
        {
            CloseAndDisposeForm(ref _studentDashboardForm);

            if (_loginForm != null && !_loginForm.IsDisposed)
            {
                _loginForm.Hide();
            }

            _studentDashboardForm = new StudentDashboardForm(connectionString, username);
            _studentDashboardForm.SetAppContext(this);
            _studentDashboardForm.FormClosed += OnStudentDashboardFormClosed;
            _studentDashboardForm.Show();
        }

        private void OnStudentDashboardFormClosed(object sender, FormClosedEventArgs e)
        {
            var dashboard = sender as StudentDashboardForm;
            if (dashboard != null)
            {
                if (!dashboard.LoggedOut)
                {
                    OpenLoginForm();
                }
                else
                {
                    if (_loginForm != null && !_loginForm.IsDisposed && !_loginForm.Visible)
                    {
                        _loginForm.ClearFields();
                        _loginForm.Show();
                        _loginForm.BringToFront();
                    }
                    else if (_loginForm == null || _loginForm.IsDisposed)
                    {
                        OpenLoginForm();
                    }
                }
            }
            _studentDashboardForm = null;
        }

        // =============================================
        // ADMIN NAVIGATION (Department Clearance)
        // =============================================
        public void OpenAdminForm(string connectionString, string username)
        {
            CloseAndDisposeForm(ref _adminForm);

            if (_loginForm != null && !_loginForm.IsDisposed)
            {
                _loginForm.Hide();
            }

            _adminForm = new AdminForm(connectionString, username);
            _adminForm.SetAppContext(this);
            _adminForm.FormClosed += OnAdminFormClosed;
            _adminForm.Show();
        }

        private void OnAdminFormClosed(object sender, FormClosedEventArgs e)
        {
            if (_loginForm != null && !_loginForm.IsDisposed)
            {
                _loginForm.ClearFields();
                _loginForm.Show();
                _loginForm.BringToFront();
            }
            else
            {
                OpenLoginForm();
            }
            _adminForm = null;
        }

        // =============================================
        // INSTRUCTOR NAVIGATION (Subject Clearance)
        // =============================================
        public void OpenAdminFormSubjects(string connectionString, string username, string assignedSubject)
        {
            CloseAndDisposeForm(ref _adminFormSubjects);

            if (_loginForm != null && !_loginForm.IsDisposed)
            {
                _loginForm.Hide();
            }

            _adminFormSubjects = new AdminFormSubjects(connectionString, username, assignedSubject);
            _adminFormSubjects.SetAppContext(this);
            _adminFormSubjects.FormClosed += OnAdminFormSubjectsClosed;
            _adminFormSubjects.Show();
        }

        private void OnAdminFormSubjectsClosed(object sender, FormClosedEventArgs e)
        {
            if (_loginForm != null && !_loginForm.IsDisposed)
            {
                _loginForm.ClearFields();
                _loginForm.Show();
                _loginForm.BringToFront();
            }
            else
            {
                OpenLoginForm();
            }
            _adminFormSubjects = null;
        }

        // =============================================
        // SUPER ADMIN NAVIGATION (Operator Panel)
        // =============================================
        public void OpenSuperAdminForm(string connectionString, string username)
        {
            CloseAndDisposeForm(ref _superAdminForm);

            if (_loginForm != null && !_loginForm.IsDisposed)
                _loginForm.Hide();

            _superAdminForm = new SuperAdminForm(connectionString, username);
            _superAdminForm.SetAppContext(this);
            _superAdminForm.FormClosed += (s, e) =>
            {
                if (_loginForm != null && !_loginForm.IsDisposed)
                {
                    _loginForm.ClearFields();
                    _loginForm.Show();
                    _loginForm.BringToFront();
                }
                else OpenLoginForm();
                _superAdminForm = null;
            };
            _superAdminForm.Show();
        }

        // =============================================
        // HELPER METHODS
        // =============================================
        private void CloseAndDisposeForm<T>(ref T form) where T : Form
        {
            if (form != null && !form.IsDisposed)
            {
                try
                {
                    form.Close();
                    form.Dispose();
                }
                catch { }
                form = null;
            }
        }

        public void ReturnToLogin()
        {
            if (_loginForm != null && !_loginForm.IsDisposed)
            {
                _loginForm.Show();
                _loginForm.BringToFront();
            }
            else
            {
                OpenLoginForm();
            }
        }
    }
}