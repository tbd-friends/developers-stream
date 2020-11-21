using MediatR;

namespace application.Query
{
    public class IsPlatformRegistered : IRequest<bool>
    {
        public string Url { get; set; }
    }
}