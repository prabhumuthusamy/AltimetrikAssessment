namespace TestProject.Data.Entity
{
	public class Account : BaseEntity
	{
		public string Name { get; set; }
		public string AccountNumber { get; set; }
		public decimal CreditLimit { get; set; }
		public DateTime CreatedOn { get; set; }
		public int UserId { get; set; }
	}
}
