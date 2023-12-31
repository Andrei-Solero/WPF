﻿using IMTE.Models.Definition;
using IMTE.Models.General;
using IMTE.Models.HumanResources;
using IMTE.Models.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMTE.Models.Inventory
{
    public class Instrument : StandardModel
    {
        public Instrument()
        {
            
        }
		
        public Instrument(int id, int version, bool isDeleted, DateTime createdOn, DateTime modifiedOn,
					  Company company, int? assetId, InstrumentType instrumentType,
					  Employee modifiedByEmployee, int approvedByUserId, Currency currency,
					  int currencyExchangeId, Item item, Specification specification, Department department,
					  string manufacturer, string model, bool? hasAccessory, string approvalCode,
					  bool? isPrinted, bool? isSent, bool? isForeignCurrency, DateTime createdDate)
		{
			Id = id;
			Version = version;
			IsDeleted = isDeleted;
			CreatedOn = createdOn;
			ModifiedOn = modifiedOn;
			Company = company;
			AssetId = assetId;
			InstrumentType = instrumentType;
			ModifiedByEmployee = modifiedByEmployee;
			ApprovedByUserId = approvedByUserId;
			Currency = currency;
			CurrencyExchangeId = currencyExchangeId;
			Item = item;
			Specification = specification;
			Department = department;
			Manufacturer = manufacturer;
			Model = model;
			HasAccessory = hasAccessory;
			ApprovalCode = approvalCode;
			IsPrinted = isPrinted;
			IsSent = isSent;
			IsForeignCurrency = isForeignCurrency;
			CreatedDate = createdDate;
		}


		public Company Company { get; set; }
        public int? AssetId { get; set; }
        public InstrumentType InstrumentType { get; set; }
        public Employee ModifiedByEmployee { get; set; }
        public int ApprovedByUserId { get; set; }
        public Currency Currency { get; set; }
        public int CurrencyExchangeId { get; set; }
        public Item Item { get; set; }
        public Specification Specification { get; set; }
        public Department Department { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public bool? HasAccessory { get; set; }
        public string ApprovalCode { get; set; }
        public bool? IsPrinted { get; set; }
        public bool? IsSent { get; set; }
        public bool? IsForeignCurrency { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
