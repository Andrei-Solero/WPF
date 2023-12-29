using IMTE.IMTEEntity.Models;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.IMTEEntity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels.FieldBindings
{
    public class MeasuringDeviceCertificatesFieldBinding : BindableBase
    {
        public void SetFieldBinding(MeasuringDeviceCertificates measuringDeviceCertificatesObj)
        {
            MeasuringDevice = measuringDeviceCertificatesObj.MeasuringDevice;
            ModifiedByEmployee = measuringDeviceCertificatesObj.ModifiedByEmployee;
            Company = measuringDeviceCertificatesObj.Company;
            CalibrationCerticate = measuringDeviceCertificatesObj.CalibrationCerticate;
            CalibratedByEmployee = measuringDeviceCertificatesObj.CalibratedByEmployee;
            CalibrationMethod = measuringDeviceCertificatesObj.CalibrationMethod;
            CalibratedOn = measuringDeviceCertificatesObj.CalibratedOn;
            UsageNoOnCalibration = measuringDeviceCertificatesObj.UsageNoOnCalibration;
            NextCalibrationDate = measuringDeviceCertificatesObj.NextCalibrationDate;
            Remarks = measuringDeviceCertificatesObj.Remarks;
            AcceptanceCriteria = measuringDeviceCertificatesObj.AcceptanceCriteria;
            CalibrationResult = measuringDeviceCertificatesObj.CalibrationResult;
        }


        private MeasuringDevice _measuringDevice = new MeasuringDevice();
        public MeasuringDevice MeasuringDevice
        {
            get { return _measuringDevice; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _measuringDevice, value);
                    MeasuringDeviceCertificates.MeasuringDevice = value;
                }
            }
        }

        private Employee _modifiedByEmployee = new Employee();
        public Employee ModifiedByEmployee
        {
            get { return _modifiedByEmployee; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _modifiedByEmployee, value);
                    MeasuringDeviceCertificates.ModifiedByEmployee = value;
                }
            }
        }

        private Company _company = new Company();
        public Company Company
        {
            get { return _company; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _company, value);
                    MeasuringDeviceCertificates.Company = value;
                }
            }
        }

        private byte[] _calibrationCerticate;
        public byte[] CalibrationCerticate
        {
            get { return _calibrationCerticate; }
            set
            {
                SetProperty(ref _calibrationCerticate, value);
                MeasuringDeviceCertificates.CalibrationCerticate = value;
            }
        }

        private Employee _calibratedByEmployee = new Employee();
        public Employee CalibratedByEmployee
        {
            get { return _calibratedByEmployee; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref _calibratedByEmployee, value);
                    MeasuringDeviceCertificates.CalibratedByEmployee = value;
                }
            }
        }

        private string _calibrationMethod;
        public string CalibrationMethod
        {
            get { return _calibrationMethod; }
            set
            {
                SetProperty(ref _calibrationMethod, value);
                MeasuringDeviceCertificates.CalibrationMethod = value;
            }
        }

        private DateTime? _calibratedOn = DateTime.Now;
        public DateTime? CalibratedOn
        {
            get { return _calibratedOn; }
            set
            {
                SetProperty(ref _calibratedOn, value);
                MeasuringDeviceCertificates.CalibratedOn = value;
            }
        }

        private int? _usageNoOnCalibration;
        public int? UsageNoOnCalibration
        {
            get { return _usageNoOnCalibration; }
            set
            {
                SetProperty(ref _usageNoOnCalibration, value);
                MeasuringDeviceCertificates.UsageNoOnCalibration = value;
            }
        }

        private DateTime? _nextCalibrationDate = DateTime.Now;
        public DateTime? NextCalibrationDate
        {
            get { return _nextCalibrationDate; }
            set
            {
                SetProperty(ref _nextCalibrationDate, value);
                MeasuringDeviceCertificates.NextCalibrationDate = value;
            }
        }

        private string _remarks;
        public string Remarks
        {
            get { return _remarks; }
            set
            {
                SetProperty(ref _remarks, value);
                MeasuringDeviceCertificates.Remarks = value;
            }
        }

        private string _acceptanceCriteria;
        public string AcceptanceCriteria
        {
            get { return _acceptanceCriteria; }
            set
            {
                SetProperty(ref _acceptanceCriteria, value);
                MeasuringDeviceCertificates.AcceptanceCriteria = value;
            }
        }

        private string _calibrationResult;
        public string CalibrationResult
        {
            get { return _calibrationResult; }
            set
            {
                SetProperty(ref _calibrationResult, value);
                MeasuringDeviceCertificates.CalibrationResult = value;
            }
        }


        #region Full Property

        private MeasuringDeviceCertificates _measuringDeviceCertificate = new MeasuringDeviceCertificates();
        public MeasuringDeviceCertificates MeasuringDeviceCertificates
        {
            get { return _measuringDeviceCertificate; }
            set { SetProperty(ref _measuringDeviceCertificate, value); }
        }


        #endregion

    }
}
