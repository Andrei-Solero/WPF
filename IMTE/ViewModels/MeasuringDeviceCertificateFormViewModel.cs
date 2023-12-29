using IMTE.DataAccess;
using IMTE.ViewModels.FieldBindings;
using Microsoft.Win32;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMTE.EventAggregator.Core.EventAggregators;
using Prism.Events;
using Prism.Services.Dialogs;
using IMTE.EventAggregator.Core;
using IMTE.Models.HumanResources;
using System.Collections.ObjectModel;
using IMTE.IMTEEntity.Models;

namespace IMTE.ViewModels
{
    public class MeasuringDeviceCertificateFormViewModel : MeasuringDeviceCertificatesFieldBindingDialog
    {
        private readonly MeasuringDeviceCertificateDA measuringDeviceCertificateDA;
        private readonly EmployeeDA employeeDA;
        private readonly IEventAggregator ea;
        private readonly IDialogService dialogService;

        public DelegateCommand BrowseCertificateCommand { get; private set; }
        public DelegateCommand SaveMeasuringDeviceCertificateCommand { get; private set; }
        public DelegateCommand EmployeeConfigLookupCommand { get; private set; }

        public MeasuringDeviceCertificateFormViewModel(IEventAggregator ea, IDialogService dialogService)
        {
            measuringDeviceCertificateDA = new MeasuringDeviceCertificateDA();
            employeeDA = new EmployeeDA();

            BrowseCertificateCommand = new DelegateCommand(BrowseCertificate);
            SaveMeasuringDeviceCertificateCommand = new DelegateCommand(SaveMeasuringDeviceCertificate);
            EmployeeConfigLookupCommand = new DelegateCommand(EmployeeConfigLookup);
            this.ea = ea;
            this.dialogService = dialogService;

            ea.GetEvent<EmployeeLookupToMDForm>().Subscribe(SetEmployeeData);
            ea.GetEvent<SaveMeasuringDeviceCertificateDirectlyToForm>().Subscribe(SaveDirectlyToForm);

            LoadDataToForm();
        }

        public override void OnDialogOpened(IDialogParameters parameters)
        {
            MeasuringDevice = parameters.GetValue<MeasuringDevice>("MeasuringDeviceObj");
        }

        private async void LoadDataToForm()
        {
            Employees = new ObservableCollection<Employee>(await employeeDA.GetEmployees());
        }

        private void SetEmployeeData(Employee obj)
        {
            CalibratedByEmployee = obj;
        }

        private void EmployeeConfigLookup()
        {
            dialogService.ShowDialog("EmpConfig");
        }

        private async void SaveMeasuringDeviceCertificate()
        {
            if (MeasuringDevice.Id != null && MeasuringDevice.Id != 0)
            {
                await measuringDeviceCertificateDA.CreateMeasuringDeviceCertificateAsync(MeasuringDeviceCertificates);
            }
            else
            {
                ea.GetEvent<SaveMeasuringDeviceCertificate>().Publish(true);
                ea.GetEvent<MeasuringDeviceCertificatesData>().Publish(MeasuringDeviceCertificates);
            }
        }

        private bool _willSaveDirectlyToForm;
        public bool WillSaveDirectlyToForm
        {
            get { return _willSaveDirectlyToForm; }
            set { SetProperty(ref _willSaveDirectlyToForm, value); }
        }


        private void SaveDirectlyToForm(bool obj)
        {
            WillSaveDirectlyToForm = obj;
        }

        private void BrowseCertificate()
        {
            string filePath = "";
            OpenFileDialog opDialog = new OpenFileDialog
            {
                Filter = "Word (*.docx)|*.docx|PDF Files (*.pdf)|*.pdf"
            };

            if (opDialog.ShowDialog() == true)
                filePath = opDialog.FileName;

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    CalibrationCerticate = reader.ReadBytes((int)stream.Length);
                }
            }
        }


        #region Observable Collections

        private ObservableCollection<Employee> _employees;
        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { SetProperty(ref _employees, value); }
        }


        #endregion

    }
}
