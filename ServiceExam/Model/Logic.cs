using CookieService;
using Dapper;
using System;
using System.Data.SQLite;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
        //internal static void ParseCookies(ref DataGrid cookiesDataGrid, Logger childLogger)
        //{
        //    string connStr = $"Data Source = {childLogger.cookiePath + @"\Cookies"}; ProviderName = System.Data.SQLite";
        //    using (SQLiteConnection connection = new SQLiteConnection(connStr))
        //    {
        //        if (cookiesDataGrid.ItemsSource != null)
        //            GC.Collect(GC.GetGeneration(cookiesDataGrid.ItemsSource));

        //        cookiesDataGrid.ItemsSource = connection.Query("SELECT * FROM cookies").ToList();
        //    }
        //}

        //Метод для получения куки браузера
        internal static void ParseCookies(ref DataGrid cookiesDataGrid, string cookiePath)
        {
            string connStr = $"Data Source = {cookiePath}; ProviderName = System.Data.SQLite";
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(connStr))
                {
                    //Не мешало бы подчистить за собой память в случае переопределения источника данных датагрида
                    if (cookiesDataGrid.ItemsSource != null)
                        GC.Collect(GC.GetGeneration(cookiesDataGrid.ItemsSource));

                    //У Mozilla Firefox таблица с куки, в отличии от остальных браузеров, называется "moz_cookies"
                    if (!cookiePath.Contains("Mozilla"))
                        cookiesDataGrid.ItemsSource = connection.Query("SELECT * FROM cookies").ToList();
                    else
                        cookiesDataGrid.ItemsSource = connection.Query("SELECT * FROM moz_cookies").ToList();
                }
            } catch (System.Exception e) { MessageBox.Show(e.Message); }
        }
    }
}
