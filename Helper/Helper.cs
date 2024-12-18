using Microsoft.Office.Core;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Cosmetify.Helper
{
    public static class Helper
    {
        public static Brush PickBrush()
        {
            Brush result = Brushes.Transparent;

            Random rnd = new Random();

            Type brushesType = typeof(Brushes);

            PropertyInfo[] properties = brushesType.GetProperties();

            int random = rnd.Next(properties.Length);
            result = (Brush)properties[random].GetValue(null, null);

            return result;
        }

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            var count = 0;
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            //foreach (PropertyInfo prop in Props)
            //{
            //    if (prop.Name.ToLower() == "id" || prop.Name.ToLower().Contains("category") || prop.Name.ToLower().Contains("diffstock"))
            //    {
            //        count++;
            //        continue;
            //    }

            //    //Setting column names as Property names
            //    //dataTable.Columns.Add(prop.Name);
            //}

            dataTable.Columns.Add("Item Name");
            dataTable.Columns.Add("Product Name");
            dataTable.Columns.Add("Brand Name");
            dataTable.Columns.Add("Pkg Type");
            dataTable.Columns.Add("Pkg Qty");
            dataTable.Columns.Add("Batch Qty");
            dataTable.Columns.Add("Qty Reqt");
            dataTable.Columns.Add("Total Orders");
            dataTable.Columns.Add("Total Reqd");
            dataTable.Columns.Add("Total Stock");
            dataTable.Columns.Add("Balance Qty");
            dataTable.Columns.Add("SKU");
            dataTable.Columns.Add("Created Orders");
            dataTable.Columns.Add("Created Reqd Qty");
            dataTable.Columns.Add("Hold Orders");
            dataTable.Columns.Add("Hold Reqd Qty");

            foreach (T item in items)
            {
                var j = 0;
                var values = new object[16];
                //for (int i = 0; i < Props.Length; i++)
                //{
                //    var name = Props[i].Name;
                //    if (name.ToLower() == "id" || name.ToLower().Contains("category") || name.ToLower().Contains("diffstock"))
                //    {
                //        j++;
                //        continue;
                //    }

                //    item.GetType().GetProperty("")
                //    //inserting property values to datatable rows
                //    values[i - j] = Props[i].GetValue(item, null);
                //}
                values[0] = item.GetType().GetProperty("ActivesName").GetValue(item);
                values[1] = item.GetType().GetProperty("ProductNames").GetValue(item);
                values[2] = item.GetType().GetProperty("BrandNames").GetValue(item);
                values[3] = item.GetType().GetProperty("PkgTypes").GetValue(item);
                values[4] = item.GetType().GetProperty("PkgQty").GetValue(item);
                values[5] = item.GetType().GetProperty("BatchQty").GetValue(item);
                values[6] = item.GetType().GetProperty("QtyReqd").GetValue(item);
                values[7] = item.GetType().GetProperty("TotalBatchOrders").GetValue(item);
                values[8] = item.GetType().GetProperty("TotalRequired").GetValue(item);
                values[9] = item.GetType().GetProperty("Stocks").GetValue(item);
                values[10] = item.GetType().GetProperty("RemainingStock").GetValue(item);
                values[11] = item.GetType().GetProperty("SKU").GetValue(item);
                values[12] = item.GetType().GetProperty("TotalCreated").GetValue(item);
                values[13] = item.GetType().GetProperty("TotalCreatedRequired").GetValue(item);
                values[14] = item.GetType().GetProperty("TotalHold").GetValue(item);
                values[15] = item.GetType().GetProperty("TotalHoldRequired").GetValue(item);
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataTable ConvertCsvToDataTable(string filePath, int categ, int scateg, int sscateg)
        {
            //reading all the lines(rows) from the file.
            string[] rows = File.ReadAllLines(filePath);

            DataTable dtData = new DataTable();
            string[] rowValues = null;
            DataRow dr = dtData.NewRow();

            //Creating columns
            if (rows.Length > 0)
            {
                foreach (string columnName in rows[0].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries))
                    dtData.Columns.Add(columnName);
            }

            dtData.Columns.Add("category");
            dtData.Columns.Add("subcategory");
            dtData.Columns.Add("subsubcategory");

            //Creating row for each line.(except the first line, which contain column names)
            for (int row = 1; row < rows.Length; row++)
            {
                var rowStr = new string[8];
                rowValues = rows[row].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < rowValues.Length; i++)
                {
                    rowStr[i] = rowValues[i].Trim();
                }

                rowStr[5] = categ.ToString();
                rowStr[6] = scateg.ToString();
                rowStr[7] = sscateg.ToString();

                dr = dtData.NewRow();
                dr.ItemArray = rowStr;
                dtData.Rows.Add(dr);
            }

            return dtData;
        }

        public static void LogError(Exception e)
        {
            try
            {
                if (File.Exists(System.IO.Path.GetTempPath() + "\\BK_Error_Log.txt"))
                {
                    File.AppendAllText(System.IO.Path.GetTempPath() + "\\BK_Error_Log.txt", DateTime.Now.ToString() + " Sentry Method: " + e.Message + Environment.NewLine);
                }
                else
                {
                    File.Create(System.IO.Path.GetTempPath() + "\\BK_Error_Log.txt");
                    File.AppendAllText(System.IO.Path.GetTempPath() + "\\BK_Error_Log.txt", DateTime.Now.ToString() + " Sentry Method: " + e.Message + Environment.NewLine);
                }
            }
            catch (Exception)
            {
            }
        }

        public static void BugReport(Exception e)
        {
            try
            {
#if !DEBUG
                SentrySdk.Init(o =>
                {
                    // Tells which project in Sentry to send events to:
                    o.Dsn = "https://9e3abd3f3a55309e9f69813acfa722bd@o4508244693286912.ingest.us.sentry.io/4508244697153536";
                    // When configuring for the first time, to see what the SDK is doing:
                    o.Debug = false;
                    o.AttachStacktrace = true;
                    // Set TracesSampleRate to 1.0 to capture 100% of transactions for tracing.
                    // We recommend adjusting this value in production.
                    o.TracesSampleRate = 1.0;
                });

                SentrySdk.CaptureException(e);
                File.AppendAllText(System.IO.Path.GetTempPath() + "\\BK_Error_Log.txt", DateTime.Now.ToString() + " Sentry Method: " + e.Message + Environment.NewLine);
#endif
            }
            catch (Exception ex)
            {
                LogError(ex);
            }            
        }
    }
}
