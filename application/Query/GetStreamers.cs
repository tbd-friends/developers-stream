using System.Collections.Generic;
using MediatR;
using viewmodels;

namespace application.Query
{
    public class GetStreamers : IRequest<PagedResult<StreamViewModel>>
    {
        public string Term { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}