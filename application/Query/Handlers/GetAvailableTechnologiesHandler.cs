
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using viewmodels;

namespace application.Query.Handlers
{
    public class GetAvailableTechnologiesHandler : IRequestHandler<GetAvailableTechnologies, IEnumerable<TechnologyViewModel>>, IRequest<IEnumerable<TechnologyViewModel>>
    {
        private readonly IApplicationContext _context;

        public GetAvailableTechnologiesHandler(IApplicationContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<TechnologyViewModel>> Handle(GetAvailableTechnologies request, CancellationToken cancellationToken)
        {
            return Task.FromResult((from t in _context.AvailableTechnologies
                                    orderby t.Name
                                    select new TechnologyViewModel
                                    {
                                        Id = t.Id,
                                        Name = t.Name,
                                        Description = t.Description
                                    }).AsEnumerable());
        }
    }
}