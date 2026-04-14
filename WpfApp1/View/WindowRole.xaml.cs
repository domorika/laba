using System.Windows;
using WpfApp1.Model;
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    public partial class WindowRole : Window
    {
        private RoleViewModel vmRole;

        public WindowRole()
        {
            InitializeComponent();
            vmRole = new RoleViewModel();
            lvrole.ItemsSource = vmRole.ListRoles;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            WindowNewRole wnRole = new WindowNewRole();
            wnRole.Title = "Новая должность";
            wnRole.Owner = this;

            int maxIdRole = vmRole.MaxId() + 1;
            Role role = new Role
            {
                Id = maxIdRole
            };

            wnRole.DataContext = role;
            if (wnRole.ShowDialog() == true)
            {
                vmRole.ListRoles.Add(role);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            WindowNewRole wnRole = new WindowNewRole
            {
                Title = "Редактирование должности",
                Owner = this,
            };
            Role role = lvrole.SelectedItem as Role;
            if (role != null)
            {
                Role tempRole = role.ShallowCopy();
                wnRole.DataContext = tempRole;
                if (wnRole.ShowDialog() == true)
                {
                    role.NameRole = tempRole.NameRole;
                    lvrole.ItemsSource = null;
                    lvrole.ItemsSource = vmRole.ListRoles;
                }
            }
            else
            {
                MessageBox.Show("Необходимо выбрать для редактирования",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            Role role = (Role)lvrole.SelectedItem;
            if (role != null)
            {
                MessageBoxResult result = MessageBox.Show("Удалить все данные по должности: " +
                                        role.NameRole, "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    vmRole.ListRoles.Remove(role);
                }
            }
            else
            {
                MessageBox.Show("Необходимо выбрать должность для удаления", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}