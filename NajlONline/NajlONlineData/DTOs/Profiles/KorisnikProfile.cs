using AutoMapper;
using NajlONline.DTOs;
using NajlONline.DTOs.Confirmation;
using NajlONline.DTOs.Creation;
using NajlONline.Models;
using NajlONlineData.DTOs.Confirmation;
using NajlONlineData.DTOs.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NajlONlineData.DTOs.Profiles
{
    public class KorisnikProfile : Profile
    {
        public KorisnikProfile()
        {
            CreateMap<KorisnikModel, KorisnikDTO>();
           // CreateMap<KorisnikModel, KorisnikUpdateDTO>();
            CreateMap<KorisnikUpdateDTO, KorisnikModel>();
            CreateMap<KorisnikCreationDTO, KorisnikModel>();
            CreateMap<KorisnikModel, KorisnikConfirmation>();
            CreateMap<KorisnikConfirmation, KorisnikConfirmationDTO>();

        }
    }
}
