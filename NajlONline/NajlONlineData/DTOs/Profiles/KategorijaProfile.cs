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
    class KategorijaProfile : Profile
    {
        public KategorijaProfile()
        {
            CreateMap<KategorijaModel, KategorijaDTO>();
            CreateMap<KategorijaUpdateDTO, KategorijaModel>();
            CreateMap<KategorijaCreationDTO, KategorijaModel>();
            CreateMap<KategorijaModel, KategorijaConfirmation>();
            CreateMap<KategorijaConfirmation, KategorijaConfirmationDTO>();
        }
    }
}
