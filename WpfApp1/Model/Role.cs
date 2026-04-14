using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp1.Model
{
    public class Role : INotifyPropertyChanged
    {
        public int Id { get; set; }

        private string _nameRole;
        public string NameRole
        {
            get => _nameRole;
            set
            {
                _nameRole = value;
                OnPropertyChanged();
            }
        }

        public Role() { }

        public Role(int id, string nameRole)
        {
            Id = id;
            NameRole = nameRole;
        }

        public Role ShallowCopy()
        {
            return (Role)MemberwiseClone();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}