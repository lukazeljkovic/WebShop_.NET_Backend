using AutoMapper;
using NajlONline.Models;
using NajlONlineData.DTOs.Confirmation;
using NajlONlineData.DTOs.Creation;
using NajlONlineData.DTOs.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Profiles
{
    public class KarticaProfile : Profile
    {
        public KarticaProfile()
        {
            CreateMap<KarticaModel, KarticaDTO>();
            CreateMap<KarticaUpdateDTO, KarticaModel>();
            CreateMap<KarticaCreationDTO, KarticaModel>();
            CreateMap<KarticaModel, KarticaConfirmation>();
            CreateMap<KarticaConfirmation, KarticaConfirmationDTO>();
        }
    }
}
