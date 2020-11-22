using System.Collections.Generic;
using MediatR;
using administration.viewmodels;

namespace application.Query.Administration
{
    public class GetPendingStreamers : IRequest<IEnumerable<StreamerViewModel>>
    {
        
    }
}