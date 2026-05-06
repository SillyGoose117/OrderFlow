namespace OrderFlow.Console.Models;

public class Customer
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string City { get; set; }
    public string Address { get; set; }
    public bool IsVIP { get; set; }
    public Guid CustomerId { get; set; }

    public Customer()
    {
        
    }

    public Customer(string name, string lastName, string email, string phone, string city, string address, bool isVIP)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        Phone = phone;
        City = city;
        Address = address;
        IsVIP = isVIP;
        CustomerId = Guid.NewGuid();
    }

    public override string ToString()
    {
        return $"{Name}, {LastName}, {Email}, {Phone}, {Address} {(IsVIP ? "This customer is a VIP." : "This customer is not a VIP.")}";
    }
}