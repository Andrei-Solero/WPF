﻿using IMTE.ViewModels;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IMTE.Views
{
    /// <summary>
    /// Interaction logic for MDLookup_PlantConfigContainer.xaml
    /// </summary>
    public partial class MDLookup_PlantConfig : UserControl
    {
        public MDLookup_PlantConfig()
        {
            InitializeComponent();

            var regionManager = new RegionManager();

            RegionManager.SetRegionManager(this, regionManager);
            DataContext = new MDLookup_PlantConfigViewModel(regionManager);
        }
    }
}
