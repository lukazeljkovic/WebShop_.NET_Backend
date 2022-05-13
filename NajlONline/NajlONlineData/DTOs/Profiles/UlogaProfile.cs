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
    public class UlogaProfile : Profile
    {
        public UlogaProfile()
        {
            CreateMap<UlogaModel, UlogaDTO>();
            CreateMap<UlogaUpdateDTO, UlogaModel>();
            CreateMap<UlogaCreationDTO, UlogaModel>();
            CreateMap<UlogaModel, UlogaConfirmation>();
            CreateMap<UlogaConfirmation, UlogaConfirmationDTO>();
        }
    }
}
