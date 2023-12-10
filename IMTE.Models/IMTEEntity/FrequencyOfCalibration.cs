using IMTE.Models.HumanResources;
using IMTE.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.IMTEEntity
{
	public class FrequencyOfCalibration : StandardModel
	{
        public Employee ModifiedByEmployee { get; set; }
        public string Title { get; set; }
        public string Frequency { get; set; }
        public string Description { get; set; }

    }
}
