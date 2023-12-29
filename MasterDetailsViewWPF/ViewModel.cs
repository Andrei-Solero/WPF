using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterDetailsViewWPF
{
    public class ViewModel
    {
        ObservableCollection<Employee> _employees;

        public ObservableCollection<Employee> Employees
        {
            get { return _employees; }
            set { _employees = value; }
        }

        public ViewModel()
        {
            this.GenerateOrders();
            this.GenerateSales();
            _employees = GetEmployeesDetails();
        }

        public ObservableCollection<Employee> GetEmployeesDetails()
        {
            var employees = new ObservableCollection<Employee>();
            employees.Add(new Employee() { EmployeeID = 1, OrderID = 1001, City = "Berlin", Orders = GetOrders(1001), Sales = GetSales(1001) });
            employees.Add(new Employee() { EmployeeID = 2, OrderID = 1002, City = "Mexico D.F.", Orders = GetOrders(1002), Sales = GetSales(1002) });
            employees.Add(new Employee() { EmployeeID = 3, OrderID = 1003, City = "London", Orders = GetOrders(1002), Sales = GetSales(1003) });
            employees.Add(new Employee() { EmployeeID = 4, OrderID = 1004, City = "BERGS", Orders = GetOrders(1002), Sales = GetSales(1004) });
            employees.Add(new Employee() { EmployeeID = 5, OrderID = 1005, City = "Mannheim", Orders = GetOrders(1002), Sales = GetSales(1005) });
            return employees;
        }

        //Orders collection is initialized here.
        ObservableCollection<OrderInfo> Orders = new ObservableCollection<OrderInfo>();

        public void GenerateOrders()
        {
            Orders.Add(new OrderInfo() { OrderID = 1001, Quantity = 10 });
            Orders.Add(new OrderInfo() { OrderID = 1001, Quantity = 10 });
            Orders.Add(new OrderInfo() { OrderID = 1002, Quantity = 20 });
            Orders.Add(new OrderInfo() { OrderID = 1002, Quantity = 20 });
            Orders.Add(new OrderInfo() { OrderID = 1003, Quantity = 50 });
            Orders.Add(new OrderInfo() { OrderID = 1004, Quantity = 70 });
            Orders.Add(new OrderInfo() { OrderID = 1005, Quantity = 20 });
            Orders.Add(new OrderInfo() { OrderID = 1005, Quantity = 20 });
        }

        private ObservableCollection<OrderInfo> GetOrders(int orderID)
        {
            ObservableCollection<OrderInfo> orders = new ObservableCollection<OrderInfo>();

            foreach (var order in Orders)

                if (order.OrderID == orderID)
                    orders.Add(order);
            return orders;
        }

        //Sales collection is initialized here.
        ObservableCollection<SalesInfo> Sales = new ObservableCollection<SalesInfo>();

        public void GenerateSales()
        {
            Sales.Add(new SalesInfo() { OrderID = 1001, SalesID = "A00001", ProductName = "Bike1" });
            Sales.Add(new SalesInfo() { OrderID = 1001, SalesID = "A00002", ProductName = "Bike1" });
            Sales.Add(new SalesInfo() { OrderID = 1002, SalesID = "A00003", ProductName = "Cycle" });
            Sales.Add(new SalesInfo() { OrderID = 1003, SalesID = "A00004", ProductName = "Car" });
        }

        private ObservableCollection<SalesInfo> GetSales(int orderID)
        {
            ObservableCollection<SalesInfo> sales = new ObservableCollection<SalesInfo>();

            foreach (var sale in Sales)

                if (sale.OrderID == orderID)
                    sales.Add(sale);
            return sales;
        }
    }
}
