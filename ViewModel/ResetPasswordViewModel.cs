using Cosmetify.Command;
using Cosmetify.Helper;
using Cosmetify.Model;
using Cosmetify.Validator;
using System;
using System.Windows;
using System.Windows.Input;

namespace Cosmetify.ViewModel
{
    class ResetPasswordViewModel
    {
        #region Properties
        public Action CloseAction { get; set; }

        private Window window;

        public ICommand ResetCommand { get; set; }
    #endregion

        #region Constructor
        public ResetPasswordViewModel(Window window)
        {
            this.window = window;
            ResetCommand = new RelayCommand(ResetCommandExecute);
        }
    #endregion

        #region Private Methods
        private void ResetCommandExecute()
        {
            if(UserModel.Instance.Password == null)
            {
                return;
            }
            var Validator = new PersonalAccountValidator().ValidatePassword(UserModel.Instance.Password);

            if (!Validator.IsValid)
            {
                MessageBox.Show(Validator.ValidationMessage);
            }
            else
            {
                AccountManager.ChangePassword(window, UserModel.Instance.Email, UserModel.Instance.Password);
                //CloseAction?.Invoke();
            }
        }
        #endregion
    }
}
