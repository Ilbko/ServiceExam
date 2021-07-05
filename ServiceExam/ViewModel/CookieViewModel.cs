using ServiceExam.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ServiceExam.Model;
using CookieService;
using System.IO;

namespace ServiceExam.ViewModel
{
    public class CookieViewModel
    {
        private DataGrid cookiesDataGrid;

        private RelayCommand googleCommand;
        public RelayCommand GoogleCommand
        {
            get
            {
                return googleCommand ?? new RelayCommand(act => Logic.ParseCookies(ref this.cookiesDataGrid, Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Google\Chrome\User Data\Default\Cookies"));
            }
        }

        private RelayCommand operaCommand;
        public RelayCommand OperaCommand
        {
            get
            {
                return operaCommand ?? new RelayCommand(act => Logic.ParseCookies(ref this.cookiesDataGrid, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Opera Software\Opera Stable\Cookies"));
            }
        }

        private RelayCommand mozillaCommand;
        public RelayCommand MozillaCommand
        {
            get
            {
                return mozillaCommand ?? new RelayCommand(act => Logic.ParseCookies(ref this.cookiesDataGrid, Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Mozilla\Firefox\Profiles", "cookies.sqlite", SearchOption.AllDirectories)[0]));
            }
        }

        public CookieViewModel(ref DataGrid cookiesDataGrid) => this.cookiesDataGrid = cookiesDataGrid;
    }
}
