using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WpfApp1.Model;
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    public partial class WindowEmployee : Window
    {
        private PersonViewModel vmPerson;
        private RoleViewModel vmRole;
        private ObservableCollection<PersonDPO> personsDPO;
        private System.Collections.Generic.List<Role> roles;

        public WindowEmployee()
        {
            InitializeComponent();
            vmPerson = new PersonViewModel();
            vmRole = new RoleViewModel();
            roles = vmRole.ListRoles.ToList();
            personsDPO = new ObservableCollection<PersonDPO>();

            foreach (var person in vmPerson.ListPerson)
            {
                PersonDPO p = new PersonDPO();
                p.CopyFromPerson(person);  // Исправлено: вызываем метод у созданного объекта
                personsDPO.Add(p);
            }
            lvEmployee.ItemsSource = personsDPO;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            WindowNewEmployee wnEmployee = new WindowNewEmployee
            {
                Title = "Новый сотрудник",
                Owner = this
            };

            int maxIdPerson = vmPerson.MaxId() + 1;
            PersonDPO per = new PersonDPO
            {
                Id = maxIdPerson,
                Birthday = System.DateTime.Now
            };

            wnEmployee.DataContext = per;
            wnEmployee.CbRole.ItemsSource = roles;

            if (wnEmployee.ShowDialog() == true)
            {
                Role r = (Role)wnEmployee.CbRole.SelectedItem;
                if (r != null)
                {
                    per.Role = r.NameRole;
                }
                per.FirstName = wnEmployee.TbFirstName.Text;
                per.LastName = wnEmployee.TbLastName.Text;
                personsDPO.Add(per);

                Person p = new Person();
                p.CopyFromPersonDPO(per);  // Исправлено: вызываем метод у объекта p
                vmPerson.ListPerson.Add(p);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            WindowNewEmployee wnEmployee = new WindowNewEmployee
            {
                Title = "Редактирование данных",
                Owner = this
            };

            PersonDPO perDPO = (PersonDPO)lvEmployee.SelectedItem;

            if (perDPO != null)
            {
                PersonDPO tempPerDPO = perDPO.ShallowCopy();
                wnEmployee.DataContext = tempPerDPO;
                wnEmployee.CbRole.ItemsSource = roles;
                wnEmployee.CbRole.Text = tempPerDPO.Role;
                wnEmployee.TbFirstName.Text = tempPerDPO.FirstName;
                wnEmployee.TbLastName.Text = tempPerDPO.LastName;
                wnEmployee.ClBirthday.SelectedDate = tempPerDPO.Birthday;

                if (wnEmployee.ShowDialog() == true)
                {
                    Role r = (Role)wnEmployee.CbRole.SelectedItem;
                    if (r != null)
                    {
                        perDPO.Role = r.NameRole;
                    }
                    perDPO.FirstName = wnEmployee.TbFirstName.Text;
                    perDPO.LastName = wnEmployee.TbLastName.Text;
                    if (wnEmployee.ClBirthday.SelectedDate.HasValue)
                    {
                        perDPO.Birthday = wnEmployee.ClBirthday.SelectedDate.Value;
                    }

                    lvEmployee.ItemsSource = null;
                    lvEmployee.ItemsSource = personsDPO;

                    // Поиск и обновление Person
                    Person pToUpdate = vmPerson.ListPerson.FirstOrDefault(p => p.Id == perDPO.Id);
                    if (pToUpdate != null)
                    {
                        pToUpdate.CopyFromPersonDPO(perDPO);
                    }
                }
            }
            else
            {
                MessageBox.Show("Необходимо выбрать сотрудника для редактирования",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            PersonDPO person = (PersonDPO)lvEmployee.SelectedItem;
            if (person != null)
            {
                MessageBoxResult result = MessageBox.Show("Удалить данные по сотруднику: \n" +
                    person.LastName + " " + person.FirstName,
                    "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Warning);
                if (result == MessageBoxResult.OK)
                {
                    personsDPO.Remove(person);

                    Person per = vmPerson.ListPerson.FirstOrDefault(p => p.Id == person.Id);
                    if (per != null)
                    {
                        vmPerson.ListPerson.Remove(per);
                    }
                }
            }
            else
            {
                MessageBox.Show("Необходимо выбрать данные по сотруднику для удаления",
                    "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}