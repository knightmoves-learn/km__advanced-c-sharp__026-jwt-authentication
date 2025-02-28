using Microsoft.EntityFrameworkCore;

namespace HomeEnergyApi.Models
{
    public class HomeRepository : IReadRepository<int, Home>, IWriteRepository<int, Home>, IOwnerLastNameQueryable<Home>
    {
        private HomeDbContext context;

        public HomeRepository(HomeDbContext context)
        {
            this.context = context;
        }

        public Home Save(Home home)
        {
            if (home.HomeUsageData != null)
            {
                var usageData = home.HomeUsageData;
                usageData.Home = home;
                context.HomeUsageDatas.Add(usageData);
            }

            context.Homes.Add(home);
            context.SaveChanges();
            return home;
        }

        public Home Update(int id, Home home)
        {
            home.Id = id;
            context.Homes.Update(home);
            context.SaveChanges();
            return home;
        }

        public List<Home> FindAll()
        {
            return context.Homes
            .Include(h => h.HomeUsageData)
            .Include(h => h.HomeUtilityProviders)
            .ToList();
        }

        public Home FindById(int id)
        {
            return context.Homes.Find(id);
        }

        public Home RemoveById(int id)
        {
            var home = context.Homes.Find(id);
            context.Homes.Remove(home);
            context.SaveChanges();
            return home;
        }

        public int Count()
        {
            return context.Homes.Count();
        }

        public List<Home> FindByOwnerLastName(string ownerLastName)
        {
            return context.Homes
                .Where(h => h.OwnerLastName == ownerLastName)
                .Include(h => h.HomeUsageData)
                .Include(h => h.HomeUtilityProviders)
                .ToList();
        }
    }
}