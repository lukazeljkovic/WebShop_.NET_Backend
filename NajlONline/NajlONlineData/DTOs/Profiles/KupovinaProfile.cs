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
    class KupovinaProfile : Profile
    {
        public KupovinaProfile()
        {
            CreateMap<KupovinaModel, KupovinaDTO>();
            CreateMap<KupovinaUpdateDTO, KupovinaModel>();
            CreateMap<KupovinaCreationDTO, KupovinaModel>();
            CreateMap<KupovinaModel, KupovinaConfirmation>();
            CreateMap<KupovinaConfirmation, KupovinaConfirmationDTO>();
        }
    
    }

}
