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

namespace BahiKitaab.Helper
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

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static DataTable ConvertCsvToDataTable(string filePath)
        {
            //reading all the lines(rows) from the file.
            string[] rows = File.ReadAllLines(filePath);

            DataTable dtData = new DataTable();
            string[] rowValues = null;
            DataRow dr = dtData.NewRow();

            //Creating columns
            if (rows.Length > 0)
            {
                foreach (string columnName in rows[0].Split(','))
                    dtData.Columns.Add(columnName);
            }

            //Creating row for each line.(except the first line, which contain column names)
            for (int row = 1; row < rows.Length; row++)
            {
                rowValues = rows[row].Split(',');
                dr = dtData.NewRow();
                dr.ItemArray = rowValues;
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
