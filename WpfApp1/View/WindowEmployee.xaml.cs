using System.Windows;
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