namespace API.Customer.Data.DataObjects
{
  public class CustomerInformation
  {
    public string OfficialId { get; set; }
    public string Email { get; set; }
    public Address Adress { get; set; }
    public string PhoneNumber { get; set; }
  }
}