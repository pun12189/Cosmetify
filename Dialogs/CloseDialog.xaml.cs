using MySqlConnector;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Cosmetify.Dialogs
{
    /// <summary>
    /// Interaction logic for CloseDialog.xaml
    /// </summary>
    public partial class CloseDialog : Window
    {
        private string _connectionString;

        public CloseDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
#if DEBUG

            //_connectionString = "DataSource=bahikitab-aws.c3s6wewcwox1.us-east-1.rds.amazonaws.com;Port=3306;Uid=admin;Pwd=Il6oOvguA2SB5IEQxWCJ;database=bahikitab";
            _connectionString = "Server=localhost;Uid=root;Pwd='';database=cosmetify";
#endif
#if RELEASE

            _connectionString = "Server=192.168.1.90;Uid=cosdb;Pwd=Cosmetify@123;database=cosmetify";
#endif
            string file = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "//backup_cosmetify.sql";
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(command))
                    {
                        command.Connection = connection;
                        connection.Open();
                        mb.ExportToFile(file);
                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.Helper.BugReport(ex);
            }
            finally 
            {
                this.Close();
            }            
        }

        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
