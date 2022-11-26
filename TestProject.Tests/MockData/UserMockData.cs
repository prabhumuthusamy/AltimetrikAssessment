using TestProject.DTO.User;

namespace TestProject.Tests.MockData
{
	public class UserMockData
	{
		public static List<User> GetUsers()
		{
			return new List<User>{
			new User { Id = 1, Name = "Name 1", Email = "email1@testmail.com", MonthlyExpenses = 2001, MonthlySalary = 5001, CreatedOn = DateTime.Now, },
			new User { Id = 2, Name = "Name 2", Email = "email2@testmail.com", MonthlyExpenses = 2002, MonthlySalary = 5002, CreatedOn = DateTime.Now, },
			new User { Id = 3, Name = "Name 3", Email = "email3@testmail.com", MonthlyExpenses = 2003, MonthlySalary = 5003, CreatedOn = DateTime.Now, },
			new User { Id = 4, Name = "Name 4", Email = "email4@testmail.com", MonthlyExpenses = 2004, MonthlySalary = 5004, CreatedOn = DateTime.Now, },
			new User { Id = 5, Name = "Name 5", Email = "email5@testmail.com", MonthlyExpenses = 2005, MonthlySalary = 5005, CreatedOn = DateTime.Now, },
			new User { Id = 6, Name = "Name 6", Email = "email6@testmail.com", MonthlyExpenses = 2006, MonthlySalary = 5006, CreatedOn = DateTime.Now, },
			new User { Id = 7, Name = "Name 7", Email = "email7@testmail.com", MonthlyExpenses = 2007, MonthlySalary = 5007, CreatedOn = DateTime.Now, },
		 };
		}

		public static List<User> GetEmptyUsers()
		{
			return new List<User>();
		}


		public static User NewUser()
		{
			return new User { Id = 0, Name = "New Name", Email = "newemail@testmail.com", MonthlyExpenses = 2000, MonthlySalary = 5000, CreatedOn = DateTime.Now, };
		}

		public static List<UserDetailResponseDto> MapUsersDto(List<User> users)
		{
			return users.Select(x => new UserDetailResponseDto
			{
				Id = x.Id,
				Name = x.Name,
				EmailAddress = x.Email,
				MonthlyExpenses = x.MonthlyExpenses,
				MonthlySalary = x.MonthlySalary,
			}).ToList();
		}

		public static CreateUserRequestDto NewUserDto()
		{
			return new CreateUserRequestDto { Name = "New Name", EmailAddress = "newemail@testmail.com", MonthlyExpenses = 2000, MonthlySalary = 5000 };
		}

		public static UserDetailResponseDto NullUserDetailDto()
		{
			return null;
		}

		public static UserDetailResponseDto UserDetailDto()
		{
			return MapUsersDto(GetUsers()).FirstOrDefault();
		}

		public static ResponseDTO<T> CreateResponseDto<T>(T data, bool isSuccess=false, HttpStatusCode code= HttpStatusCode.OK, string errorCode = null)
		{
			return new ResponseDTO<T>
			{
				IsSuccess = isSuccess,
				Data = data,
				StatusCode = code,
				ErrorCode = errorCode
			};
		}
	}
}
