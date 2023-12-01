using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.Models.Definition;
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
    public class MDLookup_UnitConfigListViewModel : BindableBase
    {
        private readonly UnitDA unitDA;
        private readonly IEventAggregator ea;

        public DelegateCommand PassSelectedObjToMDFormCommand { get; private set; }

        public MDLookup_UnitConfigListViewModel(IEventAggregator ea)
        {
            unitDA = new UnitDA();
            this.ea = ea;

            Units = new ObservableCollection<UnitEntity>(unitDA.GetAllUnit());

            PassSelectedObjToMDFormCommand = new DelegateCommand(PassSelectedObjToMDForm);
        }

        private void PassSelectedObjToMDForm()
        {
            ea.GetEvent<UnitToMDForm>().Publish(SelectedUnit);
         }


        #region Full Properties

        private UnitEntity _selectedUnit = new UnitEntity();
        public UnitEntity SelectedUnit
        {
            get { return _selectedUnit; }
            set { SetProperty(ref _selectedUnit, value); }
        }


        #endregion

        #region Observable Collections

        private ObservableCollection<UnitEntity> _units;
        public ObservableCollection<UnitEntity> Units
        {
            get { return _units; }
            set { SetProperty(ref _units, value); }
        }


        #endregion

    }
}
