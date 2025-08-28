using Cosmetify.Command;
using Cosmetify.Helper;
using Cosmetify.Model;
using MySqlConnector;
using Newtonsoft.Json;
using Sentry.Reflection;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Net.Http;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Cosmetify.ViewModel
{
    class LoginViewModel : INotifyPropertyChanged
    {
        #region Properties

        private ForgotPasswordViewModel forgotPasswordViewModel;

        private static readonly HttpClient _httpClient = new HttpClient();

        private static readonly string SystemId = Environment.UserName;

        private static readonly string SoftwareId = Assembly.GetExecutingAssembly().GetHashCode().ToString();

        private Window window;

        public ICommand LoginCommand { get; set; }

        public ICommand RegisterCommand { get; set; }

        public ICommand ForgotPasswordCommand { get; set; }

        public Action CloseAction { get; set; }

        private string _email;

        public string Email
        {
            get { return _email; }
            set
            {
                if (_email == value) return;
                _email = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Email"));
            }
        }

        #endregion

        #region Constructor
        public LoginViewModel(Window window)
        {
            this.window = window;
            UserModel.Instance.Email = Email;
            LoginCommand = new RelayCommand(LoginCommandExecute);
            RegisterCommand = new RelayCommand(RegisterCommandExecute);
            ForgotPasswordCommand = new RelayCommand(ForgotPasswordCommandExecute);
            var x = SystemId + SoftwareId;
        }
        #endregion

        #region Private Methods
        private async void LoginCommandExecute()
        {
            if (checkDB_Conn() == false) 
            {
                return;
            }

            UserModel.Instance.Email = Email;
            if (UserModel.Instance.Email == null || UserModel.Instance.Password == null)
            {
                MessageBox.Show("Both email and password should be filled in.");
                return;
            }

            var apimodel = await GetApiDataAsync("https://bahikitab.edvertisements.com/getstatus-api.php?systemid=" + SystemId);
            if (apimodel != null && apimodel[0].Status.Contains("inactive"))
            {
                if (apimodel[0].Notice != string.Empty)
                {
                    MessageBox.Show(apimodel[0].Notice, "Notice", MessageBoxButton.OK, MessageBoxImage.Warning);
                }                
            }
            else
            {
                if (AccountManager.AccountExists(UserModel.Instance.Email, UserModel.Instance.Password))
                {
                    var homepageViewModel = new HomepageViewModel(window);
                    WindowManager.ChangeWindowContent(window, homepageViewModel, Resources.HomepageWindowTitle, Resources.HomepageControlPath);

                    if (homepageViewModel.CloseAction == null)
                    {
                        homepageViewModel.CloseAction = () => window.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Invalid credentials.");
                }
            }
        }

        private void RegisterCommandExecute()
        {
            Email = null;
            var registerViewModel = new RegisterViewModel(window);
            WindowManager.ChangeWindowContent(window, registerViewModel, Resources.RegisterAccountWindowTitle, Resources.RegisterAccountControlPath);
            if (registerViewModel.CloseAction == null)
            {
                registerViewModel.CloseAction = () => window.Close();
            }
        }
        public void ForgotPasswordCommandExecute()
        {
            forgotPasswordViewModel = new ForgotPasswordViewModel(window);
            WindowManager.ChangeWindowContent(window, forgotPasswordViewModel, Resources.ForgotPasswordWindowTitle, Resources.ForgotPasswordControlPath);
            if (forgotPasswordViewModel.CloseAction == null)
            {
                forgotPasswordViewModel.CloseAction = () => window.Close();
            }
            window.Show();
        }

        public static bool checkDB_Conn()
        {
            var conn_info = string.Empty;
#if DEBUG

            //_connectionString = "DataSource=bahikitab-aws.c3s6wewcwox1.us-east-1.rds.amazonaws.com;Port=3306;Uid=admin;Pwd=Il6oOvguA2SB5IEQxWCJ;database=bahikitab";
            conn_info = "Server=localhost;Uid=root;Pwd='';database=cosmetify";
#endif
#if RELEASE

            conn_info = "Server=192.168.1.90;Uid=cosdb;Pwd=Cosmetify@123;database=cosmetify";
#endif
#if TESTING

            //_connectionString = "DataSource=bahikitab-aws.c3s6wewcwox1.us-east-1.rds.amazonaws.com;Port=3306;Uid=admin;Pwd=Il6oOvguA2SB5IEQxWCJ;database=bahikitab";
            conn_info = "Server=localhost;Uid=root;Pwd='';database=cosmetify";
#endif
            bool isConn = false;
            MySqlConnection conn = null;
            try
            {
                conn = new MySqlConnection(conn_info);
                conn.Open();
                isConn = true;
            }
            catch (ArgumentException a_ex)
            {
                MessageBox.Show(a_ex.Message);
                isConn = false;
            }
            catch (MySqlException ex)
            {
                string sqlErrorMessage = "Message: " + ex.Message + "\n" +
                "Source: " + ex.Source + "\n" +
                "Number: " + ex.Number;
                
                isConn = false;
                switch (ex.Number)
                {
                    //http://dev.mysql.com/doc/refman/5.0/en/error-messages-server.html
                    case 1042: MessageBox.Show("Unable to connect to any of the specified MySQL hosts (Check Server,Port)");
                        break;
                    case 0: 
                        MessageBox.Show("Check DB name,username,password");
                        break;
                    default:
                        MessageBox.Show(sqlErrorMessage);
                        break;
                }
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
            return isConn;
        }

        public async Task<List<ApiClassModel>> GetApiDataAsync(string apiUrl)
        {
            try
            {
                List<ApiClassModel>? data = null;
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    if (jsonString.Contains("No data found"))
                    {
                        await _httpClient.GetAsync("https://bahikitab.edvertisements.com/insertdata.php?systemid="+ SystemId +"&softwareid="+SoftwareId);
                    }
                    else
                    {
                        data = JsonConvert.DeserializeObject<List<ApiClassModel>>(jsonString);
                    }
                    
                    return data;
                }                
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }

            return null;
        }

        /*public async Task PostApiDataAsync(string systemid, string softwareid)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonString = await response.Content.ReadAsStringAsync();
                    ObservableCollection<ApiClassModel> data = JsonConvert.DeserializeObject<ObservableCollection<ApiClassModel>>(jsonString);
                    return data;
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }

            return null;
        }*/

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
