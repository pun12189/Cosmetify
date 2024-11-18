using BahiKitaab.Model;
using BahiKitaab.ViewModel;
using MahApps.Metro.Controls;
using System.Windows;

namespace BahiKitaab.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : MetroWindow
    {
        public LoginView()
        {
            InitializeComponent();
            DataContext = new LoginViewModel(this);
        }
    }
}
