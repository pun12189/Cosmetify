using Cosmetify.Model;
using Cosmetify.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Cosmetify.View
{
    /// <summary>
    /// Interaction logic for RegisterControl.xaml
    /// </summary>
    public partial class RegisterControl : UserControl
    {

        public RegisterControl()
        {
            InitializeComponent();
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            UserModel.Instance.Password = Password.Password;
        }
    }
}
