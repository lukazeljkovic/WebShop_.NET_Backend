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
    class SezonaProfile : Profile
    {
        public SezonaProfile()
        {
            CreateMap<SezonaModel, SezonaDTO>();
            CreateMap<SezonaUpdateDTO, SezonaModel>();
            CreateMap<SezonaCreationDTO, SezonaModel>();
            CreateMap<SezonaModel, SezonaConfirmation>();
            CreateMap<SezonaConfirmation, SezonaConfirmationDTO>();
        }
    }
}
