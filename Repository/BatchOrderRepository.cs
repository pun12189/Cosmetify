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
using Cosmetify.ViewModel;
using System.Text.Json;
using System.Windows.Media.Imaging;
using System.IO;
using System.Globalization;

namespace Cosmetify.Repository
{
    public class BatchOrderRepository : RepositoryBase
    {
        private JsonSerializerOptions options = new() { IncludeFields = true, PropertyNameCaseInsensitive = true };
        public BatchModel GetProduct(int id)
        {
            BatchModel? product = null;
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from batchorder where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            product = new BatchModel
                            {
                                Id = reader.GetInt32(0),
                                BatchOrderNo = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Customer = reader.IsDBNull(2) ? null : HomepageViewModel.CommonViewModel.LeadsRepository.GetCustomer(reader.GetInt32(2)),
                                ProductName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                BatchDate = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4),
                                Expiry = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                                BatchOrderCollection = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<ObservableCollection<BatchOrderModel>>(reader.GetString(6), options),
                                PkgType = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                PkgOrderQuantity = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                Description = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                Remarks = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                AdditionalInfo = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                PlanningDate = reader.IsDBNull(12) ? DateTime.Now : reader.GetDateTime(12),
                                PlannedDate = reader.IsDBNull(13) ? DateTime.Now : reader.GetDateTime(13),
                                MfgDate = reader.IsDBNull(14) ? DateTime.Now : reader.GetDateTime(14),
                                CompletionDate = reader.IsDBNull(15) ? DateTime.Now : reader.GetDateTime(15),
                                Status = reader.IsDBNull(16) ? BatchStatus.Created : (BatchStatus)Enum.Parse(typeof(BatchStatus), reader.GetString(16)),
                                OrderStage = reader.IsDBNull(17) ? null : JsonSerializer.Deserialize<OrderStageModel>(reader.GetString(17), options),
                                OrderId = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                                Colour = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                                Perfume = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                                Claims = reader.IsDBNull(21) ? null : JsonSerializer.Deserialize<ObservableCollection<string>>(reader.GetString(21), options),
                                PackagingTypeImage = reader.IsDBNull(22) ? null : ByteToImage((byte[])reader["pkg_img"]),
                                BrandName = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                                ProductID = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
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

        public ObservableCollection<BatchModel> SearchBatch(string data)
        {
            ObservableCollection<BatchModel> batches = new ObservableCollection<BatchModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from batchorder where order_no LIKE @data OR add_info LIKE @data OR order_id LIKE @data OR color LIKE @data OR perfume LIKE @data OR brand_name LIKE @data OR product_id LIKE @data OR remarks LIKE @data OR description LIKE @data OR prod_name LIKE @data OR status LIKE @data OR pkgtype LIKE @data";
                    command.Parameters.Add("@data", MySqlDbType.String).Value = "%" + data + "%";
                    MySqlDataReader reader = command.ExecuteReader(); 
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var batch = new BatchModel
                            {
                                Id = reader.GetInt32(0),
                                BatchOrderNo = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Customer = reader.IsDBNull(2) ? null : HomepageViewModel.CommonViewModel.LeadsRepository.GetCustomer(reader.GetInt32(2)),
                                ProductName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                BatchDate = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4),
                                Expiry = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                                BatchOrderCollection = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<ObservableCollection<BatchOrderModel>>(reader.GetString(6), options),
                                PkgType = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                PkgOrderQuantity = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                Description = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                Remarks = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                AdditionalInfo = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                PlanningDate = reader.IsDBNull(12) ? DateTime.Now : reader.GetDateTime(12),
                                PlannedDate = reader.IsDBNull(13) ? DateTime.Now : reader.GetDateTime(13),
                                MfgDate = reader.IsDBNull(14) ? DateTime.Now : reader.GetDateTime(14),
                                CompletionDate = reader.IsDBNull(15) ? DateTime.Now : reader.GetDateTime(15),
                                Status = reader.IsDBNull(16) ? BatchStatus.Created : (BatchStatus)Enum.Parse(typeof(BatchStatus), reader.GetString(16)),
                                OrderStage = reader.IsDBNull(17) ? null : JsonSerializer.Deserialize<OrderStageModel>(reader.GetString(17), options),
                                OrderId = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                                Colour = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                                Perfume = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                                Claims = reader.IsDBNull(21) ? null : JsonSerializer.Deserialize<ObservableCollection<string>>(reader.GetString(21), options),
                                PackagingTypeImage = reader.IsDBNull(22) ? null : ByteToImage((byte[])reader["pkg_img"]),
                                BrandName = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                                ProductID = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
                            };

                            batches.Add(batch);
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }

            return batches;
        }

        public ObservableCollection<BatchModel> BatchFilters(string? fromDate = null, string? toDate = null)
        {
            ObservableCollection<BatchModel> batches = new ObservableCollection<BatchModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    var cmdText = "select * from batchorder where";
                    var conditions = string.Empty;
                    if (fromDate != null)
                    {
                        var date = DateTime.ParseExact(fromDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        fromDate = date.ToString("yyyy-MM-dd");
                        conditions = " planning_date BETWEEN " + "'" + fromDate + "'";
                    }
                    else
                    {
                        conditions = " planning_date BETWEEN " + "'" + DateTime.Now.Date.ToString("yyyy-MM-dd") + "'";
                    }

                    if (toDate != null)
                    {
                        var date = DateTime.ParseExact(toDate, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                        toDate = date.ToString("yyyy-MM-dd");
                        conditions = conditions + " AND" + "'" + toDate + "'";                        
                    }
                    else
                    {
                        conditions = conditions + " AND " + "'" + DateTime.Now.AddDays(30).Date.ToString("yyyy-MM-dd") + "'";
                    }

                    command.CommandText = cmdText + conditions;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var btch = new BatchModel
                            {
                                Id = reader.GetInt32(0),
                                BatchOrderNo = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Customer = reader.IsDBNull(2) ? null : HomepageViewModel.CommonViewModel.LeadsRepository.GetCustomer(reader.GetInt32(2)),
                                ProductName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                BatchDate = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4),
                                Expiry = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                                BatchOrderCollection = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<ObservableCollection<BatchOrderModel>>(reader.GetString(6), options),
                                PkgType = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                PkgOrderQuantity = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                Description = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                Remarks = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                AdditionalInfo = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                PlanningDate = reader.IsDBNull(12) ? DateTime.Now : reader.GetDateTime(12),
                                PlannedDate = reader.IsDBNull(13) ? DateTime.Now : reader.GetDateTime(13),
                                MfgDate = reader.IsDBNull(14) ? DateTime.Now : reader.GetDateTime(14),
                                CompletionDate = reader.IsDBNull(15) ? DateTime.Now : reader.GetDateTime(15),
                                Status = reader.IsDBNull(16) ? BatchStatus.Created : (BatchStatus)Enum.Parse(typeof(BatchStatus), reader.GetString(16)),
                                OrderStage = reader.IsDBNull(17) ? null : JsonSerializer.Deserialize<OrderStageModel>(reader.GetString(17), options),
                                OrderId = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                                Colour = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                                Perfume = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                                Claims = reader.IsDBNull(21) ? null : JsonSerializer.Deserialize<ObservableCollection<string>>(reader.GetString(21), options),
                                PackagingTypeImage = reader.IsDBNull(22) ? null : ByteToImage((byte[])reader["pkg_img"]),
                                BrandName = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                                ProductID = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
                            };

                            batches.Add(btch);
                        }

                        reader.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }

            return batches;
        }

        public ObservableCollection<BatchModel> BatchFilters(int? custid = null, string? orderid = null, string? pkgtype = null, string? bName = null, string? status = null, string? mfgDate = null, string? expDate = null, string? cdDate = null)
        {
            ObservableCollection<BatchModel> batches = new ObservableCollection<BatchModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    bool isappend = false;
                    var cmdText = "select * from batchorder where";
                    if (custid != null)
                    {
                        isappend = true;
                        cmdText += cmdText + " cust_id=" + custid;
                    }

                    if (orderid != null)
                    {
                        if (isappend)
                        {
                            cmdText += cmdText + " AND order_no=" + orderid;
                        }
                        else
                        {
                            isappend = true;
                            cmdText += cmdText + " order_no=" + orderid;
                        }

                    }

                    if (pkgtype != null)
                    {
                        if (isappend)
                        {
                            cmdText += cmdText + " AND pkgtype=" + pkgtype;
                        }
                        else
                        {
                            isappend = true;
                            cmdText += cmdText + " pkgtype=" + pkgtype;
                        }
                    }

                    if (bName != null)
                    {
                        if (isappend)
                        {
                            cmdText += cmdText + " AND prod_name=" + bName;
                        }
                        else
                        {
                            isappend = true;
                            cmdText += cmdText + " prod_name=" + bName;
                        }
                    }

                    if (status != null)
                    {
                        if (isappend)
                        {
                            cmdText += cmdText + " AND status=" + status;
                        }
                        else
                        {
                            isappend = true;
                            cmdText += cmdText + " status=" + status;
                        }
                    }

                    if (mfgDate != null)
                    {
                        if (isappend)
                        {
                            cmdText += cmdText + " AND mfg_date= " + mfgDate;
                        }
                        else
                        {
                            isappend = true;
                            cmdText += cmdText + " mfg_date= " + mfgDate;
                        }
                    }

                    if (expDate != null)
                    {
                        if (isappend)
                        {
                            cmdText += cmdText + " AND expiry= " + expDate;
                        }
                        else
                        {
                            isappend = true;
                            cmdText += cmdText + " expiry= " + expDate;
                        }
                    }

                    if (cdDate != null)
                    {
                        if (isappend)
                        {
                            cmdText += cmdText + " AND completion_date= " + cdDate;
                        }
                        else
                        {
                            isappend = true;
                            cmdText += cmdText + " completion_date= " + cdDate;
                        }
                    }

                    command.CommandText = cmdText;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var btch = new BatchModel
                            {
                                Id = reader.GetInt32(0),
                                BatchOrderNo = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Customer = reader.IsDBNull(2) ? null : HomepageViewModel.CommonViewModel.LeadsRepository.GetCustomer(reader.GetInt32(2)),
                                ProductName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                BatchDate = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4),
                                Expiry = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                                BatchOrderCollection = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<ObservableCollection<BatchOrderModel>>(reader.GetString(6), options),
                                PkgType = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                PkgOrderQuantity = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                Description = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                Remarks = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                AdditionalInfo = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                PlanningDate = reader.IsDBNull(12) ? DateTime.Now : reader.GetDateTime(12),
                                PlannedDate = reader.IsDBNull(13) ? DateTime.Now : reader.GetDateTime(13),
                                MfgDate = reader.IsDBNull(14) ? DateTime.Now : reader.GetDateTime(14),
                                CompletionDate = reader.IsDBNull(15) ? DateTime.Now : reader.GetDateTime(15),
                                Status = reader.IsDBNull(16) ? BatchStatus.Created : (BatchStatus)Enum.Parse(typeof(BatchStatus), reader.GetString(16)),
                                OrderStage = reader.IsDBNull(17) ? null : JsonSerializer.Deserialize<OrderStageModel>(reader.GetString(17), options),
                                OrderId = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                                Colour = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                                Perfume = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                                Claims = reader.IsDBNull(21) ? null : JsonSerializer.Deserialize<ObservableCollection<string>>(reader.GetString(21), options),
                                PackagingTypeImage = reader.IsDBNull(22) ? null : ByteToImage((byte[])reader["pkg_img"]),
                                BrandName = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                                ProductID = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
                            };

                            batches.Add(btch);
                        }

                        reader.Close();
                    }
                }

            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }

            return batches;
        }

        public ObservableCollection<BatchModel> GetAllProductsWithOrderId(string orderId)
        {
            ObservableCollection<BatchModel> leads = new ObservableCollection<BatchModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from batchorder where order_id=@order_id";
                    command.Parameters.Add("@order_id", MySqlDbType.VarChar).Value = orderId;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var lead = new BatchModel
                            {
                                Id = reader.GetInt32(0),
                                BatchOrderNo = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Customer = reader.IsDBNull(2) ? null : HomepageViewModel.CommonViewModel.LeadsRepository.GetCustomer(reader.GetInt32(2)),
                                ProductName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                BatchDate = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4),
                                Expiry = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                                BatchOrderCollection = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<ObservableCollection<BatchOrderModel>>(reader.GetString(6), options),
                                PkgType = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                PkgOrderQuantity = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                Description = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                Remarks = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                AdditionalInfo = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                PlanningDate = reader.IsDBNull(12) ? DateTime.Now : reader.GetDateTime(12),
                                PlannedDate = reader.IsDBNull(13) ? DateTime.Now : reader.GetDateTime(13),
                                MfgDate = reader.IsDBNull(14) ? DateTime.Now : reader.GetDateTime(14),
                                CompletionDate = reader.IsDBNull(15) ? DateTime.Now : reader.GetDateTime(15),
                                Status = reader.IsDBNull(16) ? BatchStatus.Created : (BatchStatus)Enum.Parse(typeof(BatchStatus), reader.GetString(16)),
                                OrderStage = reader.IsDBNull(17) ? null : JsonSerializer.Deserialize<OrderStageModel>(reader.GetString(17), options),
                                OrderId = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                                Colour = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                                Perfume = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                                Claims = reader.IsDBNull(21) ? null : JsonSerializer.Deserialize<ObservableCollection<string>>(reader.GetString(21), options),
                                PackagingTypeImage = reader.IsDBNull(22) ? null : ByteToImage((byte[])reader["pkg_img"]),
                                BrandName = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                                ProductID = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
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

        public ObservableCollection<BatchModel> GetAllProducts()
        {
            ObservableCollection<BatchModel> leads = new ObservableCollection<BatchModel>();
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "select * from batchorder";
                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var lead = new BatchModel
                            {
                                Id = reader.GetInt32(0),
                                BatchOrderNo = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                                Customer = reader.IsDBNull(2) ? null : HomepageViewModel.CommonViewModel.LeadsRepository.GetCustomer(reader.GetInt32(2)),
                                ProductName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                                BatchDate = reader.IsDBNull(4) ? DateTime.Now : reader.GetDateTime(4),
                                Expiry = reader.IsDBNull(5) ? DateTime.Now : reader.GetDateTime(5),
                                BatchOrderCollection = reader.IsDBNull(6) ? null : JsonSerializer.Deserialize<ObservableCollection<BatchOrderModel>>(reader.GetString(6), options),
                                PkgType = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                                PkgOrderQuantity = reader.IsDBNull(8) ? string.Empty : reader.GetString(8),
                                Description = reader.IsDBNull(9) ? string.Empty : reader.GetString(9),
                                Remarks = reader.IsDBNull(10) ? string.Empty : reader.GetString(10),
                                AdditionalInfo = reader.IsDBNull(11) ? string.Empty : reader.GetString(11),
                                PlanningDate = reader.IsDBNull(12) ? DateTime.Now : reader.GetDateTime(12),
                                PlannedDate = reader.IsDBNull(13) ? DateTime.Now : reader.GetDateTime(13),
                                MfgDate = reader.IsDBNull(14) ? DateTime.Now : reader.GetDateTime(14),
                                CompletionDate = reader.IsDBNull(15) ? DateTime.Now : reader.GetDateTime(15),
                                Status = reader.IsDBNull(16) ? BatchStatus.Created : (BatchStatus)Enum.Parse(typeof(BatchStatus), reader.GetString(16)),
                                OrderStage = reader.IsDBNull(17) ? null : JsonSerializer.Deserialize<OrderStageModel>(reader.GetString(17), options),
                                OrderId = reader.IsDBNull(18) ? string.Empty : reader.GetString(18),
                                Colour = reader.IsDBNull(19) ? string.Empty : reader.GetString(19),
                                Perfume = reader.IsDBNull(20) ? string.Empty : reader.GetString(20),
                                Claims = reader.IsDBNull(21) ? null : JsonSerializer.Deserialize<ObservableCollection<string>>(reader.GetString(21), options),
                                PackagingTypeImage = reader.IsDBNull(22) ? null : ByteToImage((byte[])reader["pkg_img"]),
                                BrandName = reader.IsDBNull(23) ? string.Empty : reader.GetString(23),
                                ProductID = reader.IsDBNull(24) ? string.Empty : reader.GetString(24),
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

        public void InsertProduct(BatchModel lead)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "insert into batchorder(order_no, cust_id, prod_name, batch_date, expiry, batch_data, pkgtype, pkg_order_quantity, description, remarks, add_info, planning_date, planned_date, mfg_date, completion_date, status, order_stage, order_id, color, perfume, claims, pkg_img, brand_name, product_id) values(@order_no, @cust_id, @prod_name, @batch_date, @expiry, @batch_data, @pkgtype, @pkg_order_quantity, @description, @remarks, @add_info, @planning_date, @planned_date, @mfg_date, @completion_date, @status, @order_stage, @order_id, @color, @perfume, @claims, @pkg_img, @brand_name, @product_id)";
                    command.Parameters.Add("@order_no", MySqlDbType.VarChar).Value = lead.BatchOrderNo;
                    command.Parameters.Add("@cust_id", MySqlDbType.Int32).Value = lead.Customer.Id;
                    command.Parameters.Add("@prod_name", MySqlDbType.VarChar).Value = lead.ProductName;
                    command.Parameters.Add("@batch_date", MySqlDbType.DateTime).Value = lead.BatchDate;
                    command.Parameters.Add("@expiry", MySqlDbType.DateTime).Value = lead.Expiry;
                    command.Parameters.Add("@batch_data", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.BatchOrderCollection, options);
                    command.Parameters.Add("@pkgtype", MySqlDbType.VarChar).Value = lead.PkgType;
                    command.Parameters.Add("@pkg_order_quantity", MySqlDbType.VarChar).Value = lead.PkgOrderQuantity;
                    command.Parameters.Add("@description", MySqlDbType.VarChar).Value = lead.Description;
                    command.Parameters.Add("@remarks", MySqlDbType.VarChar).Value = lead.Remarks;
                    command.Parameters.Add("@add_info", MySqlDbType.VarChar).Value = lead.AdditionalInfo;
                    command.Parameters.Add("@planning_date", MySqlDbType.DateTime).Value = lead.PlanningDate;
                    command.Parameters.Add("@planned_date", MySqlDbType.DateTime).Value = lead.PlannedDate;
                    command.Parameters.Add("@mfg_date", MySqlDbType.DateTime).Value = lead.MfgDate;
                    command.Parameters.Add("@completion_date", MySqlDbType.DateTime).Value = lead.CompletionDate;
                    command.Parameters.Add("@status", MySqlDbType.VarChar).Value = lead.Status;
                    command.Parameters.Add("@order_stage", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.OrderStage, options);
                    command.Parameters.Add("@order_id", MySqlDbType.VarChar).Value = lead.OrderId;
                    command.Parameters.Add("@color", MySqlDbType.VarChar).Value = lead.Colour;
                    command.Parameters.Add("@perfume", MySqlDbType.VarChar).Value = lead.Perfume;
                    command.Parameters.Add("@claims", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Claims, options);
                    command.Parameters.Add("@pkg_img", MySqlDbType.MediumBlob).Value = ImageToByte(lead.PackagingTypeImage);
                    command.Parameters.Add("@brand_name", MySqlDbType.Text).Value = lead.BrandName;
                    command.Parameters.Add("@product_id", MySqlDbType.VarChar).Value = lead.ProductID;
                    command.ExecuteScalar();
                    MessageBox.Show("Batch Order Added");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }
        }

        public void UpdateProduct(BatchModel lead)
        {
            try
            {
                using (var connection = GetConnection())
                using (var command = new MySqlCommand())
                {
                    connection.Open();
                    command.Connection = connection;
                    command.CommandText = "update batchorder set order_no=@order_no, cust_id=@cust_id, prod_name=@prod_name, batch_date=@batch_date, expiry=@expiry, batch_data=@batch_data, pkgtype=@pkgtype, pkg_order_quantity=@pkg_order_quantity, description=@description, remarks=@remarks, add_info=@add_info, planning_date=@planning_date, planned_date=@planned_date, mfg_date=@mfg_date, completion_date=@completion_date, status=@status, order_stage=@order_stage, order_id=@order_id, color=@color, perfume=@perfume, claims=@claims, pkg_img=@pkg_img, brand_name=@brand_name, product_id=@product_id where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                    command.Parameters.Add("@order_no", MySqlDbType.VarChar).Value = lead.BatchOrderNo;
                    command.Parameters.Add("@cust_id", MySqlDbType.Int32).Value = lead.Customer.Id;
                    command.Parameters.Add("@prod_name", MySqlDbType.VarChar).Value = lead.ProductName;
                    command.Parameters.Add("@batch_date", MySqlDbType.DateTime).Value = lead.BatchDate;
                    command.Parameters.Add("@expiry", MySqlDbType.DateTime).Value = lead.Expiry;
                    command.Parameters.Add("@batch_data", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.BatchOrderCollection, options);
                    command.Parameters.Add("@pkgtype", MySqlDbType.VarChar).Value = lead.PkgType;
                    command.Parameters.Add("@pkg_order_quantity", MySqlDbType.VarChar).Value = lead.PkgOrderQuantity;
                    command.Parameters.Add("@description", MySqlDbType.VarChar).Value = lead.Description;
                    command.Parameters.Add("@remarks", MySqlDbType.VarChar).Value = lead.Remarks;
                    command.Parameters.Add("@add_info", MySqlDbType.VarChar).Value = lead.AdditionalInfo;
                    command.Parameters.Add("@planning_date", MySqlDbType.DateTime).Value = lead.PlanningDate;
                    command.Parameters.Add("@planned_date", MySqlDbType.DateTime).Value = lead.PlannedDate;
                    command.Parameters.Add("@mfg_date", MySqlDbType.DateTime).Value = lead.MfgDate;
                    command.Parameters.Add("@completion_date", MySqlDbType.DateTime).Value = lead.CompletionDate;
                    command.Parameters.Add("@status", MySqlDbType.VarChar).Value = lead.Status;
                    command.Parameters.Add("@order_stage", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.OrderStage, options);
                    command.Parameters.Add("@order_id", MySqlDbType.VarChar).Value = lead.OrderId;
                    command.Parameters.Add("@color", MySqlDbType.VarChar).Value = lead.Colour;
                    command.Parameters.Add("@perfume", MySqlDbType.VarChar).Value = lead.Perfume;
                    command.Parameters.Add("@claims", MySqlDbType.JSON).Value = JsonSerializer.Serialize(lead.Claims, options);
                    command.Parameters.Add("@pkg_img", MySqlDbType.MediumBlob).Value = ImageToByte(lead.PackagingTypeImage);
                    command.Parameters.Add("@brand_name", MySqlDbType.Text).Value = lead.BrandName;
                    command.Parameters.Add("@product_id", MySqlDbType.VarChar).Value = lead.ProductID;
                    command.ExecuteScalar();
                    //MessageBox.Show("Batch Order Updated");
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
                    command.CommandText = "delete from batchorder where id=@id";
                    command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    command.ExecuteScalar();
                    //MessageBox.Show("Batch Order Deleted");
                }
            }
            catch (Exception e)
            {
                Helper.Helper.BugReport(e);
            }
        }

        public BitmapImage ByteToImage(byte[] array)
        {
            using (var ms = new System.IO.MemoryStream(array))
            {
                var image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad; // here
                image.StreamSource = ms;
                image.EndInit();
                return image;
            }
        }

        public byte[] ImageToByte(BitmapImage imageIn)
        {
            Byte[]? buffer = null;
            if (imageIn != null)
            {
                var encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(imageIn));

                using (var ms = new MemoryStream())
                {
                    encoder.Save(ms);
                    buffer = ms.ToArray();
                }
            }

            return buffer;
        }
    }
}
