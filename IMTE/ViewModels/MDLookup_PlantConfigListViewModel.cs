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
    public class MDLookup_PlantConfigListViewModel : BindableBase
    {
        private readonly PlantDA plantDA;
        private readonly IEventAggregator ea;

        public DelegateCommand PassSelectedObjToMDFormCommand { get; private set; }

        public MDLookup_PlantConfigListViewModel(IEventAggregator ea)
        {
            plantDA = new PlantDA();

            Plants = new ObservableCollection<Plant>(plantDA.GetAllPlant());
            this.ea = ea;

            PassSelectedObjToMDFormCommand = new DelegateCommand(PassSelectedObjToMDForm);

        }

        private void PassSelectedObjToMDForm()
        {
            ea.GetEvent<PlantLookupToMDForm>().Publish(SelectedPlant);
        }

        #region Full Property

        private Plant _selectedPlant = new Plant();
        public Plant SelectedPlant
        {
            get { return _selectedPlant; }
            set { SetProperty(ref _selectedPlant, value); }
        }


        #endregion

        #region Observable Collections

        private ObservableCollection<Plant> _plants;
        public ObservableCollection<Plant> Plants
        {
            get { return _plants; }
            set { SetProperty(ref _plants, value); }
        }

        #endregion
    }
}
