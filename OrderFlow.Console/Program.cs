namespace OrderFlow.Console;
using OrderFlow.Console.Data;
using OrderFlow.Console.Models;
class Program

{
    static void Main(string[] args)
    {
        var goodOrder = SampleData.ListOfOrders[0];
        var myGoodValidator = new OrderValidator(goodOrder);
        myGoodValidator.ValidateAll();
        System.Console.WriteLine("----------------------------");
        var badOrder = SampleData.ListOfOrders[1]; 
        var myBadValidator = new OrderValidator(badOrder);
        myBadValidator.ValidateAll();
    }
}