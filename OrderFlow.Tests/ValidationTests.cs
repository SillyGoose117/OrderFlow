using OrderFlow.Console.Models;
using OrderFlow.Console.Services;

namespace OrderFlow.Tests;

public class OrderValidatorTests
{
    
        [Fact]
        public void Check_IsCartEmpty_ReturnsErrorMessage()
        {
            //Arrange
            var testCustomer = new Customer { FullName = "John Test", Address = "Test st.", City = "Test City", CustomerId = 1, Email = "Mail", IsVIP = false , Phone = "123"};
          
            var order = new Order { Customer = testCustomer };
            
            var validator = new OrderValidator(order);

            //Act
            validator.CheckIfCartEmpty(out var errorMessage);

            //Assert
            Assert.Equal("Your order is empty!", errorMessage);
        }

        [Fact]
        public void Check_IfItemAvailable_ReturnsErrorMessage()
        {
            //Arrange
            var testCustomer2 = new Customer { }; // Wystarczy uzupełnic tylko używane pola
            var testProduct2 = new Product { Stock = 1 }; // W tym przypadku wykorzystuje tylko porónwanie sztuk
            var order2 = new Order { Customer = testCustomer2 };
            var orderItem2 = new OrderItem { Product = testProduct2, Quantity = 2 };
            order2.Items.Add(orderItem2);
            var validator2 = new OrderValidator(order2);
            //ACT
            validator2.ValidateItemAvailability(out var errorMessage);
            //Assert
            Assert.Equal($"The item you were trying to get \"{testProduct2.Name}\" is currently out of stock or in insufficient quantity.\n", errorMessage);
        }

        public static IEnumerable<object[]> TestClientData => new List<object[]>
        {
           new object[] { new Customer { FullName = "",  Address = "Test st.", City = "TestCity", Email = "Mail",  Phone = "123" }},
           new object[] { new Customer { FullName = "John Test",  Address = "" , City = "TestCity", Email = "Mail",  Phone = "123" }},
           new object[] { new Customer { FullName = "John Test",  Address = "Test st." , City = "", Email = "Mail",  Phone = "123" }},
           new object[] { new Customer { FullName = "John Test",  Address = "Test st." , City = "TestCity", Email = "",  Phone = "123" }},
           new object[] { new Customer { FullName = "John Test",  Address = "Test st." , City = "TestCity", Email = "Mail",  Phone = "" }},
        };

        [Theory]
        [MemberData(nameof(TestClientData))]
        public void Check_CustomerData_ReturnsErrorMessage(Customer customer)
        {
            //Arrange
            var customerTest = customer;
            var order = new Order { Customer = customerTest };
            
            var validator = new OrderValidator(order);
            //Act
            validator.ValidateCustomerInfo(out var errorMessage);
            //Assert
            Assert.NotEmpty(errorMessage);
        }

        // [Fact]
        // public void Check_CorrectOrderStatus_ReturnsFalse()
        // {
        //     //Arrange
        //     var order = new Order { };
        //     order.CurrentStatus = Order.Status.Cancelled;
        //     var validator = new OrderValidator(order);
        //     //Act
        //     var isStatusValid = validator.statusRule.Invoke(order);
        //     //Assert
        //     Assert.False(isStatusValid);
        //}

        [Fact]
        public void Check_OrderDateNotFromTheFuture_ReturnsFalse()
        {
            //Arrange
            var order = new Order { };
            order.OrderDate = DateTime.Now.AddDays(2);
            var validator = new OrderValidator(order);
            //Act
            var isDateCorrect = validator.correctDateRule.Invoke(order);
            //Assert
            Assert.False(isDateCorrect);
        }

        [Theory]
        [InlineData(Order.Status.Validated)]
        [InlineData(Order.Status.Processing)]
        [InlineData(Order.Status.Completed)]
        [InlineData(Order.Status.New)]
        public void Check_CorrectOrderStatus_ReturnsTrue(Order.Status status)
        {
            //Arrange
            var order = new Order {  };
            order.CurrentStatus = status;
            var validator = new OrderValidator(order);
            //Act
            var isStatusValid = validator.statusRule.Invoke(order);
            //Assert
            Assert.True(isStatusValid);
        }

        [Fact]
        public void FilterOrders_VariousOrders_FiltersCorrectly()
        {
            //Arrange
            var ordersCities = new List<Order>
            {
                new Order { Customer = new Customer { City = "TestCity" } },
                new Order { Customer = new Customer { City = "TestCity" } },
                new Order { Customer = new Customer { City = "CityTest" } }
            };
            //Act
            var filterOrders = OrderProcessor.FilterOrders(ordersCities, order => order.Customer.City == "TestCity");
            //Assert
            Assert.Equal(2, filterOrders.Count);
        }

        [Fact]
        public void ProcessOrders_VariousOrders_ProcessesCorrectly()
        {
            //Arrange
            var order1 = new Order { };
            var order2 = new Order { };
            var order3 = new Order { };
            var orders = new List<Order>();
            orders.Add(order1);
            orders.Add(order2);
            orders.Add(order3);
            //Act
            OrderProcessor.ProcessOrders(orders, order => order.CurrentStatus = Order.Status.Validated);
            //Assert
            Assert.All(orders, order => Assert.Equal(Order.Status.Validated, order.CurrentStatus));
        }

        [Fact]
        public void Check_ValidateAll_ReturnsErrorMessages()
        {
            //Arrange
            var orderToValidateAll = new Order { Customer = new Customer { FullName = "John Validator" , City =  "TestCity", Email = "Mail",  Phone = "" } }; //Brak maila, telefonu
            orderToValidateAll.OrderDate = DateTime.Now.AddDays(2);
            var testProductValidateAll = new Product { Stock = 50};
            var orderItemValidateAll = new OrderItem { Product = testProductValidateAll, Quantity = 2 , UnitPrice = 50};
            orderToValidateAll.Items.Add(orderItemValidateAll);
            var validator = new OrderValidator(orderToValidateAll);
            //Act
            var listOfErrors = validator.ValidateAll();
            //Assert
            //Sprawdza jedną named methods
            Assert.Contains(listOfErrors, errorMessage => errorMessage.Contains("phone"));
            //Sprawdza jeden z delegatów func
            Assert.Contains(listOfErrors, errorMessage => errorMessage.Contains("future"));
        }
}
