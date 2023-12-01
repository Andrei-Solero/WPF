using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.Models.Definition;
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
    public class MDLookup_UnitConfigCreateViewModel : BindableBase
    {
        private readonly UnitDA unitDA;
        private readonly IEventAggregator ea;

        public DelegateCommand SaveUnitCommand { get; private set; }

        public MDLookup_UnitConfigCreateViewModel(IEventAggregator ea)
        {
            unitDA = new UnitDA();

            SaveUnitCommand = new DelegateCommand(SaveUnit);
            this.ea = ea;
        }

        private void SaveUnit()
        {
            unitDA.CreateUnit(Unit);
            MessageBox.Show("Unit of measurement saved!", "Saved");

            ea.GetEvent<UnitToMDForm>().Publish(Unit);
            
        }

        #region Full Property

        private UnitEntity _unit = new UnitEntity();
        public UnitEntity Unit
        {
            get { return _unit; }
            set { SetProperty(ref _unit, value); }
        }


        #endregion
    }
}
