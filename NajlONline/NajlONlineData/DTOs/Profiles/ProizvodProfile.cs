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
    public class ProizvodProfile : Profile
    {
        public ProizvodProfile()
        {
            CreateMap<ProizvodModel, ProizvodDTO>();
            CreateMap<ProizvodUpdateDTO, ProizvodModel>();
            CreateMap<ProizvodCreationDTO, ProizvodModel>();
            CreateMap<ProizvodModel, ProizvodConfirmation>();
            CreateMap<ProizvodConfirmation, ProizvodConfirmationDTO>();
        }
    }
}
