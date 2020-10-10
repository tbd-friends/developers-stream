using System.Collections.Generic;
using MediatR;
using viewmodels;

namespace application.Query
{
    public class GetStreams : IRequest<IEnumerable<StreamViewModel>>
    {
        
    }
}