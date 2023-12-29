using IMTE.Models.HumanResources;
using IMTE.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.IMTEEntity
{
    public class DeviceType : StandardModel
    {
        public string DeviceTypeName { get; set; }
        public Employee ModifiedByEmployeeId { get; set; }


        public override string ToString()
        {
            return DeviceTypeName;
        }

    }
}
