using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Helper;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class RoleViewModel : INotifyPropertyChanged
    {
        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set
            {
                _selectedRole = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public ObservableCollection<Role> ListRoles { get; set; } = new ObservableCollection<Role>();

        public RoleViewModel()
        {
            // Инициализация тестовых данных
            ListRoles.Add(new Role { Id = 1, NameRole = "Директор" });
            ListRoles.Add(new Role { Id = 2, NameRole = "Бухгалтер" });
            ListRoles.Add(new Role { Id = 3, NameRole = "Менеджер" });
            ListRoles.Add(new Role { Id = 4, NameRole = "Программист" });
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in ListRoles)
            {
                if (max < r.Id)
                    max = r.Id;
            }
            return max;
        }

        #region AddRole Command
        private RelayCommand _addRole;
        public RelayCommand AddRole => _addRole ?? (_addRole = new RelayCommand(obj =>
        {
            var wnRole = new WindowNewRole
            {
                Title = "Новая должность"
            };

            int maxIdRole = MaxId() + 1;
            Role role = new Role { Id = maxIdRole };
            wnRole.DataContext = role;

            if (wnRole.ShowDialog() == true)
            {
                ListRoles.Add(role);
                SelectedRole = role;
            }
        }));
        #endregion

        #region EditRole Command
        private RelayCommand _editRole;
        public RelayCommand EditRole => _editRole ?? (_editRole = new RelayCommand(obj =>
        {
            var wnRole = new WindowNewRole
            {
                Title = "Редактирование должности"
            };

            Role role = SelectedRole;
            Role tempRole = role.ShallowCopy();
            wnRole.DataContext = tempRole;

            if (wnRole.ShowDialog() == true)
            {
                role.NameRole = tempRole.NameRole;
                // Обновляем отображение
                var index = ListRoles.IndexOf(role);
                ListRoles[index] = role;
            }
        }, obj => SelectedRole != null && ListRoles.Count > 0));
        #endregion

        #region DeleteRole Command
        private RelayCommand _deleteRole;
        public RelayCommand DeleteRole => _deleteRole ?? (_deleteRole = new RelayCommand(obj =>
        {
            Role role = SelectedRole;
            MessageBoxResult result = MessageBox.Show(
                $"Удалить данные по должности: {role.NameRole}",
                "Предупреждение",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                ListRoles.Remove(role);
            }
        }, obj => SelectedRole != null && ListRoles.Count > 0));
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}