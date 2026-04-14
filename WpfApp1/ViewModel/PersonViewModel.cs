using System.Collections.ObjectModel;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    internal class PersonViewModel
    {
        public ObservableCollection<Person> ListPerson { get; set; }
        = new ObservableCollection<Person>();
        public PersonViewModel ()
        {
                this.ListPerson.Add(
                new Person
                {
                    Id = 1,
                    RoleId = 1,
                    FirstName = "Иванька",
                    LastName = "Иваньчиков",
                    Birthday = new DateTime(1980, 02, 28)
                });

                this.ListPerson.Add(
                new Person
                {
                    Id = 2,
                    RoleId = 2,
                    FirstName = "Петруня",
                    LastName = "Петруньков",
                    Birthday = new DateTime(1981, 03, 20)
                });

                this.ListPerson.Add(
               new Person
               {
                   Id = 3,
                   RoleId = 3,
                   FirstName = "Витя",
                   LastName = "Витяшеньков",
                   Birthday = new DateTime(1982, 04, 15)
               });

                this.ListPerson.Add(
               new Person
               {
                   Id = 4,
                   RoleId = 4,
                   FirstName = "Сидр",
                   LastName = "Яблочков",
                   Birthday = new DateTime(1983, 05, 10)
               });

        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListPerson)
            {
                if (max < r.Id)
                {
                    max = r.Id;
                }
            } 
            return max;
        }
    }
}
