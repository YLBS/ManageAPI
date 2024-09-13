using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Tool
{
    public static class LogConfig
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="context">内容</param>
        /// <param name="configName">文件名</param>
        /// <returns></returns>
        public static string TestSetConfig(string context, string configName)
        {
            try
            {
                string time2 = DateTime.Now.ToString("yyyy-MM-dd");
                string logPath = "log/" + time2 + configName + ".txt";
                Directory.CreateDirectory(Path.GetDirectoryName(logPath)); // 确保目录存在
                using (StreamWriter sw = File.AppendText(logPath))
                {
                    sw.WriteLine("\n   " + DateTime.Now + "\n" + context + "\n");
                    sw.Flush();
                    sw.Close();
                }
                return "写入成功：";
            }
            catch (Exception e)
            {
                return "写入失败：" + e.Message;
            }
        }
    }
}
