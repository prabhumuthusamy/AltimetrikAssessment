
namespace TestProject.Tests.Controllers
{
	public class AccountControllerTests
	{
		private readonly CancellationToken _token;
		private readonly Mock<IAccountService> _accountService;
		private readonly Mock<BaseController> _baseController;
		public AccountControllerTests()
		{
			_token = new CancellationToken();
			_accountService = new Mock<IAccountService>();
			_baseController = new Mock<BaseController>();
		}

		[Fact]
		public async Task GetAllAccountsAsync_ShouldReturn200Status()
		{
			/// Arrange
			_accountService.Setup(_ => _.GetAccountsAsync(1, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(AccountMockData.MapUsersDto(AccountMockData.GetAccounts()), true));

			var sut = new AccountController(_accountService.Object);

			/// Act
			var result = (ObjectResult)await sut.GetAccountsAsync(1, _token);


			// /// Assert
			result.StatusCode.Should().Be(200);
		}

		[Fact]
		public async Task GetAllAccountsAsync_ShouldReturn204NoContentStatus()
		{
			/// Arrange
			_accountService.Setup(_ => _.GetAccountsAsync(1, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(AccountMockData.MapUsersDto(AccountMockData.GetEmptyAccount()), true, HttpStatusCode.NoContent));

			var sut = new AccountController(_accountService.Object);

			/// Act
			var result = (ObjectResult)await sut.GetAccountsAsync(1, _token);


			/// Assert
			result.StatusCode.Should().Be(204);
			_accountService.Verify(_ => _.GetAccountsAsync(1, _token), Times.Exactly(1));
		}

		[Fact]
		public async Task GetAllAccountsAsync_ShouldReturn404NotFoundStatus()
		{
			/// Arrange
			_accountService.Setup(_ => _.GetAccountsAsync(1, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(AccountMockData.MapUsersDto(AccountMockData.GetEmptyAccount()), true, HttpStatusCode.NotFound));

			var sut = new AccountController(_accountService.Object);

			/// Act
			var result = (ObjectResult)await sut.GetAccountsAsync(1, _token);


			/// Assert
			result.StatusCode.Should().Be(404);
			_accountService.Verify(_ => _.GetAccountsAsync(1, _token), Times.Exactly(1));
		}

		[Fact]
		public async Task CreateAccountAsync_ShouldCall_CreateAccountAsync_AtleastOnce()
		{
			/// Arrange
			var newAccount = AccountMockData.NewAccountDto();
			var sut = new AccountController(_accountService.Object);
			_accountService.Setup(_ => _.CreateAccountAsync(1, newAccount, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(1, true));

			/// Act
			var result = (ObjectResult)await sut.CreateAccountAsync(1, newAccount, _token);

			/// Assert
			result.StatusCode.Should().Be(200);
			_accountService.Verify(_ => _.CreateAccountAsync(1, newAccount, _token), Times.Exactly(1));
		}

		[Fact]
		public async Task CreateAccountAsync_ShouldCall_CreateUserAsync_NotAccept()
		{
			/// Arrange
			var newAccount = AccountMockData.NewAccountDto();
			_accountService.Setup(_ => _.CreateAccountAsync(1, newAccount, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(0, false, HttpStatusCode.NotAcceptable));

			var sut = new AccountController(_accountService.Object);

			/// Act
			var result = (ObjectResult)await sut.CreateAccountAsync(1, newAccount, _token);

			/// Assert
			result.StatusCode.Should().Be(406);
			_accountService.Verify(_ => _.CreateAccountAsync(1, newAccount, _token), Times.Exactly(1));
		}

		[Fact]
		public async Task CreateAccountAsync_ShouldCall_CreateUserAsync_NotFound()
		{
			/// Arrange
			var newAccount = AccountMockData.NewAccountDto();
			_accountService.Setup(_ => _.CreateAccountAsync(1, newAccount, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(0, false, HttpStatusCode.NotFound));

			var sut = new AccountController(_accountService.Object);

			/// Act
			var result = (ObjectResult)await sut.CreateAccountAsync(1, newAccount, _token);

			/// Assert
			result.StatusCode.Should().Be(404);
			_accountService.Verify(_ => _.CreateAccountAsync(1, newAccount, _token), Times.Exactly(1));
		}
	}
}
