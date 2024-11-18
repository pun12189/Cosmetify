using System.Windows.Controls;

namespace Cosmetify.View
{
    /// <summary>
    /// Interaction logic for ForgotPasswordControl.xaml
    /// </summary>
    public partial class ForgotPasswordControl : UserControl
    {
        public ForgotPasswordControl()
        {
            InitializeComponent();
            Email.Focus();
        }
    }
}
