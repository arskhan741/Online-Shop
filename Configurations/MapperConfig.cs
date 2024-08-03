using AutoMapper;
using Online_Shop.DTOs.CategoryDTOs;
using Online_Shop.DTOs.RoleDTOs;
using Online_Shop.Models;


namespace Online_Shop.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig() 
        {
            CreateMap<Role, CreateRoleDTO>().ReverseMap();
            CreateMap<Role, UpdateRoleDTO>().ReverseMap();
            CreateMap<Role, DeleteRoleDTO>().ReverseMap();
            CreateMap<Role, GetRoleDetails>().ReverseMap();

            CreateMap<Category, CreateCategoryDTO>().ReverseMap();
            CreateMap<Category, UpdateCategoryDTO>().ReverseMap();
            CreateMap<Category, DeleteCategoryDTO>().ReverseMap();
            CreateMap<Category, GetCategoryDetailsDTO>().ReverseMap();

        }
    }
}
