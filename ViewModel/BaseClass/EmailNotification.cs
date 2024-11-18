using BahiKitaab.Helper;
using BahiKitaab.Model;
using BahiKitaab.Validator;
using System.Windows;

namespace BahiKitaab.ViewModel.BaseClass
{
    public class EmailNotification
    {
        public bool SendEmailCode(Window window, string emailSubject, string emailContent)
        {
            var confirmationCode = new Random().Next(100000, 999999).ToString();
            emailContent += confirmationCode;
            var Validator = new PersonalAccountValidator().ValidateEmail(UserModel.Instance.Email, emailSubject, emailContent);
            if (UserModel.Instance.Email == null || !Validator.IsValid)
            {
                MessageBox.Show(Validator.ValidationMessage);
                return false;
            }
            var confirmationCodeViewModel = new ConfirmationCodeViewModel(window, confirmationCode);
            WindowManager.ChangeWindowContent(window, confirmationCodeViewModel, Resources.ConfirmationCodeWindowTitle, Resources.ConfirmationCodeControlPath);

            if (confirmationCodeViewModel.CloseAction == null)
            {
                confirmationCodeViewModel.CloseAction = () => window.Close();
            }
            return true;
        }
    }
}
