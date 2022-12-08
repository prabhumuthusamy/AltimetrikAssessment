using TestProject.DTO.User;

namespace TestProject.Service.Interface
{
	public interface IUserService
	{
		Task<ResponseDTO<List<UserDetailResponseDto>>> GetUsersAsync(CancellationToken token);
		Task<ResponseDTO<UserDetailResponseDto>> GetUserByIdAsync(int id, CancellationToken token);
		Task<ResponseDTO<bool>> CheckUserEmailAsync(string email, CancellationToken token);
		Task<ResponseDTO<int>> CreateUserAsync(CreateUserRequestDto requestDto, CancellationToken token);
		Task<ResponseDTO<List<UserDetailResponseDto>>> GetUsersAsync(int availableCount, int pageSize, CancellationToken token);
	}
}