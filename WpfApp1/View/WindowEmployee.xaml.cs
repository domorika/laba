using System.Windows;
using System.Windows.Data;
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    public partial class WindowEmployee : Window
    {
        private PersonViewModel _viewModel;

        public WindowEmployee()
        {
            InitializeComponent();
            _viewModel = new PersonViewModel();
            DataContext = _viewModel;
        } 
    }
}