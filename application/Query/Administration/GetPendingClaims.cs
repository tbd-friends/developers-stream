using System.Collections.Generic;
using MediatR;
using viewmodels;

namespace application.Query.Administration
{
    public class GetPendingClaims : IRequest<IEnumerable<StreamerClaimViewModel>>
    {
        
    }
}