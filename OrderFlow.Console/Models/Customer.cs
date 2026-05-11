namespace OrderFlow.Console.Models;

public class Customer
{
    public string FullName { get; set; }
    public string? Email { get; set; }
    public string Phone { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public bool IsVIP { get; set; }
    public Guid CustomerId { get; set; }
    public List<Order> Orders { get; set; }
    public string? Notes { get; set; }

    public Customer()
    {
        
    }

    public Customer(string name, string email, string phone, string city, string address, bool isVIP)
    {
        FullName = name;
        Email = email;
        Phone = phone;
        City = city;
        Address = address;
        IsVIP = isVIP;
        CustomerId = Guid.NewGuid();
    }

    public override string ToString()
    {
        return $"{FullName}, {Email}, {Phone}, {Address} {(IsVIP ? "This customer is a VIP." : "This customer is not a VIP.")}";
    }
}