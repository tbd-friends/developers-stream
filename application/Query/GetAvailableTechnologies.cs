using System.Collections;
using System.Collections.Generic;
using MediatR;
using viewmodels;

namespace application.Query
{
    public class GetAvailableTechnologies : IRequest<IEnumerable<TechnologyViewModel>>
    {
        
    }
}