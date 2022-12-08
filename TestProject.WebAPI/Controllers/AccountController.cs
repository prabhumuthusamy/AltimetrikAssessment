using TestProject.DTO.Account;

namespace TestProject.WebAPI.Controllers
{
	[Route("api/{id}/[controller]")]
	public class AccountController : BaseController
	{
		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService = accountService;
		}

		[HttpGet("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDTO<List<AccountDetailResponseDTO>>))]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public async Task<IActionResult> GetAccountsAsync(int id, CancellationToken cancellationToken = default)
		{
			if (id <= 0) return NotFound();
			var result = await _accountService.GetAccountsAsync(id, cancellationToken);
			return Result(result);
		}

		[HttpPost("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDTO<int>))]
		[ProducesResponseType(StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]

		public async Task<IActionResult> CreateAccountAsync(int id, [FromBody] CreateAccountRequestDTO requestDto, CancellationToken cancellationToken = default)
		{
			if (id <= 0) return NotFound();
			return Result(await _accountService.CreateAccountAsync(id, requestDto, cancellationToken));
		}
	}
}
