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
    public class VrstaProizvodaProfile : Profile
    {
        public VrstaProizvodaProfile()
        {
            CreateMap<VrstaProizvodaModel, VrstaProizvodaDTO>();
            CreateMap<VrstaProizvodaUpdateDTO, VrstaProizvodaModel>();
            CreateMap<VrstaProizvodaCreationDTO, VrstaProizvodaModel>();
            CreateMap<VrstaProizvodaModel, VrstaProizvodaConfirmation>();
            CreateMap<VrstaProizvodaConfirmation, VrstaProizvodaConfirmationDTO>();
        }
    }
}
