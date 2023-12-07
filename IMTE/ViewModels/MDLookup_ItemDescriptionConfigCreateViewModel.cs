using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.Models.General;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IMTE.ViewModels
{
    public class MDLookup_ItemDescriptionConfigCreateViewModel : BindableBase
    {
        private readonly DescriptionDA descriptionDA;
        private readonly IEventAggregator ea;

        public DelegateCommand SaveDescriptionCommand { get; set; }

        public MDLookup_ItemDescriptionConfigCreateViewModel(IEventAggregator ea)
        {
            descriptionDA = new DescriptionDA();

            SaveDescriptionCommand = new DelegateCommand(SaveDescription);
            this.ea = ea;
        }

        private void SaveDescription()
        {
            descriptionDA.CreateDescription(Description);

            MessageBox.Show("Item Description Saved!");

            ea.GetEvent<DescriptionLookupToMDForms>().Publish(Description);
        }


        #region Full Properties

        private Description _description = new Description();
        public Description Description
        {
            get { return _description = new Description(); }
            set { SetProperty(ref _description, value); }
        }


        #endregion

    }
}
