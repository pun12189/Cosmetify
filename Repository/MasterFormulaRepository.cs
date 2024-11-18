using BahiKitaab.Model.Enums;
using BahiKitaab.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BahiKitaab.Repository
{
    public class MasterFormulaRepository : RepositoryBase
    {
        public MasterFormulaModel GetFormula(int id)
        {
            MasterFormulaModel? product = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from masterformula where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            product = new MasterFormulaModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Code = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Requirements = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<ObservableCollection<MasterProductModel>>(reader.GetString(3)),
                                RemainingWater = reader.IsDBNull(4) ? double.MinValue : reader.GetDouble(4),
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

            return product;
        }

        public ObservableCollection<MasterFormulaModel> GetAllFormulas()
        {
            ObservableCollection<MasterFormulaModel> leads = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from masterformula";
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        leads = new ObservableCollection<MasterFormulaModel>();
                        while (reader.Read())
                        {
                            var lead = new MasterFormulaModel
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Code = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Requirements = reader.IsDBNull(3) ? null : JsonSerializer.Deserialize<ObservableCollection<MasterProductModel>>(reader.GetString(3)),
                                RemainingWater = reader.IsDBNull(4) ? double.MinValue : reader.GetDouble(4),
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

        public void InsertFormula(MasterFormulaModel lead)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into masterformula(name, code, requirements, remaining) values(@name, @code, @requirements, @remaining)";
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.Name;
                    command.Parameters.Add("@code", MySqlDbType.VarChar).Value = lead.Code;
                    command.Parameters.Add("@requirements", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Requirements);
                    command.Parameters.Add("@remaining", MySqlDbType.Double).Value = lead.RemainingWater;
                    command.ExecuteScalar();
                    MessageBox.Show("Master Formula Added");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public void UpdateFormula(MasterFormulaModel lead)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "update masterformula set name=@name, code=@code, requirements=@requirements, remaining=@remaining where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.Name;
                    command.Parameters.Add("@code", MySqlDbType.VarChar).Value = lead.Code;
                    command.Parameters.Add("@requirements", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Requirements);
                    command.Parameters.Add("@remaining", MySqlDbType.VarChar).Value = lead.RemainingWater;
                    command.ExecuteScalar();
                    MessageBox.Show("Master Formula Updated");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public void DeleteFormula(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "delete from masterformula where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.ExecuteScalar();
                    MessageBox.Show("Master Formula Deleted");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }
    }
}
