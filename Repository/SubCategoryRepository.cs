using BahiKitaab.Model;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Windows;

namespace BahiKitaab.Repository
{
    public class SubCategoryRepository : RepositoryBase
    {
        public CategoryRepository CategoryRepository { get; set; }

        public SubSubCategoryRepository SubSubCategoryRepository { get; set; }

        public SubCategoryModel GetSubCategory(int id)
        {
            SubCategoryModel? category = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from sub_category where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            category = new SubCategoryModel();
                            category.Id = reader.GetInt32(0);
                            category.CategoryName = reader.GetString(1);
                            if (this.CategoryRepository != null)
                            {
                                category.ParentCategoryId = reader.GetInt32(2);
                            }

                            if (this.SubSubCategoryRepository != null)
                            {
                                category.SubSubCategories = this.SubSubCategoryRepository.GetSubSubCategoriesByParentId(reader.GetInt32(0));
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

        public ObservableCollection<SubCategoryModel> GetSubCategories()
        {
            ObservableCollection<SubCategoryModel> categories = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from sub_category";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        categories = new ObservableCollection<SubCategoryModel>();
                        while (reader.Read())
                        {
                            var category = new SubCategoryModel();
                            category.Id = reader.GetInt32(0);
                            category.CategoryName = reader.GetString(1);
                            if (this.CategoryRepository != null)
                            {
                                category.ParentCategoryId = reader.GetInt32(2);
                            }

                            if (this.SubSubCategoryRepository != null)
                            {
                                category.SubSubCategories = this.SubSubCategoryRepository.GetSubSubCategoriesByParentId(reader.GetInt32(0));
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

        public ObservableCollection<SubCategoryModel> GetSubCategoriesByParentId(int parent_id)
        {
            ObservableCollection<SubCategoryModel> categories = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from sub_category where parent_categ_id=@parent_id";
                    command.Parameters.Add("@parent_id", MySqlDbType.Int32).Value = parent_id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        categories = new ObservableCollection<SubCategoryModel>();
                        while (reader.Read())
                        {
                            var category = new SubCategoryModel();
                            category.Id = reader.GetInt32(0);
                            category.CategoryName = reader.GetString(1);
                            if (this.CategoryRepository != null)
                            {
                                category.ParentCategoryId = reader.GetInt32(2);
                            }

                            if (this.SubSubCategoryRepository != null)
                            {
                                category.SubSubCategories = this.SubSubCategoryRepository.GetSubSubCategoriesByParentId(reader.GetInt32(0));
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

        public void InsertSubCategory(string categ_name, int parent_id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into sub_category(categ_name, parent_categ_id) values(@categ_name, @parent_id)";
                    command.Parameters.Add("@categ_name", MySqlDbType.VarChar).Value = categ_name;
                    command.Parameters.Add("@parent_id", MySqlDbType.Int32).Value = parent_id;
                    command.ExecuteScalar();
                    MessageBox.Show("Sub-Category Added");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public void UpdateSubCategory(int id, string categ_name, int parent_id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "update sub_category set categ_name=@categ_name, parent_categ_id=@parent_id, updated_at=@updated where id=@id";
                    command.Parameters.Add("@categ_name", MySqlDbType.VarChar).Value = categ_name;
                    command.Parameters.Add("@parent_id", MySqlDbType.Double).Value = parent_id;
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.Parameters.Add("@updated", MySqlDbType.DateTime).Value = DateTime.Now;
                    command.ExecuteScalar();
                    MessageBox.Show("Sub-Category Updated");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public void DeleteSubCategory(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "delete from sub_category where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.ExecuteScalar();
                    MessageBox.Show("Sub-Category Deleted");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }
    }
}
