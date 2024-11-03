using API.Models.DTO;
using API.Models.EntityFramework;
using AutoMapper;

namespace API.Models.Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<UtilisateurDTO, Utilisateur>().ReverseMap();    
        }
    }
}
