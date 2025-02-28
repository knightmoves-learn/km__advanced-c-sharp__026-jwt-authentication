using HomeEnergyApi.Models;

namespace HomeEnergyApi.Services
{
    public class HomeUtilityProviderService
    {
        private IReadRepository<int, UtilityProvider> utilityProviderRepository;
        private IWriteRepository<int, HomeUtilityProvider> homeUtilityProviderRepository;

        public HomeUtilityProviderService(IReadRepository<int, UtilityProvider> utilityProviderRepository, IWriteRepository<int, HomeUtilityProvider> homeUtilityProviderRepository)
        {
            this.utilityProviderRepository = utilityProviderRepository;
            this.homeUtilityProviderRepository = homeUtilityProviderRepository;
        }

        public void Associate(Home home, ICollection<int> utilityProviderIds)
        {
            if(utilityProviderIds != null)
            {
                foreach(int utilityProviderId in utilityProviderIds)
                {
                    UtilityProvider utilityProvider = utilityProviderRepository.FindById(utilityProviderId);
                    HomeUtilityProvider homeUtilityProvider = new();
                    homeUtilityProvider.UtilityProvider = utilityProvider;
                    homeUtilityProvider.UtilityProviderId = utilityProviderId;
                    homeUtilityProvider.HomeId = home.Id;
                    homeUtilityProvider.Home = home;

                    homeUtilityProviderRepository.Save(homeUtilityProvider);
                }
            }
        }
    }
}