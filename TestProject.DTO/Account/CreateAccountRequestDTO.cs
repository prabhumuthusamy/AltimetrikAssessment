namespace TestProject.DTO.Account
{
	public class CreateAccountRequestDTO
	{
		[Required]
		[StringLength(50)]
		public string Name { get; set; }
		[Required]
		[StringLength(50)]
		public string AccountNumber { get; set; }
		[Required]
		[Range(typeof(decimal), "0", "1000")]
		public decimal CreditLimit { get; set; }
	}
}
