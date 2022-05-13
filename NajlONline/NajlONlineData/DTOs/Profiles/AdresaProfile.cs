using AutoMapper;
using NajlONline.DTOs;
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
   public class AdresaProfile : Profile
    {
        public AdresaProfile()
        {
            CreateMap<AdresaModel, AdresaDTO>();
            CreateMap<AdresaUpdateDTO, AdresaModel>();
            CreateMap<AdresaCreationDTO, AdresaModel>();
            CreateMap<AdresaModel, AdresaConfirmation>();
            CreateMap<AdresaConfirmation, AdresaConfirmationDTO>();
        }
    }
}
