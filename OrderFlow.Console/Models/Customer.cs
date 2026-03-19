namespace OrderFlow.Console.Models;

public class Customer
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Address { get; set; }
    public bool IsVIP { get; set; }

    public Customer(string name, string lastName, string email, string phone, string address, bool isVIP)
    {
        Name = name;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Address = address;
        IsVIP = isVIP;
    }
}