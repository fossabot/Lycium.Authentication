using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public interface IAuthenticationDelegate
    {
        Task PipelineDelegate(HttpContext context, Func<Task> next);
    }
}
