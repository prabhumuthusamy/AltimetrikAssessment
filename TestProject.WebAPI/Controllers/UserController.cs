using TestProject.DTO.User;

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
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDTO<List<UserDetailResponseDto>>))]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetUsersAsync(CancellationToken cancellationToken = default)
		{
			var result = await _userService.GetUsersAsync(cancellationToken);
			return Result(result);
		}

		[HttpGet("{available}/{pageSize}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDTO<List<UserDetailResponseDto>>))]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		public async Task<IActionResult> GetUsersAsync(int available, int pageSize, CancellationToken cancellationToken = default)
		{
			var result = await _userService.GetUsersAsync(available, pageSize, cancellationToken);
			return Result(result);
		}

		[HttpPost("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDTO<int>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<IActionResult> CreateUserAsync([FromBody] CreateUserRequestDto requestDto, CancellationToken cancellationToken = default)
		{
			if (requestDto == null) { return BadRequest(); }
			return Result(await _userService.CreateUserAsync(requestDto, cancellationToken));
		}

		[HttpPost("check")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDTO<bool>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status409Conflict)]
		public async Task<IActionResult> ValidateUserEmailAsync([FromBody] ValidateUserEmailRequestDto requestDto, CancellationToken cancellationToken = default)
		{
			return Result(await _userService.CheckUserEmailAsync(requestDto.EmailAddress, cancellationToken));
		}

		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDTO<List<UserDetailResponseDto>>))]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status500InternalServerError)]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		public async Task<IActionResult> GetUserAsync(int id, CancellationToken cancellationToken = default)
		{
			if (id <= 0) return NotFound();
			var result = await _userService.GetUserByIdAsync(id, cancellationToken);
			return Result(result);
		}
	}
}
