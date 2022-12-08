namespace TestProject.DTO.User
{
	public class CreateUserRequestDto
	{
		[Required]
		public string Name { get; set; }
		[Required]
		[DataType(DataType.EmailAddress)]
		public string EmailAddress { get; set; }
		[Required]
		[Range(typeof(decimal), "0", "79228162514264337593543950335")]
		public decimal MonthlySalary { get; set; }
		[Required]
		[Range(typeof(decimal), "0", "79228162514264337593543950335")]
		public decimal MonthlyExpenses { get; set; }
	}
}
