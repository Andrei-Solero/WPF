using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.Models.General;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MDLookup_ItemDescriptionConfigListViewModel : BindableBase
    {
        private readonly DescriptionDA descriptionDA;
        private readonly IEventAggregator ea;

        public DelegateCommand PassTextToItemDescriptionCommand { get; private set; }

        public MDLookup_ItemDescriptionConfigListViewModel(IEventAggregator ea)
        {
            descriptionDA = new DescriptionDA();

            Descriptions = new ObservableCollection<Description>(descriptionDA.GetAllDescriptions());
            this.ea = ea;

            PassTextToItemDescriptionCommand = new DelegateCommand(PassTextToItemDescription);
        }

        private void PassTextToItemDescription()
        {
            ea.GetEvent<DescriptionLookupToMDForms>().Publish(SelectedDescription);
        }


        #region Observable Collections

        private ObservableCollection<Description> _descriptions;
        public ObservableCollection<Description> Descriptions
        {
            get { return _descriptions; }
            set { SetProperty(ref _descriptions, value); }
        }

        #endregion

        #region Full Properties

        private Description _selectedDescription = new Description();
        public Description SelectedDescription
        {
            get { return _selectedDescription; }
            set { SetProperty(ref _selectedDescription, value); }
        }


        #endregion

    }
}
