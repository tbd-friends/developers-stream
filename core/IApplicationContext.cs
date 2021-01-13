using System.Linq;
using core.Models;

namespace core
{
    public interface IApplicationContext
    {
        IQueryable<AvailableTechnology> AvailableTechnologies { get; }
        IQueryable<StreamerClaimRequest> StreamerClaimRequests { get; }
        IQueryable<Streamer> Streamers { get; }
        IQueryable<StreamerPlatform> StreamerPlatforms { get; }
        IQueryable<StreamerTechnology> StreamerTechnologies { get; }

        int SaveChanges();
        void Insert<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
    }
}