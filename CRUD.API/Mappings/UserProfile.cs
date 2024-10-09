using AutoMapper;
using CRUD.API.DTOs;
using CRUD.Data.Models;

namespace CRUD.API.Mappings
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            //CreateMap<User, CreateUserRequest>();

            CreateMap<User, UserResponse>();

            CreateMap<CreateUserRequest, User>();

            CreateMap<UpdateUserRequest, User>();
        }
    }
}
