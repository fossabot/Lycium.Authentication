using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Lycium.Authentication.Controllers
{

    [LyciumApi]
    public class PassController : ControllerBase
    {
        public string JsonResult<T>(T instance)
        {
            return JsonSerializer.Serialize(instance, LyciumConfiguration.JsonOption);
        }
    }

}
