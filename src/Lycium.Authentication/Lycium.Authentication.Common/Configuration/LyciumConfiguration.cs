using Microsoft.AspNetCore.Http;
using System.Text.Json;

public static class LyciumConfiguration
{
    public static readonly JsonSerializerOptions JsonOption;
    static LyciumConfiguration()
    {
        JsonOption = new JsonSerializerOptions() { PropertyNamingPolicy = null };
    }
    public const string SECRET_KEY = "SecretKey";
    public const string HOST_TOKEN = "HostToken";
    public const string USER_UID = "Uid";
    public const string USER_GID = "Gid";
    public const string USER_TOKEN = "Token";

    public static async void ReturnMessage(HttpContext context, int code, string message)
    {

        if (context != null)
        {
            var result = JsonSerializer.Serialize(
            new
            {
                code,
                msg = message

            }, JsonOption);
            await context.Response.WriteAsync(result);
        }

    }

}
