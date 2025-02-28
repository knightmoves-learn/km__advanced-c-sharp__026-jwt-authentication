using AutoMapper;
using HomeEnergyApi.Models;

namespace HomeEnergyApi.Dtos
{
    public class HomeProfile : Profile
    {
        public HomeProfile()
        {
            CreateMap<HomeDto, Home>()
                .ForMember(dest => dest.HomeUsageData,
                           opt => opt.MapFrom(src => src.MonthlyElectricUsage != null
                                                     ? new HomeUsageData { MonthlyElectricUsage = src.MonthlyElectricUsage }
                                                     : null));
            
            CreateMap<UtilityProviderDto, UtilityProvider>();
                // .ForMember(dest => dest.ProvidedUtilities,
                //            opt => opt.MapFrom(src => src.ProvidedUtilities != null
                //                                      ? src.ProvidedUtilities));
        }
    }
}
