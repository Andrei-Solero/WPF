using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.General
{
    public class Item
    {
        public int Id { get; set; }
        public int Version { get; set; }
        public int ItemCategoryId { get; set; }
        public Description Description { get; set; }
        public string ItemCode { get; set; }
        [Required(ErrorMessage = "Short Description is required.")]
        public string ShortDescription { get; set; }
    }
}
