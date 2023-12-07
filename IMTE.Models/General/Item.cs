using IMTE.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.General
{
    public class Item : StandardModel
    {
        public int ItemCategoryId { get; set; }
        public Description Description { get; set; }
        public string ItemCode { get; set; }
        public string ShortDescription { get; set; }
    }
}
