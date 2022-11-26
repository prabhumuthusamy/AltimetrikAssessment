using Microsoft.AspNetCore.Mvc;
using TestProject.DTO.Account;
using TestProject.Service.Interface;

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
		public async Task<IActionResult> GetAccountsAsync(int id, CancellationToken cancellationToken = default)
		{
			if (id <= 0) return NotFound();
			var result = await _accountService.GetAccountsAsync(id, cancellationToken);
			return Result(result);
		}

		[HttpPost("")]
		public async Task<IActionResult> CreateAccountAsync(int id, [FromBody] CreateAccountRequestDTO requestDto, CancellationToken cancellationToken = default)
		{
			if (id <= 0) return NotFound();
			return Result(await _accountService.CreateAccountAsync(id, requestDto, cancellationToken));
		}
	}
}
