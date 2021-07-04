using ServiceExam.View.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using ServiceExam.Model;
using CookieService;

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
                return googleCommand ?? new RelayCommand(act => Logic.ParseCookies(ref this.cookiesDataGrid, new GoogleLogger()));
            }
        }

        //private RelayCommand mozillaCommand;
        //public RelayCommand MozillaCommand
        //{
        //    get
        //    {
        //        return mozillaCommand ?? new RelayCommand(act => ..Logic.ParseCookies(ref this.cookiesDataGrid));
        //    }
        //}

        //private RelayCommand operaCommand;
        //public RelayCommand OperaCommand
        //{
        //    get
        //    {
        //        return operaCommand ?? new RelayCommand(act => Logic.ParseCookies(ref this.cookiesDataGrid));
        //    }
        //}


        public CookieViewModel(ref DataGrid cookiesDataGrid) => this.cookiesDataGrid = cookiesDataGrid;
    }
}
