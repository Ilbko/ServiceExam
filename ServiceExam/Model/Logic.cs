using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CookieService;
using Dapper;

namespace ServiceExam.Model
{
    public static class Logic
    {
        //internal static void ParseGoogleCookies(ref DataGrid cookiesDataGrid)
        //{

        //}
        //internal static void ParseMozillaCookies(ref DataGrid cookiesDataGrid)
        //{

        //}
        //internal static void ParseOperaCookies(ref DataGrid cookiesDataGrid)
        //{

        //}
        internal static void ParseCookies(ref DataGrid cookiesDataGrid, Logger childLogger)
        {
            string connStr = $"Data Source = {childLogger.cookiePath + @"\Cookies"}; ProviderName = System.Data.SQLite";
            using (SQLiteConnection connection = new SQLiteConnection(connStr))
            {
                GC.Collect(GC.GetGeneration(cookiesDataGrid.ItemsSource));
                cookiesDataGrid.ItemsSource = connection.Query("SELECT * FROM cookies").ToList();
            }
        }
    }
}
