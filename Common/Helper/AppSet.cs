using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commons.Helper
{
    public class AppSet
    {
        private readonly IConfiguration _configuration;
        //获取公共资源文件存放目录
        private static string? _ResourcesPath;

        public AppSet(IConfiguration configuration)
        {
            _configuration = configuration;
            _ResourcesPath = _configuration["ResourcesPath"];
        }

        /// <summary>
        /// 获取模版目录路径
        /// </summary>
        public static string TempletPath => _ResourcesPath+ "Template\\";
        /// <summary>
        ///  获取XML文件目录路径
        /// </summary>

        public static string ResourcesPath=> _ResourcesPath + "XmlFile\\";
    }
}
