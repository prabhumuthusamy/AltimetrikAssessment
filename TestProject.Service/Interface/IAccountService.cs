using TestProject.DTO.Account;

namespace TestProject.Service.Interface
{
	public interface IAccountService
	{
		Task<ResponseDTO<int>> CreateAccountAsync(int userId, CreateAccountRequestDTO requestDTO, CancellationToken token);
		Task<ResponseDTO<List<AccountDetailResponseDTO>>> GetAccountsAsync(int userId, CancellationToken token);
	}
}