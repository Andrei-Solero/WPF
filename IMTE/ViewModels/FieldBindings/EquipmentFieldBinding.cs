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
    public class EquipmentFieldBinding : BindableBase
    {

        /// <summary>
        /// Use to set all the fields value from the functions parameter
        /// </summary>
        /// <param name="equipmentObj"></param>
        public virtual void SetFieldBinding(EquipmentSerial equipmentObj)
        {
            // Set values from the equipmentObj to the properties
            SerialNo = equipmentObj.SerialNo;
            Manufacturer = equipmentObj.Equipment.Manufacturer;
            Model = equipmentObj.Equipment.Model;
            HasAccessory = equipmentObj.Equipment.HasAccessory;
            ApprovalCode = equipmentObj.Equipment.ApprovalCode;
            IsPrinted = equipmentObj.Equipment.IsPrinted;
            IsForeignCurrency = equipmentObj.Equipment.IsForeignCurrency;
            IsSent = equipmentObj.Equipment.IsSent;
            EquipmentType = equipmentObj.Equipment.EquipmentType;
            
            ItemCode = equipmentObj.Equipment.Item.ItemCode;
            ItemShortDescription = equipmentObj.Equipment.Item.ShortDescription;
            ItemDescriptionText = equipmentObj.Equipment.Item.Description.Text;
        }


        private string _serialNo;
        public string SerialNo
        {
            get { return _serialNo; }
            set
            {
                SetProperty(ref _serialNo, value);
            }
        }

        private string _manufacturer;
        public string Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                SetProperty(ref _manufacturer, value);
            }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
            }
        }

        private bool? _hasAccessory;
        public bool? HasAccessory
        {
            get { return _hasAccessory; }
            set
            {
                SetProperty(ref _hasAccessory, value);
            }
        }

        private string _approvalCode;
        public string ApprovalCode
        {
            get { return _approvalCode; }
            set
            {
                SetProperty(ref _approvalCode, value);
            }
        }

        private bool? _isPrinted;
        public bool? IsPrinted
        {
            get { return _isPrinted; }
            set
            {
                SetProperty(ref _isPrinted, value);
            }
        }

        private bool? _isForeignCurrency;
        public bool? IsForeignCurrency
        {
            get { return _isForeignCurrency; }
            set
            {
                SetProperty(ref _isForeignCurrency, value);
            }
        }

        private bool? _isSent;
        public bool? IsSent
        {
            get { return _isSent; }
            set
            {
                SetProperty(ref _isSent, value);
            }
        }

        private EquipmentType _equipmentType = new EquipmentType();
        public EquipmentType EquipmentType
        {
            get { return _equipmentType; }
            set
            {
                SetProperty(ref _equipmentType, value);
            }
        }

        private string _itemCode;
        public string ItemCode
        {
            get { return _itemCode; }
            set
            {
                SetProperty(ref _itemCode, value);
            }
        }

        private string _itemShortDescription;
        public string ItemShortDescription
        {
            get { return _itemShortDescription; }
            set
            {
                SetProperty(ref _itemShortDescription, value);
            }
        }

        private string _itemDescriptiontext;
        public string ItemDescriptionText
        {
            get { return _itemDescriptiontext; }
            set
            {
                SetProperty(ref _itemDescriptiontext, value);
            }
        }

        private Department _department = new Department();
        public Department Department
        {
            get { return _department; }
            set { SetProperty(ref _department, value); }
        }


    }
}
