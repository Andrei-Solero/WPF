using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestComboBoxBinding.Models
{
    public class Fruits
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Taste { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
