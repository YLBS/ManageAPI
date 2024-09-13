namespace Commons.ConfigOptions
{
    public class CorsOptions
    {
        public string Name { get; set; }
        /// <summary>
        /// 是否允许所有跨域
        /// </summary>
        public bool EnableAll { get; set; }
        /// <summary>
        /// 跨域的IP
        /// </summary>
        public List<Policy> Policy { get; set; }
    }

    public class Policy
    {
        public string Name { get; set; }
        /// <summary>
        /// Ip域名
        /// </summary>
        public string Domain { get; set; }
    }
}
