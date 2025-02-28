namespace HomeEnergyApi.Models
{
    using Microsoft.EntityFrameworkCore;

    public class HomeUtilityProviderRepository : IWriteRepository<int, HomeUtilityProvider>, IReadRepository<int, HomeUtilityProvider>
    {
        private HomeDbContext context;

        public HomeUtilityProviderRepository(HomeDbContext context)
        {
            this.context = context;
        }

        public HomeUtilityProvider Save(HomeUtilityProvider homeUtilityProvider)
        {
            context.HomeUtilityProviders.Add(homeUtilityProvider);
            context.SaveChanges();
            return homeUtilityProvider;
        }

        public HomeUtilityProvider Update(int id, HomeUtilityProvider homeUtilityProvider)
        {
            homeUtilityProvider.Id = id;
            context.HomeUtilityProviders.Update(homeUtilityProvider);
            context.SaveChanges();
            return homeUtilityProvider;
        }

        public HomeUtilityProvider RemoveById(int id)
        {
            var homeUtilityProvider = context.HomeUtilityProviders.Find(id);
            context.HomeUtilityProviders.Remove(homeUtilityProvider);
            context.SaveChanges();
            return homeUtilityProvider;
        }

        public List<HomeUtilityProvider> FindAll()
        {
            return context.HomeUtilityProviders
            .ToList();
        }

        public HomeUtilityProvider FindById(int id)
        {
            return context.HomeUtilityProviders.Find(id);
        }

        public int Count()
        {
            return context.HomeUtilityProviders.Count();
        }
    }
}