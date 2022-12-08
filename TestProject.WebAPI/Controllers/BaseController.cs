
namespace TestProject.WebAPI.Controllers
{
	[ApiController]
	public class BaseController : ControllerBase
	{
		public IActionResult Result<T>(ResponseDTO<T> result)
		{
			return StatusCode((int)result.StatusCode, result);
		}
	}
}
