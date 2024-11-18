using Cosmetify.Model;
using Cosmetify.Model.Enums;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Cosmetify.Repository
{
    public class ProductRepository : RepositoryBase
    {
        public ProductRepository() {
            this.SubCategoryRepository = new SubCategoryRepository();
            this.CategoryRepository = new CategoryRepository(this.SubCategoryRepository);
            this.SubSubCategoryRepository = new SubSubCategoryRepository(this.SubCategoryRepository);
            this.SubCategoryRepository.CategoryRepository = this.CategoryRepository;
            this.SubCategoryRepository.SubSubCategoryRepository = this.SubSubCategoryRepository;
        }

        public CategoryRepository CategoryRepository { get; set; }

        public SubCategoryRepository SubCategoryRepository { get; set; }

        public SubSubCategoryRepository SubSubCategoryRepository { get; set; }

        public ProductModel GetProduct(int id)
        {
            ProductModel? product = null;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from purchase where id=@id";
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        product = new ProductModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Category = reader.IsDBNull(3) ? null : this.CategoryRepository.GetCategory(reader.GetInt32(3)),
                            Status = reader.IsDBNull(4) ? ProductStatus.Block : (ProductStatus)Enum.Parse(typeof(ProductStatus), reader.GetString(4)),
                            Packing = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            BatchNo = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            Expiry = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7),
                            MfgDate = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8),
                            Stock = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                            PurchasePrice = reader.IsDBNull(10) ? 0 : reader.GetDouble(10),
                            MRP = reader.IsDBNull(11) ? 0 : reader.GetDouble(11),
                            MaxSellingPrice = reader.IsDBNull(12) ? 0 : reader.GetDouble(12),
                            MinSellingPrice = reader.IsDBNull(13) ? 0 : reader.GetDouble(13),
                            MfrName = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                            Company = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                            Rate = reader.IsDBNull(16) ? 0 : reader.GetDouble(16),
                            ProductImage = reader.IsDBNull(18) ? null : ByteToImage((byte[])reader["prod_img"])
                        };
                    }

                    reader.Close();
                }
            }

            return product;
        }

        public ObservableCollection<ProductModel> GetAllProducts()
        {
            ObservableCollection<ProductModel> leads = null;
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "select * from purchase";
                var reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    leads = new ObservableCollection<ProductModel>();
                    while (reader.Read())
                    {
                        var lead = new ProductModel
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                            Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            Category = reader.IsDBNull(3) ? null : this.CategoryRepository.GetCategory(reader.GetInt32(3)),
                            Status = reader.IsDBNull(4) ? ProductStatus.Block : (ProductStatus)Enum.Parse(typeof(ProductStatus), reader.GetString(4)),
                            Packing = reader.IsDBNull(5) ? string.Empty : reader.GetString(5),
                            BatchNo = reader.IsDBNull(6) ? string.Empty : reader.GetString(6),
                            Expiry = reader.IsDBNull(7) ? DateTime.MinValue : reader.GetDateTime(7),
                            MfgDate = reader.IsDBNull(8) ? DateTime.MinValue : reader.GetDateTime(8),
                            Stock = reader.IsDBNull(9) ? 0 : reader.GetInt32(9),
                            PurchasePrice = reader.IsDBNull(10) ? 0 : reader.GetDouble(10),
                            MRP = reader.IsDBNull(11) ? 0 : reader.GetDouble(11),
                            MaxSellingPrice = reader.IsDBNull(12) ? 0 : reader.GetDouble(12),
                            MinSellingPrice = reader.IsDBNull(13) ? 0 : reader.GetDouble(13),
                            MfrName = reader.IsDBNull(14) ? string.Empty : reader.GetString(14),
                            Company = reader.IsDBNull(15) ? string.Empty : reader.GetString(15),
                            Rate = reader.IsDBNull(16) ? 0 : reader.GetDouble(16),
                            ProductImage = reader.IsDBNull(18) ? null : ByteToImage((byte[])reader["prod_img"])
                        };

                        leads.Add(lead);
                    }

                    reader.Close();
                }
            }

            return leads;
        }

        public void InsertProduct(ProductModel lead)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "insert into purchase(Name, Description, Category, Status, Packing, BatchNo, Expiry, MfgDate, Stock, PurchasePrice, MRP, MaxSP, MinSP, MfrName, Company, rate, data, prod_img) values(@Name, @Description, @Category, @Status, @Packing, @BatchNo, @Expiry, @MfgDate, @Stock, @PurchasePrice, @MRP, @MaxSP, @MinSP, @MfrName, @Company, @rate, @data, @prod_img)";
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = lead.Name;
                command.Parameters.Add("@Description", MySqlDbType.VarChar).Value = lead.Description;
                command.Parameters.Add("@Category", MySqlDbType.Int32).Value = lead.Category.Id;
                command.Parameters.Add("@Status", MySqlDbType.VarChar).Value = lead.Status;
                command.Parameters.Add("@Packing", MySqlDbType.VarChar).Value = lead.Packing;
                command.Parameters.Add("@BatchNo", MySqlDbType.VarChar).Value = lead.BatchNo;
                command.Parameters.Add("@Expiry", MySqlDbType.DateTime).Value = lead.Expiry;
                command.Parameters.Add("@MfgDate", MySqlDbType.DateTime).Value = lead.MfgDate;
                command.Parameters.Add("@Stock", MySqlDbType.Int32).Value = lead.Stock;
                command.Parameters.Add("@PurchasePrice", MySqlDbType.Double).Value = lead.PurchasePrice;
                command.Parameters.Add("@MRP", MySqlDbType.Double).Value = lead.MRP;
                command.Parameters.Add("@MaxSP", MySqlDbType.Double).Value = lead.MaxSellingPrice;
                command.Parameters.Add("@MinSP", MySqlDbType.Double).Value = lead.MinSellingPrice;
                command.Parameters.Add("@MfrName", MySqlDbType.VarChar).Value = lead.MfrName;
                command.Parameters.Add("@Company", MySqlDbType.VarChar).Value = lead.Company;
                command.Parameters.Add("@rate", MySqlDbType.Double).Value = lead.Rate;
                command.Parameters.Add("@data", MySqlDbType.VarChar).Value = null;
                command.Parameters.Add("@prod_img", MySqlDbType.MediumBlob).Value = ImageToByte(lead.ProductImage);
                command.ExecuteScalar();
                MessageBox.Show("Product Added");
            }
        }

        public void UpdateProduct(ProductModel lead)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "update purchase set Name=@Name, Description=@Description, Category=@Category, Status=@Status, Packing=@Packing, BatchNo=@BatchNo, Expiry=@Expiry, MfgDate=@MfgDate, Stock=@Stock, PurchasePrice=@PurchasePrice, MRP=@MRP, MaxSP=@MaxSP, MinSP=@MinSP, MfrName=@MfrName, Company=@Company, rate=@rate, data=@data, prod_img=@prod_img where id=@id";
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = lead.Id;
                command.Parameters.Add("@Name", MySqlDbType.VarChar).Value = lead.Name;
                command.Parameters.Add("@Description", MySqlDbType.VarChar).Value = lead.Description;
                command.Parameters.Add("@Category", MySqlDbType.Int32).Value = lead.Category.Id;
                command.Parameters.Add("@Status", MySqlDbType.VarChar).Value = lead.Status;
                command.Parameters.Add("@Packing", MySqlDbType.VarChar).Value = lead.Packing;
                command.Parameters.Add("@BatchNo", MySqlDbType.VarChar).Value = lead.BatchNo;
                command.Parameters.Add("@Expiry", MySqlDbType.DateTime).Value = lead.Expiry;
                command.Parameters.Add("@MfgDate", MySqlDbType.DateTime).Value = lead.MfgDate;
                command.Parameters.Add("@Stock", MySqlDbType.Int32).Value = lead.Stock;
                command.Parameters.Add("@PurchasePrice", MySqlDbType.Double).Value = lead.PurchasePrice;
                command.Parameters.Add("@MRP", MySqlDbType.Double).Value = lead.MRP;
                command.Parameters.Add("@MaxSP", MySqlDbType.Double).Value = lead.MaxSellingPrice;
                command.Parameters.Add("@MinSP", MySqlDbType.Double).Value = lead.MinSellingPrice;
                command.Parameters.Add("@MfrName", MySqlDbType.VarChar).Value = lead.MfrName;
                command.Parameters.Add("@Company", MySqlDbType.VarChar).Value = lead.Company;
                command.Parameters.Add("@rate", MySqlDbType.Double).Value = lead.Rate;
                command.Parameters.Add("@data", MySqlDbType.JSON).Value = null;
                command.Parameters.Add("@prod_img", MySqlDbType.MediumBlob).Value = ImageToByte(lead.ProductImage);
                command.ExecuteScalar();
                MessageBox.Show("Product Updated");
            }
        }

        public void DeleteProduct(int id)
        {
            using (var connection = GetConnection())
            using (var command = new MySqlCommand())
            {
                connection.Open();
                command.Connection = connection;
                command.CommandText = "delete from purchase where id=@id";
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                command.ExecuteScalar();
                MessageBox.Show("Product Deleted");
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
    }
}
