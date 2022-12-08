namespace TestProject.Data.Entity
{
	public class User : BaseEntity
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public decimal MonthlySalary { get; set; }
		public decimal MonthlyExpenses { get; set; }
		public DateTime CreatedOn { get; set; }
	}
}
