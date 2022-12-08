using TestProject.DTO.User;

namespace TestProject.Service.Service
{
	public class UserService : IUserService
	{
		private readonly IMapper _mapper;
		private readonly AltimetrikDbContext _dbContext;

		public UserService(IMapper mapper, AltimetrikDbContext dbContext)
		{
			_mapper = mapper;
			_dbContext = dbContext;
		}

		/// <summary>
		/// Create User
		/// </summary>
		/// <param name="requestDto">User details</param>
		/// <param name="token">Cancellation token</param>
		/// <returns>user id if success</returns>
		public async Task<ResponseDTO<int>> CreateUserAsync(CreateUserRequestDto requestDto, CancellationToken token)
		{
			var result = new ResponseDTO<int>();
			var isEmailExists = await CheckUserEmailAsync(requestDto.EmailAddress, token);
			if (!isEmailExists.IsSuccess)
			{
				result.ErrorCode = isEmailExists.ErrorCode;
				result.StatusCode = isEmailExists.StatusCode;
				return result;
			}

			var user = _mapper.Map<User>(requestDto);
			user.CreatedOn = DateTime.UtcNow;
			await _dbContext.AddAsync(user, token);
			await _dbContext.SaveChangesAsync(token);

			result.Data = user.Id;
			result.IsSuccess = true;
			return result;
		}

		/// <summary>
		/// Check user email already exists or not
		/// </summary>
		/// <param name="email">email address</param>
		/// <param name="token">Cancellation token</param>
		/// <returns>if email already exists return as true</returns>
		public async Task<ResponseDTO<bool>> CheckUserEmailAsync(string email, CancellationToken token)
		{
			var result = new ResponseDTO<bool>() { IsSuccess = true };
			email = email.ToLower().Trim();
			result.Data = await _dbContext.Users.AnyAsync(x => x.Email.ToLower().Trim() == email, token);
			if (result.Data)
			{
				result.StatusCode = HttpStatusCode.Conflict;
				result.IsSuccess = false;
				result.ErrorCode = "EmailExists";
			}

			return result;
		}

		/// <summary>
		/// Get user details based on User Id
		/// </summary>
		/// <param name="id">User Id</param>
		/// <param name="token">Cancellation token</param>
		/// <returns>return user details</returns>
		public async Task<ResponseDTO<UserDetailResponseDto>> GetUserByIdAsync(int id, CancellationToken token)
		{
			var result = new ResponseDTO<UserDetailResponseDto>();
			var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, token);
			if (user == null)
			{
				result.StatusCode = HttpStatusCode.NotFound;
				result.IsSuccess = false;
				result.ErrorCode = "UserNotExists";
				return result;
			}

			result.Data = _mapper.Map<UserDetailResponseDto>(user);
			result.IsSuccess = true;
			return result;
		}

		/// <summary>
		/// To Get list of all users
		/// </summary>
		/// <param name="token">Cancellation token</param>
		/// <returns>return list of users</returns>
		public async Task<ResponseDTO<List<UserDetailResponseDto>>> GetUsersAsync(CancellationToken token)
		{
			var result = new ResponseDTO<List<UserDetailResponseDto>>();
			var user = await _dbContext.Users.OrderBy(x => x.Name).ToListAsync(token);
			result.Data = _mapper.Map<List<UserDetailResponseDto>>(user);

			if (!result.Data.Any())
				result.StatusCode = HttpStatusCode.NoContent;

			result.IsSuccess = true;
			return result;
		}

		/// <summary>
		/// To Get list of all users
		/// </summary>
		/// <param name="availableCount">Available items</param>
		/// <param name="pageSize">page size</param>
		/// <param name="token">Cancellation token</param>
		/// <returns>return list of users</returns>
		public async Task<ResponseDTO<List<UserDetailResponseDto>>> GetUsersAsync(int availableCount, int pageSize, CancellationToken token)
		{
			if (availableCount <= 0) availableCount = 0;
			if (pageSize <= 0) pageSize = 1;

			var result = new ResponseDTO<List<UserDetailResponseDto>>();
			var user = await _dbContext.Users.OrderBy(x => x.Name).Skip(availableCount).Take(pageSize).ToListAsync(token);
			result.Data = _mapper.Map<List<UserDetailResponseDto>>(user);
			
			if (!result.Data.Any())
				result.StatusCode = HttpStatusCode.NoContent;

			result.IsSuccess = true;
			return result;
		}
	}
}
