using Cosmetify.Model.Enums;
using Cosmetify.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using Cosmetify.ViewModel;

namespace Cosmetify.Repository
{
    public class ActivesRepository : RepositoryBase
    {
        public ActivesModel GetProduct(int id)
        {
            ActivesModel? product = null;
            try
            {                
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from actives where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            product = new ActivesModel
                            {
                                Id = reader.GetInt32(0),
                                ActivesName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                ShortCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Stocks = reader.IsDBNull(3) ? double.MinValue : reader.GetDouble(3),
                                Units = reader.IsDBNull(4) ? ProductUnits.Kilograms : (ProductUnits)Enum.Parse(typeof(ProductUnits), reader.GetString(4)),
                                SKU = reader.IsDBNull(5) ? double.MinValue : reader.GetDouble(5),
                                Category = reader.IsDBNull(6) ? null : HomepageViewModel.CommonViewModel.CategoryRepository.GetCategory(reader.GetInt32(6)),
                                SubCategory = reader.IsDBNull(7) ? null : HomepageViewModel.CommonViewModel.SubCategoryRepository.GetSubCategory(reader.GetInt32(7)),
                                SubSubCategory = reader.IsDBNull(8) ? null : HomepageViewModel.CommonViewModel.SubSubCategoryRepository.GetSubSubCategory(reader.GetInt32(8)),
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

        public ObservableCollection<ActivesModel> SearchActives(string searchData)
        {
            ObservableCollection<ActivesModel> leads = new ObservableCollection<ActivesModel>();
            try
            {                
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from actives where name LIKE @data OR code LIKE @data";
                    command.Parameters.Add("@data", MySqlDbType.VarChar).Value = "%" + searchData + "%";
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var lead = new ActivesModel
                            {
                                Id = reader.GetInt32(0),
                                ActivesName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                ShortCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Stocks = reader.IsDBNull(3) ? double.MinValue : reader.GetDouble(3),
                                Units = reader.IsDBNull(4) ? ProductUnits.Kilograms : (ProductUnits)Enum.Parse(typeof(ProductUnits), reader.GetString(4)),
                                SKU = reader.IsDBNull(5) ? double.MinValue : reader.GetDouble(5),
                                Category = reader.IsDBNull(6) ? null : HomepageViewModel.CommonViewModel.CategoryRepository.GetCategory(reader.GetInt32(6)),
                                SubCategory = reader.IsDBNull(7) ? null : HomepageViewModel.CommonViewModel.SubCategoryRepository.GetSubCategory(reader.GetInt32(7)),
                                SubSubCategory = reader.IsDBNull(8) ? null : HomepageViewModel.CommonViewModel.SubSubCategoryRepository.GetSubSubCategory(reader.GetInt32(8)),
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

        public ObservableCollection<ActivesModel> GetAllProducts()
        {
            ObservableCollection<ActivesModel> leads = new ObservableCollection<ActivesModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from actives";
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var lead = new ActivesModel
                            {
                                Id = reader.GetInt32(0),
                                ActivesName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                ShortCode = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                                Stocks = reader.IsDBNull(3) ? double.MinValue : reader.GetDouble(3),
                                Units = reader.IsDBNull(4) ? ProductUnits.Kilograms : (ProductUnits)Enum.Parse(typeof(ProductUnits), reader.GetString(4)),
                                SKU = reader.IsDBNull(5) ? double.MinValue : reader.GetDouble(5),
                                Category = reader.IsDBNull(6) ? null : HomepageViewModel.CommonViewModel.CategoryRepository.GetCategory(reader.GetInt32(6)),
                                SubCategory = reader.IsDBNull(7) ? null : HomepageViewModel.CommonViewModel.SubCategoryRepository.GetSubCategory(reader.GetInt32(7)),
                                SubSubCategory = reader.IsDBNull(8) ? null : HomepageViewModel.CommonViewModel.SubSubCategoryRepository.GetSubSubCategory(reader.GetInt32(8)),
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

        public void InsertProduct(ActivesModel lead)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into actives(name, code, stocks, units, sku, category, subcategory, subsubcategory) values(@name, @code, @stocks, @units, @sku, @category, @subcategory, @subsubcategory)";
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.ActivesName;
                    command.Parameters.Add("@code", MySqlDbType.VarChar).Value = lead.ShortCode;
                    command.Parameters.Add("@stocks", MySqlDbType.Double).Value = lead.Stocks;
                    command.Parameters.Add("@units", MySqlDbType.VarChar).Value = lead.Units;
                    command.Parameters.Add("@sku", MySqlDbType.Double).Value = lead.SKU;
                    if (lead.Category != null)
                    {
                        command.Parameters.Add("@category", MySqlDbType.Int32).Value = lead.Category.Id;
                    }
                    else
                    {
                        command.Parameters.Add("@category", MySqlDbType.Int32).Value = null;
                    }

                    if (lead.SubCategory != null)
                    {
                        command.Parameters.Add("@subcategory", MySqlDbType.Int32).Value = lead.SubCategory.Id;
                    }
                    else
                    {
                        command.Parameters.Add("@subcategory", MySqlDbType.Int32).Value = null;
                    }

                    if (lead.SubSubCategory != null)
                    {
                        command.Parameters.Add("@subsubcategory", MySqlDbType.Int32).Value = lead.SubSubCategory.Id;
                    }
                    else
                    {
                        command.Parameters.Add("@subsubcategory", MySqlDbType.Int32).Value = null;
                    }

                    command.ExecuteScalar();
                    MessageBox.Show("Actives Added");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public void UpdateProduct(ActivesModel lead)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "update actives set name=@name, code=@code, stocks=@stocks, units=@units, sku=@sku, category=@category, subcategory=@subcategory, subsubcategory=@subsubcategory where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@name", MySqlDbType.VarChar).Value = lead.ActivesName;
                    command.Parameters.Add("@code", MySqlDbType.VarChar).Value = lead.ShortCode;
                    command.Parameters.Add("@stocks", MySqlDbType.Double).Value = lead.Stocks;
                    command.Parameters.Add("@units", MySqlDbType.VarChar).Value = lead.Units;
                    command.Parameters.Add("@sku", MySqlDbType.Double).Value = lead.SKU;
                    if (lead.Category != null)
                    {
                        command.Parameters.Add("@category", MySqlDbType.Int32).Value = lead.Category.Id;
                    }
                    else
                    {
                        command.Parameters.Add("@category", MySqlDbType.Int32).Value = null;
                    }

                    if (lead.SubCategory != null)
                    {
                        command.Parameters.Add("@subcategory", MySqlDbType.Int32).Value = lead.SubCategory.Id;
                    }
                    else
                    {
                        command.Parameters.Add("@subcategory", MySqlDbType.Int32).Value = null;
                    }

                    if (lead.SubSubCategory != null)
                    {
                        command.Parameters.Add("@subsubcategory", MySqlDbType.Int32).Value = lead.SubSubCategory.Id;
                    }
                    else
                    {
                        command.Parameters.Add("@subsubcategory", MySqlDbType.Int32).Value = null;
                    }

                    command.ExecuteScalar();
                    //MessageBox.Show("Actives Updated");
                }
            }
            catch (Exception e)
            { 
                Helper.Helper.BugReport(e);
            }            
        }

        public void DeleteProduct(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "delete from actives where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.ExecuteScalar();
                    //MessageBox.Show("Actives Deleted");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public void BulkInsertMySQL(DataTable table, string tableName)
        {
            try
            {
                using (MySqlConnection connection = GetConnection())
                {
                    connection.Open();

                    using (MySqlTransaction tran = connection.BeginTransaction(IsolationLevel.Serializable))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            cmd.Connection = connection;
                            cmd.Transaction = tran;
                            cmd.CommandText = $"SELECT * FROM " + tableName + " limit 0";

                            try
                            {
                                using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                                {
                                    adapter.UpdateBatchSize = 10000;
                                    using (MySqlCommandBuilder cb = new MySqlCommandBuilder(adapter))
                                    {
                                        cb.SetAllValues = true;
                                        adapter.Update(table);
                                        tran.Commit();
                                    }
                                };
                            }
                            catch (MySqlException e)
                            {
                                MessageBox.Show("CSV File must have unique data", "Info");
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }
        }
    }
}
