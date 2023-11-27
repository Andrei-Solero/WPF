using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	public class MD_ToolLookupViewModel : BindableBase
	{
        private string _toolTitle;
        public string ToolTitle
        {
            get { return _toolTitle; }
            set { SetProperty(ref _toolTitle,  value); }
        }
    }
}
