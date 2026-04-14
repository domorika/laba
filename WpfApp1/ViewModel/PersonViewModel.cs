using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using WpfApp1.Helper;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private PersonDPO _selectedPersonDpo;
        public PersonDPO SelectedPersonDpo
        {
            get => _selectedPersonDpo;
            set
            {
                _selectedPersonDpo = value;
                OnPropertyChanged();
                // Обновляем доступность команд
                (EditPerson as RelayCommand)?.CanExecuteChanged?.Invoke(null, EventArgs.Empty);
                (DeletePerson as RelayCommand)?.CanExecuteChanged?.Invoke(null, EventArgs.Empty);
            }
        }

        public ObservableCollection<Person> ListPerson { get; set; } = new ObservableCollection<Person>();
        public ObservableCollection<PersonDPO> ListPersonDpo { get; set; } = new ObservableCollection<PersonDPO>();

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
                Role r = (Role)wnPerson.CbRole.SelectedItem;
                if (r != null)
                    per.Role = r.NameRole;

                ListPersonDpo.Add(per);

                Person p = new Person();
                p.CopyFromPersonDPO(per);
                ListPerson.Add(p);
            }
        }));
        #endregion

        #region EditPerson Command
        private RelayCommand _editPerson;
        public RelayCommand EditPerson => _editPerson ?? (_editPerson = new RelayCommand(obj =>
        {
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

            if (wnPerson.ShowDialog() == true)
            {
                Role r = (Role)wnPerson.CbRole.SelectedItem;
                if (r != null)
                    personDpo.Role = r.NameRole;
                personDpo.FirstName = tempPerson.FirstName;
                personDpo.LastName = tempPerson.LastName;
                personDpo.Birthday = tempPerson.Birthday;

                // Обновляем Person
                Person p = ListPerson.FirstOrDefault(pers => pers.Id == personDpo.Id);
                if (p != null)
                    p.CopyFromPersonDPO(personDpo);

                // Обновляем отображение
                var index = ListPersonDpo.IndexOf(personDpo);
                ListPersonDpo[index] = personDpo;
            }
        }, obj => SelectedPersonDpo != null && ListPersonDpo.Count > 0));
        #endregion

        #region DeletePerson Command
        private RelayCommand _deletePerson;
        public RelayCommand DeletePerson => _deletePerson ?? (_deletePerson = new RelayCommand(obj =>
        {
            PersonDPO person = SelectedPersonDpo;
            MessageBoxResult result = MessageBox.Show(
                $"Удалить данные по сотруднику: \n{person.LastName} {person.FirstName}",
                "Предупреждение",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                ListPersonDpo.Remove(person);
                Person per = ListPerson.FirstOrDefault(p => p.Id == person.Id);
                if (per != null)
                    ListPerson.Remove(per);
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