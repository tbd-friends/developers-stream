using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using application.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace site.Infrastructure.Behaviors
{
    public class AttachUserBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IHttpContextAccessor _context;

        public AttachUserBehavior(IHttpContextAccessor context)
        {
            _context = context;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var principal = _context.HttpContext?.User;

            if (principal != null && 
                principal.HasClaim(c => c.Type == ClaimTypes.Email) && 
                request is IBaseRequestWithUser rq)
            {
                rq.Email = principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            }

            var response = await next();

            return response;
        }
    }
}