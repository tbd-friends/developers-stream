using application.Infrastructure;
using viewmodels;

namespace application.Query
{
    public class SearchStreamers : IRequestWithUser<PagedResult<StreamerViewModel>>
    {
        public string Term { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string Email { get; set; }
    }
}