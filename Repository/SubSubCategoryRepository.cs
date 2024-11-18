using Cosmetify.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Cosmetify.Repository
{
    public class SubSubCategoryRepository : RepositoryBase
    {
        public SubSubCategoryRepository(SubCategoryRepository subCategoryRepository)
        {
            this.SubCategoryRepository = subCategoryRepository;
        }

        public SubCategoryRepository SubCategoryRepository { get; set; }

        public SubSubCategoryModel GetSubSubCategory(int id)
        {
            SubSubCategoryModel? category = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from sub_sub_category where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            category = new SubSubCategoryModel();
                            category.Id = reader.GetInt32(0);
                            category.CategoryName = reader.GetString(1);
                            if (this.SubCategoryRepository != null)
                            {
                                category.ParentCategoryId = reader.GetInt32(2);
                            }
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

        public ObservableCollection<SubSubCategoryModel> GetSubSubCategories()
        {
            ObservableCollection<SubSubCategoryModel> categories = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from sub_sub_category";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        categories = new ObservableCollection<SubSubCategoryModel>();
                        while (reader.Read())
                        {
                            var category = new SubSubCategoryModel();
                            category.Id = reader.GetInt32(0);
                            category.CategoryName = reader.GetString(1);
                            if (this.SubCategoryRepository != null)
                            {
                                category.ParentCategoryId = reader.GetInt32(2);
                            }

                            categories.Add(category);
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            

            return categories;
        }

        public ObservableCollection<SubSubCategoryModel> GetSubSubCategoriesByParentId(int parent_id)
        {
            ObservableCollection<SubSubCategoryModel> categories = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from sub_sub_category where parent_categ_id=@parent_id";
                    command.Parameters.Add("@parent_id", MySqlDbType.Int32).Value = parent_id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        categories = new ObservableCollection<SubSubCategoryModel>();
                        while (reader.Read())
                        {
                            var category = new SubSubCategoryModel();
                            category.Id = reader.GetInt32(0);
                            category.CategoryName = reader.GetString(1);
                            if (this.SubCategoryRepository != null)
                            {
                                category.ParentCategoryId = reader.GetInt32(2);
                            }

                            categories.Add(category);
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            

            return categories;
        }

        public void InsertSubSubCategory(string categ_name, int parent_id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into sub_sub_category(categ_name, parent_categ_id) values(@categ_name, @parent_id)";
                    command.Parameters.Add("@categ_name", MySqlDbType.VarChar).Value = categ_name;
                    command.Parameters.Add("@parent_id", MySqlDbType.Int32).Value = parent_id;
                    command.ExecuteScalar();
                    MessageBox.Show("SubSub-Category Added");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public void UpdateSubSubCategory(int id, string categ_name, int parent_id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "update sub_sub_category set categ_name=@categ_name, parent_categ_id=@parent_id, updated_at=@updated where id=@id";
                    command.Parameters.Add("@categ_name", MySqlDbType.VarChar).Value = categ_name;
                    command.Parameters.Add("@parent_id", MySqlDbType.Double).Value = parent_id;
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.Parameters.Add("@updated", MySqlDbType.DateTime).Value = DateTime.Now;
                    command.ExecuteScalar();
                    MessageBox.Show("SubSub-Category Updated");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public void DeleteSubSubCategory(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "delete from sub_sub_category where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.ExecuteScalar();
                    MessageBox.Show("SubSub-Category Deleted");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }
    }
}
