using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.Models.General;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Syncfusion.Windows.Controls.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	public class MDLookup_ItemConfigListViewModel : BindableBase
	{
        private readonly ItemDA itemDA;
		private readonly IEventAggregator ea;

		public DelegateCommand PassSelectedObjToMDFormCommand { get; private set; }

        public MDLookup_ItemConfigListViewModel(IEventAggregator ea)
        {
			this.ea = ea;
            itemDA = new ItemDA();

			PassSelectedObjToMDFormCommand = new DelegateCommand(PassSelectedObjToMDForm);
			Items = new ObservableCollection<Item>(itemDA.GetAllItems());
		}

		private void PassSelectedObjToMDForm()
		{
            ea.GetEvent<ItemLookupToMDForm>().Publish(SelectedItemObj);
		}


		#region Observable Collections

		private ObservableCollection<Item> _items;
        public ObservableCollection<Item> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        #endregion

        #region Full Properties

        private Description _description = new Description();
        public Description Description
        {
            get { return _description; }
            set 
            { 
                SetProperty(ref _description, value);
                SelectedItemObj.Description = value;
            }
        }


        private Item _selectedItemObj = new Item();
        public Item SelectedItemObj
        {
            get { return _selectedItemObj; }
            set 
            { 
                SetProperty(ref _selectedItemObj, value);
                Description = value.Description;
            }
        }

        #endregion

    }
}
