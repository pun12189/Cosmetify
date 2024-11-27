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
    public class PerfumeRepository : RepositoryBase
    {
        public PerfumeModel GetPerfume(int id)
        {
            PerfumeModel? color = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from perfume where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            color = new PerfumeModel();
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

        public ObservableCollection<PerfumeModel> GetAllPerfumes()
        {
            ObservableCollection<PerfumeModel> perfume = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from perfume";
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        perfume = new ObservableCollection<PerfumeModel>();
                        while (reader.Read())
                        {
                            var category = new PerfumeModel();
                            category.Id = reader.GetInt32(0);
                            category.Name = reader.GetString(1);
                            perfume.Add(category);
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }

            return perfume;
        }

        public void InsertPerfume(string name)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into perfume(perfume) values(@perfume)";
                    command.Parameters.Add("@perfume", MySqlDbType.VarChar).Value = name;
                    command.ExecuteScalar();
                    MessageBox.Show("Perfume Added");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }
        }

        public void UpdatePerfume(PerfumeModel colours)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "update perfume set perfume=@perfume where id=@id";
                    command.Parameters.Add("@perfume", MySqlDbType.VarChar).Value = colours.Name;
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = colours.Id;
                    command.ExecuteScalar();
                    MessageBox.Show("Perfume Updated");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }
        }

        public void DeletePerfume(int id)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "delete from perfume where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.ExecuteScalar();
                    MessageBox.Show("Perfume Deleted");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }
        }
    }
}
