using TestProject.DTO.Account;

namespace TestProject.Service.Service
{
	public class AccountService : IAccountService
	{
		private readonly AltimetrikDbContext _dbContext;
		private readonly IMapper _mapper;
		private readonly IUserService _userService;

		public AccountService(AltimetrikDbContext dbContext,
			IMapper mapper,
			IUserService userService)
		{
			_dbContext = dbContext;
			_mapper = mapper;
			_userService = userService;
		}

		/// <summary>
		/// Create Account for a user
		/// </summary>
		/// <param name="userId">User Id</param>
		/// <param name="requestDTO">Account details</param>
		/// <param name="token">Cancellation token</param>
		/// <returns>return account id if success</returns>
		public async Task<ResponseDTO<int>> CreateAccountAsync(int userId, CreateAccountRequestDTO requestDTO, CancellationToken token)
		{
			var result = new ResponseDTO<int>();
			var userInfo = await _userService.GetUserByIdAsync(userId, token);
			if (userInfo?.IsSuccess == true && userInfo?.Data != null)
			{
				if (userInfo.Data.MonthlySalary - userInfo.Data.MonthlyExpenses < 1000)
				{
					result.StatusCode = HttpStatusCode.NotAcceptable;
					result.ErrorCode = "NotEligible";
					return result;
				}

				//To restrict account has multiple account
				if (await _dbContext.Accounts.AnyAsync(x => x.UserId == userId, token))
				{
					result.StatusCode = HttpStatusCode.Conflict;
					result.ErrorCode = "AccountExists";
					return result;
				}

				var account = _mapper.Map<Account>(requestDTO);
				account.CreatedOn = DateTime.UtcNow;
				account.UserId = userId;
				await _dbContext.AddAsync(account, token);
				await _dbContext.SaveChangesAsync(token);

				result.Data = account.Id;
				result.IsSuccess = true;
				return result;
			}

			result.ErrorCode = userInfo?.ErrorCode;
			return result;
		}

		/// <summary>
		/// Get Accounts for a user
		/// </summary>
		/// <param name="userId">User Id</param>
		/// <param name="token">Cancellation token</param>
		/// <returns>list of accounts</returns>
		public async Task<ResponseDTO<List<AccountDetailResponseDTO>>> GetAccountsAsync(int userId, CancellationToken token)
		{
			var result = new ResponseDTO<List<AccountDetailResponseDTO>>();
			var userInfo = await _userService.GetUserByIdAsync(userId, token);
			if (userInfo?.IsSuccess == true && userInfo?.Data != null)
			{
				var user = await _dbContext.Accounts.Where(x => x.UserId == userId).ToListAsync(token);
				result.Data = _mapper.Map<List<AccountDetailResponseDTO>>(user);

				if (!result.Data.Any())
					result.StatusCode = HttpStatusCode.NoContent;
				
				result.IsSuccess = true;
				return result;
			}

			result.ErrorCode = userInfo?.ErrorCode;
			result.StatusCode = userInfo?.StatusCode ?? HttpStatusCode.NotFound;
			return result;
		}
	}
}
