using System.Windows;
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    public partial class WindowRole : Window
    {
        public WindowRole()
        {
            InitializeComponent();
            DataContext = new RoleViewModel();
        }
    }
}