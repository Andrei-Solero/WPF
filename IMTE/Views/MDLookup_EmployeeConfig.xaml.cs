﻿using IMTE.ViewModels;
using Prism.Events;
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
using Unity;

namespace IMTE.Views
{
    /// <summary>
    /// Interaction logic for EmployeeConfigLookup.xaml
    /// </summary>
    public partial class MDLookup_EmployeeConfig : UserControl
    {
        public MDLookup_EmployeeConfig()
        {
            InitializeComponent();

            var regionManager = new RegionManager();
            RegionManager.SetRegionManager(this, regionManager);

            DataContext = new MDLookup_EmployeeConfigViewModel(regionManager);
        }
    }
}
