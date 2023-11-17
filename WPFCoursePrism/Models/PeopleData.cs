using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFCoursePrism.Models
{
    public static class PeopleData
    {
        public static ObservableCollection<Person> People()
        {
            var output = new ObservableCollection<Person>();

            output.Add(new Person
            {
                FirstName = "Andrei",
                LastName = "Solero",
                Email = "solerosolero@gmail.com"
            });

            output.Add(new Person
            {
                FirstName = "Kaeceline",
                LastName = "Solero",
                Email = "chisolero@gmail.com"
            });

            output.Add(new Person
            {
                FirstName = "Francis",
                LastName = "Solero",
                Email = "francissolero@gmail.com"
            });

            return output;
        }
    }
}
