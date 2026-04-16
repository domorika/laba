using System.Windows;
using System.Windows.Data;
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    public partial class WindowEmployee : Window
    {
        public WindowEmployee()
        {
            InitializeComponent();
            DataContext = new PersonViewModel();
        } 
    }
}