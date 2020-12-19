using System.Collections.Generic;
using administration.viewmodels;
using MediatR;

namespace application.Query.Streamer
{
    public class GetAssociatedStreamers : IRequest<IEnumerable<StreamerViewModel>>
    {
        public string Email { get; set; }
    }
}