using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.Helper
{
    internal class FindPerson
    {
        private int _id;

        public FindPerson(int id)
        {
            _id = id;
        }

        public bool PersonPredicate(Person p)
        {
            return p.Id == _id;
        }
    }
}