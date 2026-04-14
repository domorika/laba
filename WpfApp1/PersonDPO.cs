using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp1.Model;
using WpfApp1.ViewModel;

namespace WpfApp1
{
    public class PersonDPO : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _role;
        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged();
            }
        }

        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private DateTime _birthday;
        public DateTime Birthday
        {
            get => _birthday;
            set
            {
                _birthday = value;
                OnPropertyChanged();
            }
        }

        public PersonDPO() { }

        public PersonDPO(int id, string role, string firstName, string lastName, DateTime birthday)
        {
            Id = id;
            Role = role;
            FirstName = firstName;
            LastName = lastName;
            Birthday = birthday;
        }

        public PersonDPO ShallowCopy()
        {
            return (PersonDPO)MemberwiseClone();
        }

        public PersonDPO CopyFromPerson(Person person)
        {
            RoleViewModel vmRole = new RoleViewModel();
            string role = string.Empty;
            foreach (var r in vmRole.ListRoles)
            {
                if (r.Id == person.RoleId)
                {
                    role = r.NameRole;
                    break;
                }
            }
            if (role != string.Empty)
            {
                Id = person.Id;
                Role = role;
                FirstName = person.FirstName;
                LastName = person.LastName;
                Birthday = person.Birthday;
            }
            return this;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}