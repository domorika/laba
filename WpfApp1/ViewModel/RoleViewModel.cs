using System.Collections.ObjectModel;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    class RoleViewModel
    {
        public ObservableCollection<Role> ListRoles { get; set; } = new ObservableCollection<Role>();

        public RoleViewModel()
        {
            this.ListRoles.Add(new Role
            {
                Id = 1,
                NameRole = "Диктатор"
            });

            this.ListRoles.Add(new Role
            {
                Id = 2,
                NameRole = "Бухгалтер"
            });

            this.ListRoles.Add(new Role
            {
                Id = 3,
                NameRole = "Менеджер"
            });

            this.ListRoles.Add(new Role
            {
                Id = 4,
                NameRole = "Программист"
            });
        }

        public int MaxId()
        {
            int max = 0;
            foreach (var r in this.ListRoles)
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