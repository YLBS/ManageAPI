using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Helper
{
    /// <summary>
    /// Goodjob File 助手类
    /// </summary>
    public class FileHelper
    {
        /// <summary>
        /// 获取文本内容
        /// </summary>
        /// <param name="virtualPath"> 传完整路径</param>
        /// <returns></returns>
        public static string GetFileText(string virtualPath)
        {
            /*
             //旧的 HttpContext.Current.Server.MapPath已被淘汰
             StreamReader sr = null;
            try
            {
                sr = new StreamReader(HttpContext.Current.Server.MapPath(virtualPath));
            }
            catch
            {
                sr = new StreamReader(virtualPath);

            }
            string strOut = sr.ReadToEnd();
            sr.Close();
            return strOut;
            
             */


            StreamReader sr = new StreamReader(virtualPath);
            string strOut = sr.ReadToEnd();
            sr.Close();
            return strOut;
        }
    }
}
