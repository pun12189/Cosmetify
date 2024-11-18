using System.Windows;
using System.Windows.Controls;
using BahiKitaab.Model;

namespace BahiKitaab.View
{
    /// <summary>
    /// Interaction logic for ResetPasswordControl.xaml
    /// </summary>
    public partial class ResetPasswordControl : UserControl
    {
        public ResetPasswordControl()
        {
            InitializeComponent();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            UserModel.Instance.Password = NewPassword.Password;
            if (NewPassword.Password != ConfirmPassword.Password)
            {
                UserModel.Instance.Password = null;
                MessageBox.Show("Password does not match.");
            }
        }
    }
}
