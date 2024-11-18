using System.Windows.Controls;

namespace Cosmetify.View
{
    /// <summary>
    /// Interaction logic for ConfirmationCodeControl.xaml
    /// </summary>
    public partial class ConfirmationCodeControl : UserControl
    {
        public ConfirmationCodeControl()
        {
            InitializeComponent();
            ConfirmationCode.Focus();
        }
    }
}
