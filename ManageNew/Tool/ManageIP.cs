using Nancy;
using System.Net;

namespace ManageNew.Tool
{
    /// <summary>
    /// 管理跟IP有关的
    /// </summary>
    public static class ManageIP
    {
        /// <summary>
        /// 获取本地IP，其实还是服务器的Ip。。。。 
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string ip = string.Empty;
            IPAddress[] addressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            for (int i = 0; i < addressList.Length; i++)
            {
                ip = addressList[i].ToString();
            }
            return ip;
        }
    }
}
