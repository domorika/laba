using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Helper;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private PersonDPO _selectedPersonDpo;
        internal PersonDPO SelectedPersonDpo
        {
            get => _selectedPersonDpo;
            set
            {
                _selectedPersonDpo = value;
                OnPropertyChanged();
                CommandManager.InvalidateRequerySuggested();
            }
        }

        internal ObservableCollection<Person> ListPerson { get; set; } = new ObservableCollection<Person>();

        private ObservableCollection<PersonDPO> _listPersonDpo = new ObservableCollection<PersonDPO>();
        internal ObservableCollection<PersonDPO> ListPersonDpo
        {
            get => _listPersonDpo;
            set
            {
                _listPersonDpo = value;
                OnPropertyChanged();
            }
        }

        // Список должностей для ComboBox
        private RoleViewModel _roleVM;
        public ObservableCollection<Role> RolesList { get; set; }

        public PersonViewModel()
        {
            _roleVM = new RoleViewModel();
            RolesList = _roleVM.ListRoles;

            // Инициализация тестовых данных
            ListPerson.Add(new Person { Id = 1, RoleId = 1, FirstName = "Иван", LastName = "Иванов", Birthday = new DateTime(1980, 2, 28) });
            ListPerson.Add(new Person { Id = 2, RoleId = 2, FirstName = "Петр", LastName = "Петров", Birthday = new DateTime(1981, 3, 20) });
            ListPerson.Add(new Person { Id = 3, RoleId = 3, FirstName = "Виктор", LastName = "Викторов", Birthday = new DateTime(1982, 4, 15) });
            ListPerson.Add(new Person { Id = 4, RoleId = 4, FirstName = "Сидор", LastName = "Сидоров", Birthday = new DateTime(1983, 5, 10) });

            UpdatePersonDpoList();
        }

        private void UpdatePersonDpoList()
        {
            ListPersonDpo.Clear();
            foreach (var person in ListPerson)
            {
                PersonDPO p = new PersonDPO();
                p.CopyFromPerson(person);
                ListPersonDpo.Add(p);
            }
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in ListPerson)
            {
                if (max < r.Id)
                    max = r.Id;
            }
            return max;
        }

        #region AddPerson Command
        private RelayCommand _addPerson;
        public RelayCommand AddPerson => _addPerson ?? (_addPerson = new RelayCommand(obj =>
        {
            var wnPerson = new WindowNewEmployee
            {
                Title = "Новый сотрудник"
            };

            int maxIdPerson = MaxId() + 1;
            PersonDPO per = new PersonDPO
            {
                Id = maxIdPerson,
                Birthday = DateTime.Now
            };

            wnPerson.DataContext = per;
            wnPerson.CbRole.ItemsSource = RolesList;

            if (wnPerson.ShowDialog() == true)
            {
                // Получаем выбранную должность
                Role r = (Role)wnPerson.CbRole.SelectedItem;
                if (r != null)
                {
                    per.Role = r.NameRole;
                }

                // Получаем данные из полей
                per.FirstName = wnPerson.TbFirstName.Text;
                per.LastName = wnPerson.TbLastName.Text;
                if (wnPerson.ClBirthday.SelectedDate.HasValue)
                {
                    per.Birthday = wnPerson.ClBirthday.SelectedDate.Value;
                }

                // Добавляем в коллекцию отображения
                ListPersonDpo.Add(per);

                // Добавляем в коллекцию Person
                Person p = new Person();
                p.CopyFromPersonDPO(per);
                ListPerson.Add(p);

                // Обновляем привязку
                OnPropertyChanged(nameof(ListPersonDpo));
            }
        }));
        #endregion

        #region EditPerson Command
        private RelayCommand _editPerson;
        public RelayCommand EditPerson => _editPerson ?? (_editPerson = new RelayCommand(obj =>
        {
            if (SelectedPersonDpo == null)
            {
                MessageBox.Show("Выберите сотрудника для редактирования", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var wnPerson = new WindowNewEmployee
            {
                Title = "Редактирование данных сотрудника"
            };

            PersonDPO personDpo = SelectedPersonDpo;
            PersonDPO tempPerson = personDpo.ShallowCopy();
            wnPerson.DataContext = tempPerson;
            wnPerson.CbRole.ItemsSource = RolesList;

            // Устанавливаем выбранную должность
            var selectedRole = RolesList.FirstOrDefault(r => r.NameRole == personDpo.Role);
            if (selectedRole != null)
                wnPerson.CbRole.SelectedItem = selectedRole;

            // Устанавливаем дату рождения
            wnPerson.ClBirthday.SelectedDate = tempPerson.Birthday;

            if (wnPerson.ShowDialog() == true)
            {
                // Обновляем данные
                Role r = (Role)wnPerson.CbRole.SelectedItem;
                if (r != null)
                    personDpo.Role = r.NameRole;
                personDpo.FirstName = tempPerson.FirstName;
                personDpo.LastName = tempPerson.LastName;
                if (wnPerson.ClBirthday.SelectedDate.HasValue)
                {
                    personDpo.Birthday = wnPerson.ClBirthday.SelectedDate.Value;
                }

                // Обновляем Person
                Person p = ListPerson.FirstOrDefault(pers => pers.Id == personDpo.Id);
                if (p != null)
                {
                    p.CopyFromPersonDPO(personDpo);
                }

                // Обновляем отображение
                var index = ListPersonDpo.IndexOf(personDpo);
                if (index >= 0)
                {
                    ListPersonDpo[index] = personDpo;
                }

                OnPropertyChanged(nameof(ListPersonDpo));
            }
        }, obj => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
        #endregion

        #region DeletePerson Command
        private RelayCommand _deletePerson;
        public RelayCommand DeletePerson => _deletePerson ?? (_deletePerson = new RelayCommand(obj =>
        {
            if (SelectedPersonDpo == null)
            {
                MessageBox.Show("Выберите сотрудника для удаления", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            PersonDPO person = SelectedPersonDpo;
            MessageBoxResult result = MessageBox.Show(
                $"Удалить данные по сотруднику: \n{person.LastName} {person.FirstName}",
                "Предупреждение",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                // Удаляем из коллекции отображения
                ListPersonDpo.Remove(person);

                // Удаляем из коллекции Person
                Person per = ListPerson.FirstOrDefault(p => p.Id == person.Id);
                if (per != null)
                    ListPerson.Remove(per);

                OnPropertyChanged(nameof(ListPersonDpo));
            }
        }, obj => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}