using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        void Insert<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
    }
}