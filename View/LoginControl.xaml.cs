using BahiKitaab.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BahiKitaab.View
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : UserControl
    {
        public LoginControl()
        {
            InitializeComponent();
            if (!File.Exists(System.IO.Path.GetTempPath() + "\\BK_Error_Log.txt"))
            {
                File.Create(System.IO.Path.GetTempPath() + "\\BK_Error_Log.txt");
            }
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UserModel.Instance.Password = Password.Password;
        }

        private void Click(object sender, RoutedEventArgs e)
        {
            Email.Text = null;
            Password.Password = null;
        }
    }
}
