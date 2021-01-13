using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using core.Enums;
using MediatR;
using viewmodels;

namespace application.Query.Handlers
{
    public class SearchStreamersHandler : IRequestHandler<SearchStreamers, PagedResult<StreamerViewModel>>
    {
        private readonly IApplicationContext _context;

        public SearchStreamersHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<PagedResult<StreamerViewModel>> Handle(SearchStreamers request, CancellationToken cancellationToken)
        {
            var streams = from stream in _context.Streamers
                          where stream.Status == StreamerStatus.Verified
                          select stream;

            if (!string.IsNullOrEmpty(request.Term))
            {
                streams = from stream in streams
                          let usesTechnology = (
                                                    from st in _context.StreamerTechnologies
                                                    join t in _context.AvailableTechnologies on st.TechnologyId equals t.Id
                                                    where
                                                        st.StreamerId == stream.Id &&
                                                        (t.Name.Contains(request.Term) || t.Aliases.Contains(request.Term))
                                                    select t
                                                    ).Any()
                          where stream.Name.Contains(request.Term) ||
                                stream.Description.Contains(request.Term) ||
                                usesTechnology
                          select stream;
            }

            return Task.FromResult(
                new PagedResult<StreamerViewModel>
                {
                    TotalItems = streams.Count(),
                    Results = from stream in streams.Skip((request.PageNumber - 1) * request.PageSize)
                            .Take(request.PageSize)
                            .AsEnumerable()
                              select new StreamerViewModel
                              {
                                  Id = stream.Id,
                                  Name = stream.Name,
                                  Description = stream.Description,
                                  CanStreamBeClaimed = request.Email != null &&
                                                       request.Email != stream.Email &&
                                                        !(from r in _context.StreamerClaimRequests
                                                          where r.ClaimedStreamerId == stream.Id &&
                                                                r.Status == ClaimRequestStatus.PendingApproval
                                                          select r).Any(),
                                  Platforms = from p in _context.StreamerPlatforms
                                              where p.StreamerId == stream.Id
                                              select new PlatformViewModel
                                              {
                                                  Name = p.Name,
                                                  Url = p.Url
                                              },
                                  Technologies = from st in _context.StreamerTechnologies
                                                 join a in _context.AvailableTechnologies on st.TechnologyId equals a.Id
                                                 where st.StreamerId == stream.Id
                                                 select a.Name
                              }
                });
        }
    }
}