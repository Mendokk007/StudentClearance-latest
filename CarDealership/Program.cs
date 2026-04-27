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
        public AppContext()
        {
            OpenLoginForm();
        }

        public void OpenLoginForm()
        {
            var login = new LoginForm();
            login.SetAppContext(this);
            login.FormClosed += (s, e) =>
            {
                if (!login.NavigatedAway)
                    ExitThread();
            };
            login.Show();
        }

        public void OpenHomeForm(string connectionString, string username)
        {
            var home = new HomeForm(connectionString, username);
            home.SetAppContext(this);
            home.FormClosed += (s, e) =>
            {
                if (!home.LoggedOut)
                    ExitThread();
            };
            home.Show();
        }
    }
}