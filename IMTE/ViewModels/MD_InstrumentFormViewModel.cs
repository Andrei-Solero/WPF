using IMTE.DataAccess;
using IMTE.EventAggregator.Core;
using IMTE.IMTEEntity.Models;
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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class MD_InstrumentFormViewModel : BindableBase, INavigationAware, IDataErrorInfo
    {
        private readonly IEventAggregator ea;
        private readonly IDialogService dialogService;
        private readonly InstrumentTypeDA instrumentTypeDA;
		private readonly InstrumentSerialDA instrumentSerialDA;
        private readonly DepartmentDA departmentDA;

        public DelegateCommand OpenDescriptionLookupCommand { get; private set; }
        public DelegateCommand OpenItemLookupCommand { get; private set; }
        public DelegateCommand ShowInstrumentConfigCommand { get; private set; }

        public List<InstrumentSerial> ExistingInstrumentSerial { get; private set; }

        public MD_InstrumentFormViewModel(IEventAggregator ea, IDialogService dialogService)
        {
            this.ea = ea;
            this.dialogService = dialogService;
            instrumentTypeDA = new InstrumentTypeDA();
            departmentDA = new DepartmentDA();
            instrumentSerialDA = new InstrumentSerialDA();

            Instrument = new Instrument();
            ItemEntity = new Item();
            Description = new Description();


            OpenDescriptionLookupCommand = new DelegateCommand(OpenDescriptionLookup);
            OpenItemLookupCommand = new DelegateCommand(OpenItemLookup);
			ShowInstrumentConfigCommand = new DelegateCommand(OpenInstrumentConfig);

            Task.Run(async () => await LoadDataToFormAsync());

            //// this will trigger when the user selected an existing measuring device and that measuring device has InstrumentSerial
            //ea.GetEvent<MeasuringDeviceToInstrumentSerial>().Subscribe(SetInstrumentSerialFromMeasuringDevice);

			// send the instrument data to the main measuring device form
			ea.GetEvent<InstrumentSerialToMeasuringDevice>().Publish(InstrumentSerial);

			// get the data from this form's description lookup
			ea.GetEvent<DescriptionLookupToMDForms>().Subscribe(SetDescriptionFromLookup);

            // get the data from this form's item lookup
            ea.GetEvent<ItemLookupToMDForms>().Subscribe(SetItemFromLookup);

            ea.GetEvent<DataFromIsntrumentLookup>().Subscribe(SetInstrumentDetailsFromLookup);

		}


        private async Task LoadDataToFormAsync()
        {
            var existingInstrumentSerial = await instrumentSerialDA.GetAllInstrumentSerial();
            ExistingInstrumentSerial = existingInstrumentSerial.ToList();

            InstrumentTypes = new ObservableCollection<InstrumentType>(await instrumentTypeDA.GetAllInstrumentType());

            Departments = new ObservableCollection<Department>(await departmentDA.GetDepartments());
        }

        private void SetInstrumentDetailsFromLookup(Instrument instrument)
		{
            Instrument = instrument;
            SetFieldBindingData(instrument);
		}

		private void OpenInstrumentConfig()
		{
			dialogService.ShowDialog("InstrumentConfig");

		}

		private void SetInstrumentSerialFromMeasuringDevice(InstrumentSerial serial)
		{
            InstrumentSerial = serial;
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
            ItemEntity = itemObj;
            FieldValidation();
        }

        private void SetDescriptionFromLookup(Description descriptionObj)
        {
            Description = descriptionObj;
            FieldValidation();
        }

		private Dictionary<string, string> _errorCollection = new Dictionary<string, string>();
		public Dictionary<string, string> ErrorCollection
		{
			get { return _errorCollection; }
			set { SetProperty(ref _errorCollection, value); }
		}

        #region IDataErrorInfo - Validation

        private bool FieldValidation()
        {
            var output = true;

            if (string.IsNullOrEmpty(SerialNO) ||
                string.IsNullOrEmpty(Manufacturer) ||
                string.IsNullOrEmpty(Model) ||
                Department.Id == null ||
                InstrumentType.Id == null ||
                string.IsNullOrEmpty(ApprovalCode) ||
                string.IsNullOrEmpty(ItemEntity.ItemCode) ||
                string.IsNullOrEmpty(ItemEntity.ShortDescription) ||
                string.IsNullOrEmpty(ItemEntity.Description.Text))
            {
                ea.GetEvent<ToolFormValidationToMeasuringDevice>().Publish(false);
                output = false;
            }
            else if (InstrumentSerial == null || InstrumentSerial.Id == 0 || InstrumentSerial.Id == null)
            {
                if (ExistingInstrumentSerial.Any(x => x.SerialNo == SerialNO))
                {
                    ea.GetEvent<ToolFormValidationToMeasuringDevice>().Publish(false);
                    output = false;
                }
            }

            ea.GetEvent<ToolFormValidationToMeasuringDevice>().Publish(output);
            return output;
		}

		public string Error => null;

		public string this[string columnName]
		{
			get
			{
				string result = null;
				string errorTextForCmb = "Select required option...";
				string errorTextForText = "Required...";

				switch (columnName)
				{
                    case "SerialNO":
                        if (string.IsNullOrEmpty(SerialNO))
                            result = errorTextForText;
                        if (InstrumentSerial == null || InstrumentSerial.Id == 0 || InstrumentSerial.Id == null)
                        {
                            if (ExistingInstrumentSerial != null && ExistingInstrumentSerial.Any(x => x.SerialNo == SerialNO))
                                result = "Serial No already exists";
                        }
                        break;
                    case "Manufacturer":
                        if (string.IsNullOrEmpty(Manufacturer))
                            result = errorTextForText;
                        break;
                    case "Model":
                        if (string.IsNullOrEmpty(Model))
                            result = errorTextForText;
                        break;
                    case "ApprovalCode":
                        if (string.IsNullOrEmpty(ApprovalCode))
                            result = errorTextForText;
                        break;
                    case "InstrumentType":
                        if (Instrument == null || InstrumentType.Id == null)
                            result = errorTextForCmb;
                        break;
                    case "Department":
						if (Department == null || Department.Id == null)
							result = errorTextForCmb;
						break;
					case "ItemCode":
                        if (string.IsNullOrEmpty(ItemCode))
                            result = errorTextForText;
                        break;
                    case "ItemShortDescription":
                        if (string.IsNullOrEmpty(ItemShortDescription))
                            result = errorTextForText;
                        break;
                    case "ItemDescriptionText":
                        if (string.IsNullOrEmpty(ItemDescriptionText))
                            result = errorTextForText;
                        break;

                }

				if (ErrorCollection.ContainsKey(columnName))
					ErrorCollection[columnName] = result;
				else if (result != null)
					ErrorCollection.Add(columnName, result);

				SetProperty(ref _errorCollection, ErrorCollection);

				return result;
			}
		}

        #endregion

		#region Helper

		private void SetFieldBindingData(InstrumentSerial instrumentSerialObj)
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
            ItemEntity = instrumentSerialObj.Instrument.Item;
            Description = instrumentSerialObj.Instrument.Item.Description;
            Department = instrumentSerialObj.Instrument.Department;
            ItemCode = instrumentSerialObj.Instrument.Item.ItemCode;
            ItemShortDescription = instrumentSerialObj.Instrument.Item.ShortDescription;
            ItemDescriptionText = instrumentSerialObj.Instrument.Item.Description.Text;
        }

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
			ItemEntity = instrumentObj.Item;
			Description = instrumentObj.Item.Description;
			Department = instrumentObj.Department;
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

                Instrument = value.Instrument;
                FieldValidation();

            }
        }


        private Instrument _instrument;
        public Instrument Instrument
        {
            get { return _instrument; }
            set 
            { 
                SetProperty(ref _instrument, value);

                InstrumentSerial.Instrument = value;
                ItemEntity = value.Item;
                FieldValidation();

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

                FieldValidation();

            }
        }

        private Item _item;
        public Item ItemEntity
        {
            get { return _item; }
            set 
            { 
                SetProperty(ref _item, value);
                Instrument.Item = value;

                if (value != null)
                {
					ItemCode = value.ItemCode;
					ItemShortDescription = value.ShortDescription;
                    Description = value.Description;
				}

                FieldValidation();

            }
        }

        private Description _description;
        public Description Description
        {
            get { return _description; }
            set 
            { 
                SetProperty(ref _description, value);
                ItemEntity.Description = value;

                // For field binding
                if (value != null)
                {
                    ItemDescriptionText = value.Text;
                }

                FieldValidation();

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
                FieldValidation();
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
				FieldValidation();
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
				FieldValidation();

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
				FieldValidation();

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
				FieldValidation();

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
				FieldValidation();

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
				FieldValidation();

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
				FieldValidation();

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
				FieldValidation();

			}
		}

        private string _itemCode;
        public string ItemCode
        {
            get { return _itemCode; }
            set
            {
                SetProperty(ref _itemCode, value);
                ItemEntity.ItemCode = value;
				FieldValidation();

			}
		}

        private string _itemShortDescription;
        public string ItemShortDescription
        {
            get { return _itemShortDescription; }
            set
            {
                SetProperty(ref _itemShortDescription, value);
                ItemEntity.ShortDescription = value;
				FieldValidation();

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
				FieldValidation();

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
            if (navigationContext.Parameters.Count != 0)
            {
                // Getting the data from the measuring device list - measuring device form
                InstrumentSerial = navigationContext.Parameters["instrumentObj"] as InstrumentSerial;

                // set the binding fields values from the navigationcontext parameter
                SetFieldBindingData(InstrumentSerial);

                // send the data again back to measuring device form
                ea.GetEvent<InstrumentSerialToMeasuringDevice>().Publish(InstrumentSerial);
			}
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
