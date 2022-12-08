using TestProject.Service.Service;
using TestProject.WebAPI.AutoMapperProfile;

namespace TestProject.Tests.Services
{
	public class TestAccountService : IDisposable
	{
		protected readonly AltimetrikDbContext _context;
		protected readonly IMapper _mapper;
		private readonly Mock<IUserService> _userService;
		protected readonly AccountService _accountService;
		protected readonly CancellationToken _token;
		public TestAccountService()
		{
			var options = new DbContextOptionsBuilder<AltimetrikDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

			_context = new AltimetrikDbContext(options);

			//auto mapper configuration
			var mockMapper = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new AccountProfile());
			});
			_mapper = mockMapper.CreateMapper();

			_context.Database.EnsureCreated();
			_userService = new Mock<IUserService>();

			_accountService = new AccountService(_context, _mapper, _userService.Object);
			_token = new CancellationToken();
		}

		[Fact]
		public async Task GetAllAccountAsync_ReturnAccountCollection()
		{
			/// Arrange
			_context.Accounts.AddRange(AccountMockData.GetAccounts());
			_context.SaveChanges();
			_userService.Setup(_ => _.GetUserByIdAsync(1, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(UserMockData.UserDetailDto(), true));

			/// Act
			var result = await _accountService.GetAccountsAsync(1, _token);

			/// Assert
			result.Data.Count().Should().Be(AccountMockData.GetAccounts().Count);
			result.IsSuccess.Should().Be(true);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
		}

		[Fact]
		public async Task GetAllAccountAsync_ReturnEmptyAccountCollection()
		{
			/// Arrange
			_context.Accounts.AddRange(AccountMockData.GetEmptyAccount());
			_context.SaveChanges();
			_userService.Setup(_ => _.GetUserByIdAsync(1, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(UserMockData.UserDetailDto(), true));

			/// Act
			var result = await _accountService.GetAccountsAsync(1, new CancellationToken());

			/// Assert
			result.Data.Count().Should().Be(0);
			result.IsSuccess.Should().Be(true);
			result.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}

		[Fact]
		public async Task GetAllAccountAsync_ReturnNotFoundAccountCollection()
		{
			/// Arrange
			_context.Accounts.AddRange(AccountMockData.GetAccounts());
			_context.SaveChanges();
			_userService.Setup(_ => _.GetUserByIdAsync(1, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(UserMockData.UserDetailDto(), false, HttpStatusCode.NotFound));

			/// Act
			var result = await _accountService.GetAccountsAsync(1, new CancellationToken());

			/// Assert
			result.Data.Should().BeNull();
			result.IsSuccess.Should().Be(false);
			result.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		[Fact]
		public async Task CreateAccountAsync_AddNewAccount_NotAcceptable()
		{
			/// Arrange
			var newAccount = AccountMockData.NewAccountDto();
			_context.Accounts.AddRange(AccountMockData.GetAccounts());
			_context.SaveChanges();
			var userData = UserMockData.UserDetailDto();
			userData.MonthlyExpenses = userData.MonthlySalary - 500;
			_userService.Setup(_ => _.GetUserByIdAsync(1, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(userData, true));

			/// Act
			var result = await _accountService.CreateAccountAsync(1, newAccount, new CancellationToken());

			///Assert
			int expectedRecordCount = (AccountMockData.GetAccounts().Count());
			_context.Accounts.Count().Should().Be(expectedRecordCount);
			result.IsSuccess.Should().Be(false);
			result.StatusCode.Should().Be(HttpStatusCode.NotAcceptable);
		}

		[Fact]
		public async Task CreateAccountAsync_AddNewAccount()
		{
			/// Arrange
			var newAccount = AccountMockData.NewAccountDto();
			_context.Accounts.AddRange(AccountMockData.GetAccounts());
			_context.SaveChanges();
			var userData = UserMockData.UserDetailDto();
			_userService.Setup(_ => _.GetUserByIdAsync(3, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(userData, true));

			/// Act
			var result = await _accountService.CreateAccountAsync(3, newAccount, new CancellationToken());

			///Assert
			int expectedRecordCount = (AccountMockData.GetAccounts().Count() + 1);
			_context.Accounts.Count().Should().Be(expectedRecordCount);
			result.IsSuccess.Should().Be(true);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
		}

		public void Dispose()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}
	}
}
