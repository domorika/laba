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
        private PersonDPO selectedPersonDPO;
         PersonDPO SelectedPersonDPO
        {
            get { return SelectedPersonDPO; }
            set
            {
                selectedPersonDPO = value;
                OnPropertyChanged("SelectedPersonDPO");
            }
        }

        public ObservableCollection<Person> ListPerson { get; set; } = new ObservableCollection<Person>();

        public ObservableCollection<PersonDPO> ListPersonDPO { get; set; } = new ObservableCollection<PersonDPO>();
        
        public PersonViewModel()
        {
                this.ListPerson.Add(
                new Person
                {
                    Id = 1,
                    RoleId = 1,
                    FirstName = "Иван",
                    LastName = "Иванов",
                    Birthday = new DateTime(1980, 02, 28)
                });
                this.ListPerson.Add(
                new Person
                {
                    Id = 2,
                    RoleId = 2,
                    FirstName = "Петр",
                    LastName = "Петров",
                    Birthday = new DateTime(1981, 03, 20)
                });

                this.ListPerson.Add(
                new Person
                {
                    Id = 3,
                    RoleId = 3,
                    FirstName = "Сидр",
                    LastName = "Сидоров",
                    Birthday = new DateTime(1983, 07, 13)
                });

                this.ListPerson.Add(
                new Person
                {
                    Id = 4,
                    RoleId = 4,
                    FirstName = "Виктор",
                    LastName = "Викторов",
                    Birthday = new DateTime(1987, 08, 03)
                });

            ListPersonDPO = GetListPersonDPO();
        }

        public ObservableCollection<PersonDPO> GetListPersonDPO()
        {
            foreach (var person in ListPerson)
            {
                PersonDPO p = new PersonDPO();
                p = p.CopyFromPerson(person);
                ListPersonDPO.Add(p);
            }
            return ListPersonDPO;
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListPerson)
            {
                if (max < r.Id)
                { max = r.Id; };
            }
            return max;
        }

        #region AddPerson
        private RelayCommand addPerson;
        public RelayCommand AddPerson
        {
            get
            {
                return addPerson ?? (addPerson = new RelayCommand(obj =>
            {
                WindowNewEmployee wnPerson = new WindowNewEmployee
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

                if (wnPerson.ShowDialog() == true)
                {
                    // Получаем выбранную должность
                    Role r = (Role)wnPerson.CbRole.SelectedValue;
                    per.RoleName = r.NameRole;
                    ListPersonDPO.Add(per);
                    Person p = new Person();
                    p = p.CopyFromPersonDPO(per);
                    ListPerson.Add(p);
                }
            },
            (obj) => true));
            }
        }
        #endregion

        #region EditPerson
        private RelayCommand editPerson;
        public RelayCommand EditPerson
        {
            get {
                return editPerson ?? (editPerson = new RelayCommand(obj =>
                {
                    WindowNewEmployee wnPerson = new WindowNewEmployee()
                    {
                        Title = "Редактирование данных сотрудника",
                    };

                    PersonDPO personDPO = SelectedPersonDPO;
                    PersonDPO tempPerson = new PersonDPO();
                    tempPerson = personDPO.ShallowCopy();
                    wnPerson.DataContext = tempPerson;

                    if (wnPerson.ShowDialog() == true)
                    {
                        Role r = (Role)wnPerson.CbRole.SelectedValue;
                        personDPO.NameRole = r.NameRole;
                        personDPO.FirstName = tempPerson.FirstName;
                        personDPO.LastName = tempPerson.LastName;
                        personDPO.Birthday = tempPerson.Birthday;

                        FindPerson finder = new FindPerson(personDPO.Id);
                        List<Person> listPerson = ListPerson.ToList();
                        Person p = listPerson.Find(new Predicate<Person>(finder.PersonPredicate));
                        p = p.CopyFromPersonDPO(personDPO);
                    }
                }, (obj => SelectedPersonDPO != null && ListPersonDPO.Count > 0));
            }
        }
        #endregion

        #region DeletePerson
        private RelayCommand deletePerson;
        public RelayCommand DeletePerson
        {
            get
            {
                return deletePerson ?? (deletePerson = new RelayCommand (obj =>
                {
                    PersonDPO person = SelectedPersonDPO;
                    MessageBoxResult result = MessageBox.Show("Удалить данные по сотруднику: \n" + person.LastName + " " + person.FirstName,
                        "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.OK)
                    {

                    }
                }))
            }
        }
        





        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}