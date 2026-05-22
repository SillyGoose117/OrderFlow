namespace OrderFlow.Console.Models;

delegate void ValidationRule (out string errorMessage);

class OrderValidator
{
    private Order order;
    
    private ValidationRule ruleOfValidity;
    private Func<Order, bool> statusRule;
    private Func<Order, bool> deliveryMinValueRule;
    private Func<Order, bool> correctDateRule;
    private int minDeliveryValue = 20;

    private void CheckOrderQuantity(out string errorMessage) //Sprawdź czy koszyk nie jest pusty (delegat ValidationRule)
    {
        errorMessage = "";
        if (this.order.Items.Count < 1)
        {
            errorMessage = "Your order is empty!";
        }
    }

    private void ValidateItemAvailability(out string errorMessage) //Sprawdź czy produkt z zamówienia jest dostępny (delegat ValidationRule)
    {
        errorMessage = "";
        foreach (var item in this.order.Items)
        {
            var product = item.Product;
            var productAmountOrdered = item.Quantity;
            if (product.Stock == 0 ||  product.Stock < productAmountOrdered)
            {
                errorMessage += $"The item you were trying to get \"{product.Name}\" is currently out of stock or in insufficient quantity.";
            }
        }
    }

    private void ValidateCustomerInfo(out string errorMessage) //Sprawdź czy informacje dot. klienta nie są wybrakowane (delegat ValidationRule)
    {
        errorMessage = "";
        if (string.IsNullOrWhiteSpace(this.order.Customer.FullName))
        {
            errorMessage += "Customer name and last name cannot be empty.\n";
        } 
        if (string.IsNullOrWhiteSpace(this.order.Customer.Email))
        {
            errorMessage += "Customer email cannot be empty.\n";
        } 
        if (string.IsNullOrWhiteSpace(this.order.Customer.Phone))
        {
            errorMessage += "Customer phone number cannot be empty.\n";
        } 
        if (string.IsNullOrWhiteSpace(this.order.Customer.Address))
        {
            errorMessage += "Customer address cannot be empty.";
        }
    }

    public OrderValidator(Order orderToValidate)
    {
        this.order = orderToValidate;
        
        ruleOfValidity = ValidateItemAvailability;
        ruleOfValidity += CheckOrderQuantity;
        ruleOfValidity += ValidateCustomerInfo;
        
        statusRule = isOrderStatusCorrect => isOrderStatusCorrect.CurrentStatus != Order.Status.Cancelled; //Zamóienie nie może być "cancelled"
        deliveryMinValueRule = isDeliveryValueReached =>
        {
            return this.order.TotalAmount > minDeliveryValue; //Zamówienia z dostawą tylko od jakiejś kwoty
        };
        correctDateRule = isOrderDateCorrect => this.order.OrderDate <= DateTime.Now;
    }

    public List<string> ValidateAll()
    {
        List<string> listOfRuleBreakers = new List<string>();
        foreach (ValidationRule rule in ruleOfValidity.GetInvocationList())
        {
            rule(out string errorMessage);

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                listOfRuleBreakers.Add(errorMessage);
            }
        }

        if (!statusRule.Invoke(this.order))
        {
            listOfRuleBreakers.Add("Status of a newly placed order cannot be set as 'CANCELLED'.");
        }

        if (!deliveryMinValueRule.Invoke(this.order))
        {
            listOfRuleBreakers.Add($"Minimal delivery amount not reached. The minimal amount for your order to be delivered is {minDeliveryValue}.");
        }

        if (!correctDateRule.Invoke(this.order))
        {
            listOfRuleBreakers.Add("Date of an order must not be from the future!");
        }
        return listOfRuleBreakers;
    }
}