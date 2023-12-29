using IMTE.Models.General;
using IMTE.Models.Inventory;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels.FieldBindings
{
    public class InstrumentFieldBinding : BindableBase
    {
        public void SetFieldBindingData(InstrumentSerial instrumentSerialObj)
        {
            SerialNO = instrumentSerialObj.SerialNo;
            Manufacturer = instrumentSerialObj.Instrument.Manufacturer;
            Model = instrumentSerialObj.Instrument.Model;
            HasAccessory = instrumentSerialObj.Instrument.HasAccessory;
            ApprovalCode = instrumentSerialObj.Instrument.ApprovalCode;
            IsPrinted = instrumentSerialObj.Instrument.IsPrinted;
            IsForeignCurrency = instrumentSerialObj.Instrument.IsForeignCurrency;
            IsSent = instrumentSerialObj.Instrument.IsSent;
            InstrumentType = instrumentSerialObj.Instrument.InstrumentType;

            Department = instrumentSerialObj.Instrument.Department;
            ItemCode = instrumentSerialObj.Instrument.Item.ItemCode;
            ItemShortDescription = instrumentSerialObj.Instrument.Item.ShortDescription;
            ItemDescriptionText = instrumentSerialObj.Instrument.Item.Description.Text;
        }


        private InstrumentType _instrumentType = new InstrumentType();
        public InstrumentType InstrumentType
        {
            get { return _instrumentType; }
            set { SetProperty(ref _instrumentType, value); }
        }

        private Department _department = new Department();
        public Department Department
        {
            get { return _department; }
            set { SetProperty(ref _department, value); }
        }

        private string _serialNO;
        public string SerialNO
        {
            get { return _serialNO; }
            set { SetProperty(ref _serialNO, value); }
        }

        private string _manufacturer;
        public string Manufacturer
        {
            get { return _manufacturer; }
            set { SetProperty(ref _manufacturer, value); }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }

        private bool? _hasAccessory;
        public bool? HasAccessory
        {
            get { return _hasAccessory; }
            set { SetProperty(ref _hasAccessory, value); }
        }

        private string _approvalCode;
        public string ApprovalCode
        {
            get { return _approvalCode; }
            set { SetProperty(ref _approvalCode, value); }
        }

        private bool? _isPrinted;
        public bool? IsPrinted
        {
            get { return _isPrinted; }
            set { SetProperty(ref _isPrinted, value); }
        }

        private bool? _isForeignCurrency;
        public bool? IsForeignCurrency
        {
            get { return _isForeignCurrency; }
            set { SetProperty(ref _isForeignCurrency, value); }
        }

        private bool? _isSent;
        public bool? IsSent
        {
            get { return _isSent; }
            set { SetProperty(ref _isSent, value); }
        }

        private string _itemCode;
        public string ItemCode
        {
            get { return _itemCode; }
            set { SetProperty(ref _itemCode, value); }
        }

        private string _itemShortDescription;
        public string ItemShortDescription
        {
            get { return _itemShortDescription; }
            set { SetProperty(ref _itemShortDescription, value); }
        }

        private string _itemDescriptiontext;
        public string ItemDescriptionText
        {
            get { return _itemDescriptiontext; }
            set { SetProperty(ref _itemDescriptiontext, value); }
        }
    }

}
