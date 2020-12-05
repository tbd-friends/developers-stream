using System.Collections.Generic;
using MediatR;
using viewmodels;

namespace application.Query
{
    public class GetAllTwitchStreamers : IRequest<IEnumerable<TwitchStreamerViewModel>>
    {

    }
}