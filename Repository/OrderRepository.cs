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
using Newtonsoft.Json;

namespace BahiKitaab.Repository
{
    public class OrderRepository : RepositoryBase
    {
        public OrderRepository() 
        {
            this.LeadsRepository = new LeadsRepository();
        }

        public LeadsRepository LeadsRepository { get; set; }

        public double GetAmountWithPaymentStatus(string status = null)
        {
            double amount = 0;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                if (status != null)
                {
                    command.CommandText = "select SUM(Amount) from orders where payment_status=@payment_status";
                    command.Parameters.Add("@payment_status", MySqlDbType.VarChar).Value = status;
                }
                else
                {
                    command.CommandText = "select SUM(Amount) from orders";
                }              
                
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        amount = reader.IsDBNull(0) ? 0.0 : reader.GetDouble(0);
                    }

                    reader.Close();
                }
            }

            return amount;
        }

        public int GetCancelledOrders(string status)
        {
            int count = 0;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select COUNT(id) from orders where order_status=@order_status";
                command.Parameters.Add("@order_status", MySqlDbType.VarChar).Value = status;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        count = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                    }

                    reader.Close();
                }
            }

            return count;
        }

        public OrderModel GetOrder(int id)
        {
            OrderModel? order = null;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from orders where id=@id";
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        order = new OrderModel
                        {
                            Id = reader.GetInt32(0),
                            OrderId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            PaymentType = reader.IsDBNull(2) ? PaymentType.Cash : (PaymentType)Enum.Parse(typeof(PaymentType), reader.GetString(2)),
                            Amount = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3),
                            Balance = reader.IsDBNull(4) ? 0.0 : reader.GetDouble(4),
                            Customer = reader.IsDBNull(5) ? null : this.LeadsRepository.GetCustomer(reader.GetInt32(5)),
                            PaymentStatus = reader.IsDBNull(6) ? PaymentStatus.Unpaid : (PaymentStatus)Enum.Parse(typeof(PaymentStatus), reader.GetString(6)),
                            OrderStatus = reader.IsDBNull(7) ? OrderStatus.Awaiting : (OrderStatus)Enum.Parse(typeof(OrderStatus), reader.GetString(7)),
                            TakenBy = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                            Created = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9),
                            Updated = reader.IsDBNull(10) ? DateTime.MinValue : reader.GetDateTime(10),
                            OrderedProducts = reader.IsDBNull(11) ? new ObservableCollection<OrderedProduct>() : JsonConvert.DeserializeObject<ObservableCollection<OrderedProduct>>(reader.GetString(11)),
                        };
                    }

                    reader.Close();
                }
            }

            return order;
        }

        public ObservableCollection<OrderModel> GetAllOrders()
        {
            ObservableCollection<OrderModel> orders = new ObservableCollection<OrderModel>();
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from orders";
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var lead = new OrderModel
                        {
                            Id = reader.GetInt32(0),
                            OrderId = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            PaymentType = reader.IsDBNull(2) ? PaymentType.Cash : (PaymentType)Enum.Parse(typeof(PaymentType), reader.GetString(2)),
                            Amount = reader.IsDBNull(3) ? 0.0 : reader.GetDouble(3),
                            Balance = reader.IsDBNull(4) ? 0.0 : reader.GetDouble(4),
                            Customer = reader.IsDBNull(5) ? null : this.LeadsRepository.GetCustomer(reader.GetInt32(5)),
                            PaymentStatus = reader.IsDBNull(6) ? PaymentStatus.Unpaid : (PaymentStatus)Enum.Parse(typeof(PaymentStatus), reader.GetString(6)),
                            OrderStatus = reader.IsDBNull(7) ? OrderStatus.Awaiting : (OrderStatus)Enum.Parse(typeof(OrderStatus), reader.GetString(7)),
                            TakenBy = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                            Created = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9),
                            Updated = reader.IsDBNull(10) ? DateTime.MinValue : reader.GetDateTime(10),
                            OrderedProducts = reader.IsDBNull(11) ? new ObservableCollection<OrderedProduct>() : JsonConvert.DeserializeObject<ObservableCollection<OrderedProduct>>(reader.GetString(11)),
                        };

                        orders.Add(lead);
                    }

                    reader.Close();
                }
            }

            return orders;
        }

        public void InsertOrder(OrderModel order)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "insert into orders(order_id, payment_type, amount, balance, customer_id, payment_status, order_status, takenby, created_at, updated_at, ordered_products) values(@order_id, @payment_type, @amount, @balance, @customer_id, @payment_status, @order_status, @takenby, @created_at, @updated_at, @ordered_products)";
                command.Parameters.Add("@order_id", MySqlDbType.VarChar).Value = order.OrderId;
                command.Parameters.Add("@payment_type", MySqlDbType.VarChar).Value = order.PaymentType;
                command.Parameters.Add("@amount", MySqlDbType.Double).Value = order.Amount;
                command.Parameters.Add("@balance", MySqlDbType.Double).Value = order.Balance;
                command.Parameters.Add("@customer_id", MySqlDbType.Int32).Value = order.Customer.Id;
                command.Parameters.Add("@payment_status", MySqlDbType.VarChar).Value = order.PaymentStatus;
                command.Parameters.Add("@order_status", MySqlDbType.VarChar).Value = order.OrderStatus;
                command.Parameters.Add("@takenby", MySqlDbType.VarChar).Value = order.TakenBy;
                command.Parameters.Add("@created_at", MySqlDbType.DateTime).Value = order.Created;
                command.Parameters.Add("@updated_at", MySqlDbType.DateTime).Value = order.Updated;
                command.Parameters.Add("@ordered_products", MySqlDbType.JSON).Value = JsonConvert.SerializeObject(order.OrderedProducts);
                command.ExecuteScalar();
                MessageBox.Show("Order Added");
            }
        }

        public void UpdateOrder(OrderModel order)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "update orders set order_id=@order_id, payment_type=@payment_type, amount=@amount, balance=@balance, customer_id=@customer_id, payment_status=@payment_status, order_status=@order_status, takenby=@takenby, created_at=@created_at, updated_at=@updated_at, ordered_products=@ordered_products where id=@id";
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = order.Id;
                command.Parameters.Add("@order_id", MySqlDbType.VarChar).Value = order.OrderId;
                command.Parameters.Add("@payment_type", MySqlDbType.VarChar).Value = order.PaymentType;
                command.Parameters.Add("@amount", MySqlDbType.Double).Value = order.Amount;
                command.Parameters.Add("@balance", MySqlDbType.Double).Value = order.Balance;
                command.Parameters.Add("@customer_id", MySqlDbType.Int32).Value = order.Customer.Id;
                command.Parameters.Add("@payment_status", MySqlDbType.VarChar).Value = order.PaymentStatus;
                command.Parameters.Add("@order_status", MySqlDbType.VarChar).Value = order.OrderStatus;
                command.Parameters.Add("@takenby", MySqlDbType.VarChar).Value = order.TakenBy;
                command.Parameters.Add("@created_at", MySqlDbType.DateTime).Value = order.Created;
                command.Parameters.Add("@updated_at", MySqlDbType.DateTime).Value = order.Updated;
                command.Parameters.Add("@ordered_products", MySqlDbType.JSON).Value = JsonConvert.SerializeObject(order.OrderedProducts);
                command.ExecuteScalar();
                MessageBox.Show("Order Updated");
            }
        }

        public void DeleteOrder(int id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "delete from orders where id=@id";
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.ExecuteScalar();
                MessageBox.Show("Order Deleted");
            }
        }
    }
}
