using AutoMapper;
using ApiStarter.DTOs.Users;
using ApiStarter.Models;

namespace ApiStarter.Mappers;

public class UserProfile : Profile
{
	public UserProfile()
	{
		CreateMap<User, UserDto>();

		CreateMap<UpdateUserDto, User>()
				.ForMember(dest => dest.Id, opt => opt.Ignore())
				.ForMember(dest => dest.Email, opt => opt.Ignore())
				.ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
	}
}