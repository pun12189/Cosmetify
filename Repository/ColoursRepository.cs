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
    public class ColoursRepository : RepositoryBase
    {
        public ColoursModel GetColor(int id)
        {
            ColoursModel? color = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from colors where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            color = new ColoursModel();
                            color.Id = reader.GetInt32(0);
                            color.Name = reader.GetString(1);
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }

            return color;
        }

        public ObservableCollection<ColoursModel> GetColors()
        {
            ObservableCollection<ColoursModel> colors = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from colors";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        colors = new ObservableCollection<ColoursModel>();
                        while (reader.Read())
                        {
                            var category = new ColoursModel();
                            category.Id = reader.GetInt32(0);
                            category.Name = reader.GetString(1);
                            colors.Add(category);
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }

            return colors;
        }

        public void InsertColor(string name)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into colors(color) values(@color)";
                    command.Parameters.Add("@color", MySqlDbType.VarChar).Value = name;
                    command.ExecuteScalar();
                    MessageBox.Show("Color Added");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }
        }

        public void UpdateColor(ColoursModel colours)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "update colors set color=@color where id=@id";
                    command.Parameters.Add("@color", MySqlDbType.VarChar).Value = colours.Name;
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = colours.Id;
                    command.ExecuteScalar();
                    MessageBox.Show("Color Updated");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }
        }

        public void DeleteColor(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "delete from colors where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.ExecuteScalar();
                    MessageBox.Show("Color Deleted");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }
        }
    }
}
