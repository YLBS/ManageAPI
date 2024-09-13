using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System.Collections;
namespace Commons.Helper
{
    public enum TempletType
    {
        /// <summary>
        /// 简历显示模版
        /// </summary>
        ResumeTemplet = 1,

        /// <summary>
        /// 英文简历显示模版
        /// </summary>
        ResumeEnTemplet = 2,

        /// <summary>
        /// 职位显示模版
        /// </summary>
        PositionTemplet = 3,

        /// <summary>
        /// 评语模版
        /// </summary>
        RemarkTemplet = 4,

        /// <summary>
        /// 企业预览职位模版
        /// </summary>
        PositionViewTemplet = 5,
        /// <summary>
        /// 职位显示旧模版
        /// </summary>
        OldPositionTemplet = 6,
        /// <summary>
        /// 找工作职位显示模版
        /// </summary>
        JobPositionTemplet = 7,
        /// <summary>
        /// 最新简历显示模版
        /// </summary>
        SendDResumeTemplet = 8


    }
    public class TempletHelper
    {
        private readonly IMemoryCache _cache;
        

        public TempletHelper(IMemoryCache cache)
        {
            _cache = cache;
        }
        
        public string GetTemplet(TempletType templetType)
        {
            Hashtable table = GetResource(templetType);
            if (table != null)
            {
                if (table.ContainsKey((int)templetType))
                    return table[(int)templetType].ToString();
                return string.Empty;
            }
            else
                return string.Empty;
        }

        private  Hashtable GetResource(TempletType templetType)
        {
            Hashtable resources;
            string cacheKey = "templet" + (int)templetType;
            if (!_cache.TryGetValue(cacheKey, out Hashtable result))
            {
                resources = new Hashtable();
                var h = LoadResource(resources, cacheKey, templetType);
                _cache.Set(cacheKey, h, DateTime.Now.AddMonths(1));
                return h;
            }

            return result;
        }

        private  static Hashtable LoadResource(Hashtable target, string cacheKey, TempletType templetType)
        {
            string Path = AppSet.TempletPath;
            string filePath = string.Empty;
            switch (templetType)
            {
                case TempletType.ResumeTemplet:
                    filePath = Path + "Resume.html";
                    break;
                case TempletType.ResumeEnTemplet:
                    filePath = Path + "ResumeEn.html";
                    break;
                case TempletType.PositionTemplet:
                    filePath = Path + "Position.html";
                    break;
                case TempletType.RemarkTemplet:
                    filePath = Path + "Remark.html";
                    break;
                case TempletType.PositionViewTemplet:
                    filePath = Path + "PositionView.html";
                    break;
                case TempletType.OldPositionTemplet:
                    filePath = Path + "OldPosition.html";
                    break;
                case TempletType.JobPositionTemplet:
                    filePath = Path + "JobPosition.html";
                    break;
                case TempletType.SendDResumeTemplet:
                    filePath = Path + "JobPosition.html";
                    break;
                default:
                    throw new ApplicationException("没有这个模版类别");
            }
            string templet = FileHelper.GetFileText(filePath);
            target[(int)templetType] = templet;
            return target;
        }
    }
}
