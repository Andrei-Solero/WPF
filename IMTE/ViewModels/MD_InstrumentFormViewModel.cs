using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.Models.General;
using IMTE.Models.Inventory;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
    public class MD_InstrumentFormViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator ea;
        private readonly IDialogService dialogService;
        private readonly InstrumentTypeDA instrumentTypeDA;
        private readonly DepartmentDA departmentDA;

        public DelegateCommand OpenDescriptionLookupCommand { get; private set; }
        public DelegateCommand OpenItemLookupCommand { get; private set; }
        public DelegateCommand ShowEquipmentConfigCommand { get; private set; }

        public MD_InstrumentFormViewModel(IEventAggregator ea, IDialogService dialogService)
        {
            this.ea = ea;
            this.dialogService = dialogService;
            instrumentTypeDA = new InstrumentTypeDA();
            departmentDA = new DepartmentDA();

            InstrumentTypes = new ObservableCollection<InstrumentType>(instrumentTypeDA.GetAllInstrumentType());
            Departments = new ObservableCollection<Department>(departmentDA.GetAllDepartments());

            OpenDescriptionLookupCommand = new DelegateCommand(OpenDescriptionLookup);
            OpenItemLookupCommand = new DelegateCommand(OpenItemLookup);
            ShowEquipmentConfigCommand = new DelegateCommand(OpenEquipmentConfig);

            // send the instrument data to the main measuring device form
            ea.GetEvent<InstrumentSerialToMeasuringDevice>().Publish(InstrumentSerial);

            // get the data from this form's description lookup
            ea.GetEvent<DescriptionLookupToMDForms>().Subscribe(SetDescriptionFromLookup);

            // get the data from this form's item lookup
            ea.GetEvent<ItemLookupToMDForms>().Subscribe(SetItemFromLookup);
        }

        private void OpenEquipmentConfig()
        {
            dialogService.ShowDialog("EquipmentConfig");
        }

        private void OpenItemLookup()
        {
            dialogService.ShowDialog("ItemConfig");
        }

        private void OpenDescriptionLookup()
        {
            dialogService.ShowDialog("ItemDescriptionConfig");
        }

        private void SetItemFromLookup(Item itemObj)
        {
            Item = itemObj;
        }

        private void SetDescriptionFromLookup(Description descriptionObj)
        {
            Description = descriptionObj;
        }

        #region Helper

        private void SetFieldBindingData(Instrument instrumentObj)
        {
            Manufacturer = instrumentObj.Manufacturer;
            Model = instrumentObj.Model;
            HasAccessory = instrumentObj.HasAccessory;
            ApprovalCode = instrumentObj.ApprovalCode;
            IsPrinted = instrumentObj.IsPrinted;
            IsForeignCurrency = instrumentObj.IsForeignCurrency;
            IsSent = instrumentObj.IsSent;
            InstrumentType = instrumentObj.InstrumentType;
            Item = instrumentObj.Item;
            Description = instrumentObj.Item.Description;
            ItemCode = instrumentObj.Item.ItemCode;
            ItemShortDescription = instrumentObj.Item.ShortDescription;
            ItemDescriptionText = instrumentObj.Item.Description.Text;
        }

        #endregion

        #region ----------------FIELD BINDING----------------

        private InstrumentSerial _instrumentSerial = new InstrumentSerial();
        public InstrumentSerial InstrumentSerial
        {
            get { return _instrumentSerial; }
            set 
            { 
                SetProperty(ref _instrumentSerial, value);
            }
        }


        private Instrument _instrument = new Instrument();
        public Instrument Instrument
        {
            get { return _instrument; }
            set 
            { 
                SetProperty(ref _instrument, value);
                SetFieldBindingData(value);
            }
        }

        private InstrumentType _instrumentType = new InstrumentType();
        public InstrumentType InstrumentType
        {
            get { return _instrumentType; }
            set 
            { 
                SetProperty(ref _instrumentType, value);
                Instrument.InstrumentType = value;
            }
        }

        private Item _item = new Item();
        public Item Item
        {
            get { return _item; }
            set 
            { 
                SetProperty(ref _item, value);
                Instrument.Item = value;

                ItemCode = value.ItemCode;
                ItemShortDescription = value.ShortDescription;
                ItemDescriptionText = value.Description.Text;
            }
        }

        private Description _description = new Description();
        public Description Description
        {
            get { return _description; }
            set 
            { 
                SetProperty(ref _description, value);
                Item.Description = value;
            }
        }

        private Department _department = new Department();
        public Department Department
        {
            get { return _department; }
            set 
            {
                SetProperty(ref _department, value);
                Instrument.Department = value;
            }
        }

        private string _serialNO;
        public string SerialNO
        {
            get { return _serialNO; }
            set
            {
                SetProperty(ref _serialNO, value);
                InstrumentSerial.SerialNo = value;
                InstrumentSerial.Instrument = Instrument;
            }
        }


        private string _manufacturer;
        public string Manufacturer
        {
            get { return _manufacturer; }
            set
            {
                SetProperty(ref _manufacturer, value);
                Instrument.Manufacturer = value;
                InstrumentSerial.Instrument = Instrument;
            }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            set
            {
                SetProperty(ref _model, value);
                Instrument.Model = value;
                InstrumentSerial.Instrument = Instrument;
            }
        }

        private bool? _hasAccessory;
        public bool? HasAccessory
        {
            get { return _hasAccessory; }
            set
            {
                SetProperty(ref _hasAccessory, value);
                Instrument.HasAccessory = value;
                InstrumentSerial.Instrument = Instrument;
            }
        }

        private string _approvalCode;
        public string ApprovalCode
        {
            get { return _approvalCode; }
            set
            {
                SetProperty(ref _approvalCode, value);
                Instrument.ApprovalCode = value;
                InstrumentSerial.Instrument = Instrument;
            }
        }

        private bool? _isPrinted;
        public bool? IsPrinted
        {
            get { return _isPrinted; }
            set
            {
                SetProperty(ref _isPrinted, value);
                Instrument.IsPrinted = value;
                InstrumentSerial.Instrument = Instrument;
            }
        }

        private bool? _isForeignCurrency;
        public bool? IsForeignCurrency
        {
            get { return _isForeignCurrency; }
            set
            {
                SetProperty(ref _isForeignCurrency, value);
                Instrument.IsForeignCurrency = value;
                InstrumentSerial.Instrument = Instrument;
            }
        }

        private bool? _isSent;
        public bool? IsSent
        {
            get { return _isSent; }
            set
            {
                SetProperty(ref _isSent, value);
                Instrument.IsSent = value;
                InstrumentSerial.Instrument = Instrument;
            }
        }

        private string _itemCode;
        public string ItemCode
        {
            get { return _itemCode; }
            set
            {
                SetProperty(ref _itemCode, value);
                Item.ItemCode = value;
                InstrumentSerial.Instrument = Instrument;
            }
        }

        private string _itemShortDescription;
        public string ItemShortDescription
        {
            get { return _itemShortDescription; }
            set
            {
                SetProperty(ref _itemShortDescription, value);
                Item.ShortDescription = value;
                InstrumentSerial.Instrument = Instrument;
            }
        }

        private string _itemDescriptiontext;
        public string ItemDescriptionText
        {
            get { return _itemDescriptiontext; }
            set
            {
                SetProperty(ref _itemDescriptiontext, value);
                Description.Text = value;
                InstrumentSerial.Instrument = Instrument;
            }
        }

        #endregion

        #region INavigationAware

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        #endregion

        #region ObservableCollections

        private ObservableCollection<InstrumentType> _instrumentTypes;
        public ObservableCollection<InstrumentType> InstrumentTypes
        {
            get { return _instrumentTypes; }
            set { SetProperty(ref _instrumentTypes, value); }
        }

        private ObservableCollection<Department> _departments;
        public ObservableCollection<Department> Departments
        {
            get { return _departments; }
            set { SetProperty(ref _departments, value); }
        }

        #endregion


    }
}
