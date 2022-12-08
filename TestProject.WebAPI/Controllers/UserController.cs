using Microsoft.AspNetCore.Mvc;
using TestProject.DTO.User;
using TestProject.Service.Interface;

namespace TestProject.WebAPI.Controllers
{
	[Route("api/[controller]")]
	public class UserController : BaseController
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("")]
		public async Task<IActionResult> GetUsersAsync(CancellationToken cancellationToken = default)
		{
			var result = await _userService.GetUsersAsync(cancellationToken);
			return Result(result);
		}

		[HttpGet("{available}/{pageSize}")]
		public async Task<IActionResult> GetUsersAsync(int available, int pageSize, CancellationToken cancellationToken = default)
		{
			var result = await _userService.GetUsersAsync(available, pageSize, cancellationToken);
			return Result(result);
		}

		[HttpPost("")]
		public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequestDto requestDto, CancellationToken cancellationToken = default)
		{
			if (requestDto == null) { return BadRequest(); }
			return Result(await _userService.CreateUserAsync(requestDto, cancellationToken));
		}

		[HttpPost("check")]
		public async Task<IActionResult> ValidateUserEmailAsync([FromBody] ValidateUserEmailRequestDto requestDto, CancellationToken cancellationToken = default)
		{
			return Result(await _userService.CheckUserEmailAsync(requestDto.EmailAddress, cancellationToken));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetUserAsync(int id, CancellationToken cancellationToken = default)
		{
			var result = await _userService.GetUserByIdAsync(id, cancellationToken);
			return Result(result);
		}
	}
}
