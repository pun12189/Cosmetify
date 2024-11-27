using Cosmetify.Model;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Cosmetify.Repository
{
    public class LeadsRepository : RepositoryBase
    {
        public CustomerModel GetCustomer(int id)
        {
            CustomerModel? category = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from customer_lead where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            category = new CustomerModel
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                LastName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                EmailId = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                PhoneNumber = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                Address = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                City = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                District = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                State = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                PinCode = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                                Country = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                DateOfAnniversary = reader.IsDBNull(11) ? DateTime.MinValue : reader.GetDateTime(11),
                                DateOfBirth = reader.IsDBNull(12) ? DateTime.MinValue : reader.GetDateTime(12),
                                Label = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                                Notes = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                                BrandName = reader.IsDBNull(15) ? null : JsonSerializer.Deserialize<ObservableCollection<string>>(reader.GetString(15)),
                            };
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            

            return category;
        }

        public ObservableCollection<CustomerModel> GetAllLeads()
        {
            ObservableCollection<CustomerModel> leads = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from customer_lead";
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        leads = new ObservableCollection<CustomerModel>();
                        while (reader.Read())
                        {
                            var lead = new CustomerModel
                            {
                                Id = reader.GetInt32(0),
                                FirstName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                LastName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                EmailId = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                PhoneNumber = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                                Address = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                                City = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                                District = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                State = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                PinCode = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                                Country = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                DateOfAnniversary = reader.IsDBNull(11) ? DateTime.MinValue : reader.GetDateTime(11),
                                DateOfBirth = reader.IsDBNull(12) ? DateTime.MinValue : reader.GetDateTime(12),
                                Label = reader.IsDBNull(13) ? string.Empty : reader.GetString(13),
                                Notes = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                                BrandName = reader.IsDBNull(15) ? null : JsonSerializer.Deserialize<ObservableCollection<string>>(reader.GetString(15))
                            };

                            leads.Add(lead);
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            

            return leads;
        }

        public void InsertLead(CustomerModel lead)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into customer_lead(cust_fname, cust_lname, cust_email, cust_phone, cust_address, cust_city, cust_district, cust_state, cust_pin, cust_country, cust_doa, cust_dob, cust_label, cust_notes, brand) values(@cust_fname, @cust_lname, @cust_email, @cust_phone, @cust_address, @cust_city, @cust_district, @cust_state, @cust_pin, @cust_country, @cust_doa, @cust_dob, @cust_label, @cust_notes, @brand)";
                    command.Parameters.Add("@cust_fname", MySqlDbType.VarChar).Value = lead.FirstName;
                    command.Parameters.Add("@cust_lname", MySqlDbType.VarChar).Value = lead.LastName;
                    command.Parameters.Add("@cust_email", MySqlDbType.VarChar).Value = lead.EmailId;
                    command.Parameters.Add("@cust_phone", MySqlDbType.VarChar).Value = lead.PhoneNumber;
                    command.Parameters.Add("@cust_address", MySqlDbType.VarChar).Value = lead.Address;
                    command.Parameters.Add("@cust_city", MySqlDbType.VarChar).Value = lead.City;
                    command.Parameters.Add("@cust_district", MySqlDbType.VarChar).Value = lead.District;
                    command.Parameters.Add("@cust_state", MySqlDbType.VarChar).Value = lead.State;
                    command.Parameters.Add("@cust_pin", MySqlDbType.Int32).Value = lead.PinCode;
                    command.Parameters.Add("@cust_country", MySqlDbType.VarChar).Value = lead.Country;
                    command.Parameters.Add("@cust_doa", MySqlDbType.DateTime).Value = lead.DateOfAnniversary;
                    command.Parameters.Add("@cust_dob", MySqlDbType.DateTime).Value = lead.DateOfBirth;
                    command.Parameters.Add("@cust_label", MySqlDbType.VarChar).Value = lead.Label;
                    command.Parameters.Add("@cust_notes", MySqlDbType.VarChar).Value = lead.Notes;
                    command.Parameters.Add("@brand", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.BrandName);
                    command.ExecuteScalar();
                    MessageBox.Show("Lead Added");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public void UpdateLead(CustomerModel lead)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "update customer_lead set cust_fname=@cust_fname, cust_lname=@cust_lname, cust_email=@cust_email, cust_phone=@cust_phone, cust_address=@cust_address, cust_city=@cust_city, cust_district=@cust_district, cust_state=@cust_state, cust_pin=@cust_pin, cust_country=@cust_country, cust_doa=@cust_doa, cust_dob=@cust_dob, cust_label=@cust_label, cust_notes=@cust_notes, brand=@brand where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@cust_fname", MySqlDbType.VarChar).Value = lead.FirstName;
                    command.Parameters.Add("@cust_lname", MySqlDbType.VarChar).Value = lead.LastName;
                    command.Parameters.Add("@cust_email", MySqlDbType.VarChar).Value = lead.EmailId;
                    command.Parameters.Add("@cust_phone", MySqlDbType.VarChar).Value = lead.PhoneNumber;
                    command.Parameters.Add("@cust_address", MySqlDbType.VarChar).Value = lead.Address;
                    command.Parameters.Add("@cust_city", MySqlDbType.VarChar).Value = lead.City;
                    command.Parameters.Add("@cust_district", MySqlDbType.VarChar).Value = lead.District;
                    command.Parameters.Add("@cust_state", MySqlDbType.VarChar).Value = lead.State;
                    command.Parameters.Add("@cust_pin", MySqlDbType.Int32).Value = lead.PinCode;
                    command.Parameters.Add("@cust_country", MySqlDbType.VarChar).Value = lead.Country;
                    command.Parameters.Add("@cust_doa", MySqlDbType.DateTime).Value = lead.DateOfAnniversary;
                    command.Parameters.Add("@cust_dob", MySqlDbType.DateTime).Value = lead.DateOfBirth;
                    command.Parameters.Add("@cust_label", MySqlDbType.VarChar).Value = lead.Label;
                    command.Parameters.Add("@cust_notes", MySqlDbType.VarChar).Value = lead.Notes;
                    command.Parameters.Add("@brand", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.BrandName);
                    command.ExecuteScalar();
                    MessageBox.Show("Lead Updated");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public void DeleteLead(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "delete from customer_lead where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.ExecuteScalar();
                    MessageBox.Show("Lead Deleted");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public ObservableCollection<string> GetBrandsOfCustomer(int id)
        {
            ObservableCollection<string>? brands = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from customer_lead where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        brands = new ObservableCollection<string>();
                        while (reader.Read())
                        {
                            brands = reader.IsDBNull(15) ? null : JsonSerializer.Deserialize<ObservableCollection<string>>(reader.GetString(15));                            
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }

            return brands;
        }

        /*public ReportDocument GenerateReport()
        {
            ReportDocument myReport = new ReportDocument();
            DataSet myData = new DataSet();

            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from customer_lead";
                var myAdapter = new MySql.Data.MySqlClient.MySqlDataAdapter();
                myAdapter.SelectCommand = command;
                myAdapter.Fill(myData);

                myReport.Load(@".\world_report.rpt");
                myReport.SetDataSource(myData);
            }

            return myReport;
        }*/
    }
}
