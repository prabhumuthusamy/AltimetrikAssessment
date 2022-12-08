using TestProject.Service.Service;
using TestProject.WebAPI.AutoMapperProfile;

namespace TestProject.Tests.Services
{
	public class TestUserService : IDisposable
	{
		protected readonly AltimetrikDbContext _context;
		protected readonly IMapper _mapper;
		public TestUserService()
		{
			var options = new DbContextOptionsBuilder<AltimetrikDbContext>()
			.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			.Options;

			_context = new AltimetrikDbContext(options);

			//auto mapper configuration
			var mockMapper = new MapperConfiguration(cfg =>
			{
				cfg.AddProfile(new UserProfile());
			});
			_mapper = mockMapper.CreateMapper();

			_context.Database.EnsureCreated();
		}

		[Fact]
		public async Task GetAllUsersAsync_ReturnUserCollection()
		{
			/// Arrange
			_context.Users.AddRange(UserMockData.GetUsers());
			_context.SaveChanges();

			var userService = new UserService(_mapper, _context);

			/// Act
			var result = await userService.GetUsersAsync(new CancellationToken());

			/// Assert
			result.Data.Count().Should().Be(UserMockData.GetUsers().Count);
			result.IsSuccess.Should().Be(true);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
		}

		[Fact]
		public async Task GetAllUsersAsync_ReturnEmptyUserCollection()
		{
			/// Arrange
			_context.Users.AddRange(UserMockData.GetEmptyUsers());
			_context.SaveChanges();

			var userService = new UserService(_mapper, _context);

			/// Act
			var result = await userService.GetUsersAsync(new CancellationToken());

			/// Assert
			result.Data.Count().Should().Be(0);
			result.IsSuccess.Should().Be(true);
			result.StatusCode.Should().Be(HttpStatusCode.NoContent);
		}

		[Fact]
		public async Task CreateUserAsync_AddNewUser()
		{
			/// Arrange
			var newUser = UserMockData.NewUserDto();
			_context.Users.AddRange(UserMockData.GetUsers());
			_context.SaveChanges();

			var userService = new UserService(_mapper, _context);

			/// Act
			var result = await userService.CreateUserAsync(newUser, new CancellationToken());

			///Assert
			int expectedRecordCount = (UserMockData.GetUsers().Count() + 1);
			_context.Users.Count().Should().Be(expectedRecordCount);
			result.IsSuccess.Should().Be(true);
			result.StatusCode.Should().Be(HttpStatusCode.OK);
		}

		[Fact]
		public async Task CreateUserAsync_AddNewUser_EmailExists()
		{
			/// Arrange
			var users = UserMockData.GetUsers();
			var newUser = UserMockData.NewUserDto();
			newUser.EmailAddress = users.FirstOrDefault().Email;

			_context.Users.AddRange(users);
			_context.SaveChanges();

			var userService = new UserService(_mapper, _context);

			/// Act
			var result = await userService.CreateUserAsync(newUser, new CancellationToken());

			///Assert
			int expectedRecordCount = (UserMockData.GetUsers().Count());
			_context.Users.Count().Should().Be(expectedRecordCount);
			result.IsSuccess.Should().Be(false);
			result.StatusCode.Should().Be(HttpStatusCode.Conflict);
		}

		[Fact]
		public async Task GetUserDetailAsync_GetUser()
		{
			/// Arrange
			var users = UserMockData.GetUsers();
			_context.Users.AddRange(users);
			_context.SaveChanges();

			var userService = new UserService(_mapper, _context);

			/// Act
			var result = await userService.GetUserByIdAsync(1, new CancellationToken());

			///Assert
			int expectedRecordCount = (UserMockData.GetUsers().Count());
			_context.Users.Count().Should().Be(expectedRecordCount);
			result.IsSuccess.Should().Be(true);
			result.Data.Should().NotBeNull();
			result.StatusCode.Should().Be(HttpStatusCode.OK);
		}

		[Fact]
		public async Task GetUserDetailAsync_GetUser_NotExists()
		{
			/// Arrange
			var users = UserMockData.GetUsers();
			_context.Users.AddRange(users);
			_context.SaveChanges();

			var userService = new UserService(_mapper, _context);

			/// Act
			var result = await userService.GetUserByIdAsync(100, new CancellationToken());

			///Assert
			int expectedRecordCount = (UserMockData.GetUsers().Count());
			_context.Users.Count().Should().Be(expectedRecordCount);
			result.IsSuccess.Should().Be(false);
			result.Data.Should().BeNull();
			result.StatusCode.Should().Be(HttpStatusCode.NotFound);
		}

		public void Dispose()
		{
			_context.Database.EnsureDeleted();
			_context.Dispose();
		}
	}
}
