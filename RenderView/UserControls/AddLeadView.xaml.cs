using BahiKitaab.Model;
using BahiKitaab.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using BahiKitaab.ViewModel;

namespace BahiKitaab.RenderView.UserControls
{
    /// <summary>
    /// Interaction logic for AddLeadView.xaml
    /// </summary>
    public partial class AddLeadView : UserControl
    {
        // Using a DependencyProperty as the backing store for CustomerLeads.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CustomerLeadsProperty =
            DependencyProperty.Register("CustomerLeads", typeof(ObservableCollection<CustomerModel>), typeof(AddLeadView), new PropertyMetadata(new ObservableCollection<CustomerModel>()));

        public AddLeadView()
        {
            InitializeComponent();
            this.CustomerLeads = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
        }

        private void SaveLead(object sender, RoutedEventArgs e)
        {
            var lead = new CustomerModel
            {
                FirstName = this.tbFname.Text,
                LastName = this.tbLname.Text,
                EmailId = this.tbEmail.Text,
                PhoneNumber = this.tbContact.Text,
                Address = this.tbAddress.Text,
                City = this.tbCity.Text,
                District = this.tbDist.Text,                
                State = this.tbState.Text,
                Country = this.tbCountry.Text,                
                Notes = this.tbNotes.Text,
            };

            if (string.IsNullOrEmpty(this.tbPincode.Text))
            {
                lead.PinCode = int.MinValue;
            }
            else
            {
                lead.PinCode = Convert.ToInt32(this.tbPincode.Text);
            }

            if (!string.IsNullOrEmpty(this.tbDoa.Text))
            {
                lead.DateOfAnniversary = Convert.ToDateTime(this.tbDoa.Text);
            }

            if (!string.IsNullOrEmpty(this.tbDob.Text))
            {
                lead.DateOfBirth = Convert.ToDateTime(this.tbDob.Text);
            }

            HomepageViewModel.CommonViewModel.LeadsRepository.InsertLead(lead);
            this.CustomerLeads = HomepageViewModel.CommonViewModel.LeadsRepository.GetAllLeads();
        }

        public ObservableCollection<CustomerModel> CustomerLeads
        {
            get { return (ObservableCollection<CustomerModel>)GetValue(CustomerLeadsProperty); }
            set { SetValue(CustomerLeadsProperty, value); }
        }

        private void tb_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            e.Handled = Regex.IsMatch(e.Text, "[^0-9]+");
        }

        private string[]? SearchPincode(string pincode)
        {
            string[]? loc = null;

            if (!string.IsNullOrEmpty(pincode))
            {
                HttpClient client = new HttpClient();
                var address = @"https://api.postalpincode.in/pincode/" + pincode;
                client.BaseAddress = new Uri(address);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(address).Result;                
                if (response.IsSuccessStatusCode)
                {
                    loc = new string[4];
                    string responseBody = response.Content.ReadAsStringAsync().Result;
                    JArray data = (JArray)JsonConvert.DeserializeObject(responseBody);
                    var data1 = data.First;
                    var article = (JObject)data1["PostOffice"].Children().First();
                    loc[0] = article.SelectToken("District").Value<string>();
                    loc[1] = article.SelectToken("State").Value<string>();
                    loc[2] = article.SelectToken("Country").Value<string>();
                    loc[3] = article.SelectToken("Name").Value<string>();
                }
            }

            return loc;
        }

        private void tbPincode_LostFocus(object sender, RoutedEventArgs e)
        {
            var data = SearchPincode(this.tbPincode.Text);
            if (data != null)
            {
                this.tbDist.Text = data[0].ToString();
                this.tbState.Text = data[1].ToString();
                this.tbCountry.Text = data[2].ToString();
                this.tbCity.Text = data[3].ToString();
            }
        }
    }
}
