using BahiKitaab.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BahiKitaab.Repository
{
    public class CategoryRepository : RepositoryBase
    {
        public CategoryRepository(SubCategoryRepository instance) {
            this.SubCategoryRepository = instance;
        }

        public SubCategoryRepository SubCategoryRepository { get; set; }

        public CategoryModel GetCategory(int id)
        {
            CategoryModel? category = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from category where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            category = new CategoryModel();
                            category.Id = reader.GetInt32(0);
                            category.CategoryName = reader.GetString(1);
                            category.GstRate = reader.GetDouble(2);
                            if (this.SubCategoryRepository != null)
                            {
                                category.SubCategories = this.SubCategoryRepository.GetSubCategoriesByParentId(reader.GetInt32(0));
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

        public ObservableCollection<CategoryModel> GetCategories()
        {
            ObservableCollection<CategoryModel> categories = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from category";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        categories = new ObservableCollection<CategoryModel>();
                        while (reader.Read())
                        {
                            var category = new CategoryModel();
                            category.Id = reader.GetInt32(0);
                            category.CategoryName = reader.GetString(1);
                            category.GstRate = reader.GetDouble(2);
                            if (this.SubCategoryRepository != null)
                            {
                                category.SubCategories = this.SubCategoryRepository.GetSubCategoriesByParentId(reader.GetInt32(0));
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

        public void InsertCategory(string categ_name, double gst)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into category(categ_name, gst) values(@categ_name, @gst)";
                    command.Parameters.Add("@categ_name", MySqlDbType.VarChar).Value = categ_name;
                    command.Parameters.Add("@gst", MySqlDbType.Double).Value = gst;
                    command.ExecuteScalar();
                    MessageBox.Show("Category Added");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public void UpdateCategory(int id, string categ_name, double gst)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "update category set categ_name=@categ_name, gst=@gst, updated_at=@updated where id=@id";
                    command.Parameters.Add("@categ_name", MySqlDbType.VarChar).Value = categ_name;
                    command.Parameters.Add("@gst", MySqlDbType.Double).Value = gst;
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.Parameters.Add("@updated", MySqlDbType.DateTime).Value = DateTime.Now;
                    command.ExecuteScalar();
                    MessageBox.Show("Category Updated");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }

        public void DeleteCategory(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "delete from category where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.ExecuteScalar();
                    MessageBox.Show("Category Deleted");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }            
        }
    }
}
