namespace Cema.Models;

public class Expense
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Supplier { get; set; }
    public decimal Amount { get; set; }
    public int GroupId { get; set; }
    public string Group { get; set; }
    public int CarId { get; set; }
}
