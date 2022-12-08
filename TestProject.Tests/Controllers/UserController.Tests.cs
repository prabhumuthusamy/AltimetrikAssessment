
using Moq;

namespace TestProject.Tests.Controllers
{
	public class UserControllerTests
	{
		private readonly CancellationToken _token;
		private readonly Mock<IUserService> _userService;
		public UserControllerTests()
		{
			_token = new CancellationToken();
			_userService = new Mock<IUserService>();
		}

		[Fact]
		public async Task GetAllUsersAsync_ShouldReturn200Status()
		{
			/// Arrange
			_userService.Setup(_ => _.GetUsersAsync(_token))
				.ReturnsAsync(UserMockData.CreateResponseDto(UserMockData.MapUsersDto(UserMockData.GetUsers()), true));

			var sut = new UserController(_userService.Object);

			/// Act
			var result = (ObjectResult)await sut.GetUsersAsync(_token);


			// /// Assert
			result.StatusCode.Should().Be(200);
		}

		[Fact]
		public async Task GetAllUsersAsync_ShouldReturn204NoContentStatus()
		{
			/// Arrange
			_userService.Setup(_ => _.GetUsersAsync(_token))
				.ReturnsAsync(UserMockData.CreateResponseDto(UserMockData.MapUsersDto(UserMockData.GetEmptyUsers()), true, HttpStatusCode.NoContent));
			var sut = new UserController(_userService.Object);

			/// Act
			var result = (ObjectResult)await sut.GetUsersAsync(_token);


			/// Assert
			result.StatusCode.Should().Be(204);
			_userService.Verify(_ => _.GetUsersAsync(_token), Times.Exactly(1));
		}

		[Fact]
		public async Task CreateUserAsync_ShouldCall_CreateUserAsync_AtleastOnce()
		{
			/// Arrange
			var newUser = UserMockData.NewUserDto();
			_userService.Setup(_ => _.CreateUserAsync(newUser, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(1, true));

			var sut = new UserController(_userService.Object);

			/// Act
			var result = (ObjectResult)await sut.CreateUserAsync(newUser);

			/// Assert
			result.StatusCode.Should().Be(200);
			_userService.Verify(_ => _.CreateUserAsync(newUser, _token), Times.Exactly(1));
		}

		[Fact]
		public async Task CreateUserAsync_ShouldCall_CreateUserAsync_EmailExists()
		{
			/// Arrange
			var newUser = UserMockData.NewUserDto();
			_userService.Setup(_ => _.CreateUserAsync(newUser, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(0, false, HttpStatusCode.Conflict));

			var sut = new UserController(_userService.Object);

			/// Act
			var result = (ObjectResult)await sut.CreateUserAsync(newUser);

			/// Assert
			result.StatusCode.Should().Be(409);
			_userService.Verify(_ => _.CreateUserAsync(newUser, _token), Times.Exactly(1));
		}

		[Fact]
		public async Task GetUserAsync_ShouldReturn200Status()
		{
			/// Arrange
			var newUser = UserMockData.NewUserDto();
			_userService.Setup(_ => _.GetUserByIdAsync(1, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(UserMockData.UserDetailDto(), true));

			var sut = new UserController(_userService.Object);

			/// Act
			var result = (ObjectResult)await sut.GetUserAsync(1, _token);

			/// Assert
			result.StatusCode.Should().Be(200);
			_userService.Verify(_ => _.GetUserByIdAsync(1, _token), Times.Exactly(1));
		}
		[Fact]
		public async Task GetUserAsync_ShouldReturn404Status()
		{
			/// Arrange
			var newUser = UserMockData.NewUserDto();
			_userService.Setup(_ => _.GetUserByIdAsync(12, _token))
				.ReturnsAsync(UserMockData.CreateResponseDto(UserMockData.NullUserDetailDto(), false, HttpStatusCode.NotFound));

			var sut = new UserController(_userService.Object);

			/// Act
			var result = (ObjectResult)await sut.GetUserAsync(12, _token);

			/// Assert
			result.StatusCode.Should().Be(404);
			_userService.Verify(_ => _.GetUserByIdAsync(12, _token), Times.Exactly(1));
		}
	}
}
