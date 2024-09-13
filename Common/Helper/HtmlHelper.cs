using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Commons.Helper
{
    public class HtmlHelper
    {
        private readonly HtmlEncoder _htmlEncoder;

        public HtmlHelper(HtmlEncoder htmlEncoder)
        {
            _htmlEncoder = htmlEncoder;
        }
        /// <summary>
        /// 格式化输出到页面的字符串，包括转换回车符
        /// </summary>
        /// <param name="htmlstr">要格式化的字符串</param>
        /// <param name="replace">是否替换换行符</param>
        /// <returns>格式化后的字符串</returns>
        public string FormatHtmlString(string htmlstr, bool replace)
        {
            if (string.IsNullOrEmpty(htmlstr)) return "";

            //原来的   htmlstr = HttpContext.Current.Server.HtmlEncode(htmlstr); 已被淘汰
            htmlstr = _htmlEncoder.Encode(htmlstr);
            if (replace) { htmlstr = htmlstr.Replace("\r\n", "<br />"); }
            return htmlstr;
        }
    }
}
