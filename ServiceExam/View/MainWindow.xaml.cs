using ServiceExam.ViewModel;
using System.Windows;

namespace ServiceExam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new CookieViewModel(ref this.CookiesDataGrid);
        }
    }
}
