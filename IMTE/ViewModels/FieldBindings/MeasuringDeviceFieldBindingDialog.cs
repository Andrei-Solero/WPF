using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels.FieldBindings
{
    public class MeasuringDeviceFieldBindingDialog : MeasuringDeviceFieldBinding, IDialogAware
    {
		public virtual string Title => "";

		public event Action<IDialogResult> RequestClose;

		public virtual bool CanCloseDialog() => true;

		public virtual void OnDialogClosed()
		{
		}

		public virtual void OnDialogOpened(IDialogParameters parameters)
		{
		}
	}
}
