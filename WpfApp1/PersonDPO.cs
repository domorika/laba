using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    internal class PersonDPO
    {
        public int Id { get; set; }
        public string Role { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public PersonDPO() { }
        public PersonDPO(int id, string role, string firstName, string lastName, DateTime birthday)
        {
            this.Id = id;
            this.Role = role;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Birthday = birthday;
        }

        public PersonDPO ShallowCopy()
        {
            return (PersonDPO)this.MemberwiseClone();
        }

        public PersonDPO CopyFromPerson(Model.Person person)
        {
            ViewModel.RoleViewModel vmRole = new ViewModel.RoleViewModel();
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
                this.Id = person.Id;
                this.Role = role;
                this.FirstName = person.FirstName;
                this.LastName = person.LastName;
                this.Birthday = person.Birthday;
            }
            return this;
        }
    }
}
