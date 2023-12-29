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
using System.Windows;

namespace IMTE.ViewModels
{
    public class MDLookup_PlantConfigCreateViewModel : BindableBase
    {
        private readonly LocationDA locationDA;
        private readonly PlantDA plantDA;
        private readonly IEventAggregator ea;

        public DelegateCommand CreatePlantCommand { get; private set; }

        public MDLookup_PlantConfigCreateViewModel(IEventAggregator ea)
        {
            locationDA = new LocationDA();
            plantDA = new PlantDA();

            CreatePlantCommand = new DelegateCommand(CreatePlant);

            Task.Run(async () => await LoadDataToFormAsync());
            this.ea = ea;
        }

        private async void CreatePlant()
        {
            try
            {
                await Task.Run(async () => await ExecuteCreatePlantDB());
                ea.GetEvent<PlantLookupToMDForm>().Publish(Plant);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occured {ex.Message}");
            }
        }

        private async Task ExecuteCreatePlantDB()
        {
            Plant = await plantDA.CreatePlantAsync(Plant);
        }

        private async Task LoadDataToFormAsync()
        {
            Locations = new ObservableCollection<Location>(await locationDA.GetAllLocations());
        }

        #region FIELD BINDING

        private string _plantName;
        public string PlantName
        {
            get { return _plantName; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _plantName, value);
                    Plant.PlantName = value;
                }
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _description, value);
                    Plant.Description = value;
                }
            }
        }

        private Location _location = new Location();
        public Location Location
        {
            get { return _location; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _location, value);
                    Plant.Location = value;
                }
            }
        }



        #endregion

        #region Observable Collections

        private ObservableCollection<Location> _locations;
        public ObservableCollection<Location> Locations
        {
            get { return _locations; }
            set { SetProperty(ref _locations, value); }
        }


        #endregion

        #region Full Property

        private Plant _plant = new Plant();
        public Plant Plant
        {
            get { return _plant; }
            set { SetProperty(ref _plant, value); }
        }


        #endregion

    }
}
