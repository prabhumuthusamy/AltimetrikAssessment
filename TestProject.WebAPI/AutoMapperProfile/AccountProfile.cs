using TestProject.DTO.Account;

namespace TestProject.WebAPI.AutoMapperProfile
{
	public class AccountProfile : Profile
	{
		public AccountProfile()
		{
			CreateMap<CreateAccountRequestDTO, Account>();
			CreateMap<Account, AccountDetailResponseDTO>();
		}
	}
}
