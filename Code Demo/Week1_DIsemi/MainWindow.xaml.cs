using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Week1_DI.DataAccess;
using Week1_DI.Services;

namespace Week1_DI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ICategoryDA categoryDA = new CategoryDASQLServer();
            ICategoryServices categoryServices = new CategoryServicesVer1(categoryDA);
            cbCategories.DisplayMemberPath = "Name";
            cbCategories.SelectedValuePath = "Id";
            cbCategories.ItemsSource = categoryServices.GetCategories();
        }
    }
}