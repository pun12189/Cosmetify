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

namespace Cosmetify.Repository
{
    public class TransactionRepository : RepositoryBase
    {
        public ObservableCollection<TransactionModel> GetAllTrnsOfOrder(string order_id)
        {
            ObservableCollection<TransactionModel> orders = null;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from transactions where order_id=@order_id";
                command.Parameters.Add("@order_id", MySqlDbType.VarChar).Value = order_id;
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    orders = new ObservableCollection<TransactionModel>();
                    while (reader.Read())
                    {
                        var lead = new TransactionModel
                        {
                            Id = reader.GetInt32(0),
                            Amount = reader.IsDBNull(1) ? 0.0 : reader.GetDouble(1),
                            TransactionDate = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2),
                            TransactionId = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                            Order_Id = reader.IsDBNull(4) ? 0 : reader.GetInt32(4),
                            Message = reader.IsDBNull(5) ? string.Empty : reader.GetString(5)
                        };

                        orders.Add(lead);
                    }

                    reader.Close();
                }
            }

            return orders;
        }

        public void InsertTrns(TransactionModel trans)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "insert into transactions(amount, trns_date, trns_id, order_id, message) values(@amount, @trns_date, @trns_id, @order_id, @message)";
                command.Parameters.Add("@amount", MySqlDbType.Double).Value = trans.Amount;
                command.Parameters.Add("@trns_date", MySqlDbType.DateTime).Value = trans.TransactionDate;
                command.Parameters.Add("@trns_id", MySqlDbType.VarChar).Value = trans.TransactionId;
                command.Parameters.Add("@order_id", MySqlDbType.Int32).Value = trans.Order_Id;
                command.Parameters.Add("@message", MySqlDbType.VarChar).Value = trans.Message;
                command.ExecuteScalar();
                MessageBox.Show("Transaction Added");
            }
        }

        public void UpdateTrns(TransactionModel trans)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "update transactions set amount=@amount, trns_date=@trns_date, trns_id=@trns_id, order_id=@order_id, message=@message where id=@id";
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = trans.Id;
                command.Parameters.Add("@amount", MySqlDbType.Double).Value = trans.Amount;
                command.Parameters.Add("@trns_date", MySqlDbType.DateTime).Value = trans.TransactionDate;
                command.Parameters.Add("@trns_id", MySqlDbType.VarChar).Value = trans.TransactionId;
                command.Parameters.Add("@order_id", MySqlDbType.Int32).Value = trans.Order_Id;
                command.Parameters.Add("@message", MySqlDbType.VarChar).Value = trans.Message;
                command.ExecuteScalar();
                MessageBox.Show("Transaction Updated");
            }
        }

        public void DeleteTrns(int id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "delete from transactions where id=@id";
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.ExecuteScalar();
                MessageBox.Show("Transaction Deleted");
            }
        }
    }
}
