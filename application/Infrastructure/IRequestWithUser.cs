using MediatR;

namespace application.Infrastructure
{
    public interface IBaseRequestWithUser
    {
        string Email { get; set; }
    }

    public interface IRequestWithUser : IBaseRequestWithUser, IRequest
    {

    }

    public interface IRequestWithUser<out TResponse> : IBaseRequestWithUser, IRequest<TResponse>
    {

    }
}