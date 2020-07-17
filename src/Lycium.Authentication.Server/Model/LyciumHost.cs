namespace Lycium.Authentication.Common
{
    public class LyciumHost 
    {
        public long Id { get; set; }
        /// <summary>
        /// 客户端标识
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 客户端URL
        /// </summary>
        public string HostUrl { get; set; }

        /// <summary>
        /// 客户端错误处理时重定向的登录页面
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// 客户端与服务端通信的Key
        /// </summary>
        public string HostToken { get; set; }

        /// <summary>
        /// Key存活时长
        /// </summary>
        public long TokenAliveTime { get; set; }

        /// <summary>
        /// Key创建时间
        /// </summary>
        public long TokenCreateTime { get; set; }

        /// <summary>
        /// 使用的配置名称
        /// </summary>
        public long ConfigId { get; set; }
    }
}
