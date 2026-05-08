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
        private HomeForm _homeForm;
        private AdminForm _adminForm;

        public AppContext()
        {
            OpenLoginForm();
        }

        public void OpenLoginForm()
        {
            // Clean up existing forms
            CloseAndDisposeForm(ref _homeForm);
            CloseAndDisposeForm(ref _adminForm);

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

        public void OpenHomeForm(string connectionString, string username)
        {
            CloseAndDisposeForm(ref _homeForm);

            if (_loginForm != null && !_loginForm.IsDisposed)
            {
                _loginForm.Hide();
            }

            _homeForm = new HomeForm(connectionString, username);
            _homeForm.SetAppContext(this);
            _homeForm.FormClosed += OnHomeFormClosed;
            _homeForm.Show();
        }

        private void OnHomeFormClosed(object sender, FormClosedEventArgs e)
        {
            var home = sender as HomeForm;
            if (home != null)
            {
                if (!home.LoggedOut)
                {
                    OpenLoginForm();
                }
                else
                {
                    if (_loginForm != null && !_loginForm.IsDisposed && !_loginForm.Visible)
                    {
                        _loginForm.ClearFields(); // Add this line
                        _loginForm.Show();
                        _loginForm.BringToFront();
                    }
                    else if (_loginForm == null || _loginForm.IsDisposed)
                    {
                        OpenLoginForm();
                    }
                }
            }
            _homeForm = null;
        }

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
                _loginForm.ClearFields(); // Add this line to clear fields
                _loginForm.Show();
                _loginForm.BringToFront();
            }
            else
            {
                OpenLoginForm();
            }
            _adminForm = null;
        }

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