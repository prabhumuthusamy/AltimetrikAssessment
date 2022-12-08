using TestProject.DTO.Account;

namespace TestProject.Tests.MockData
{
	public class AccountMockData
	{
		public static List<Account> GetAccounts()
		{
			return new List<Account>{
			new Account { Id = 1, Name = "Name 1", AccountNumber = "111111111111", CreditLimit = 501, UserId=1, CreatedOn = DateTime.Now, },
			new Account { Id = 2, Name = "Name 2", AccountNumber = "222222222222", CreditLimit = 502, UserId=1, CreatedOn = DateTime.Now, },
			new Account { Id = 3, Name = "Name 3", AccountNumber = "333333333333", CreditLimit = 503, UserId=1, CreatedOn = DateTime.Now, },
			new Account { Id = 4, Name = "Name 4", AccountNumber = "444444444444", CreditLimit = 504, UserId=1, CreatedOn = DateTime.Now, },
			new Account { Id = 5, Name = "Name 5", AccountNumber = "555555555555", CreditLimit = 505, UserId=1, CreatedOn = DateTime.Now, },
			new Account { Id = 6, Name = "Name 6", AccountNumber = "666666666666", CreditLimit = 506, UserId=1, CreatedOn = DateTime.Now, },
			new Account { Id = 7, Name = "Name 7", AccountNumber = "777777777777", CreditLimit = 507, UserId=1, CreatedOn = DateTime.Now, },
		 };
		}

		public static List<Account> GetEmptyAccount()
		{
			return new List<Account>();
		}


		public static Account NewAccount()
		{
			return new Account { Id = 0, Name = "Name 0", AccountNumber = "000000000", CreditLimit = 50, UserId = 1, CreatedOn = DateTime.Now, };
		}

		public static List<AccountDetailResponseDTO> MapUsersDto(List<Account> accounts)
		{
			return accounts.Select(x => new AccountDetailResponseDTO
			{
				Id = x.Id,
				Name = x.Name,
				AccountNumber = x.AccountNumber,
				CreditLimit = x.CreditLimit,
			}).ToList();
		}

		public static CreateAccountRequestDTO NewAccountDto()
		{
			return new CreateAccountRequestDTO { Name = "New Name", AccountNumber = "12345678", CreditLimit = 100, };
		}
	}
}
