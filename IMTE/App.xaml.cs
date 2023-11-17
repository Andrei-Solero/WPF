using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace IMTE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cWWJCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH9feHRVQmVdVkZwWEQ=");
            base.OnStartup(e);

            var bs = new Bootstrapper();
            bs.Run();
        }
    }
}
