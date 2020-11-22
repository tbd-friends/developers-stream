using System.Collections.Generic;
using MediatR;
using viewmodels;

namespace application.Query
{
    public class GetStreamers : IRequest<PagedResult<StreamerViewModel>>
    {
        public string Term { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}