using AutoMapper;
using TestProject.Data.Entity;
using TestProject.DTO.User;

namespace TestProject.WebAPI.AutoMapperProfile
{
	public class UserProfile : Profile
	{
		public UserProfile() {
			CreateMap<CreateUserRequestDto, User>()
				   .ForMember(x => x.Email, x => x.MapFrom(_ => _.EmailAddress));

			CreateMap<User, UserDetailResponseDto>()
				   .ForMember(x => x.EmailAddress, x => x.MapFrom(_ => _.Email));
		}
	}
}
