namespace TestProject.DTO.User
{
	public class ValidateUserEmailRequestDto
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string EmailAddress { get; set; }
	}
}
