using Microsoft.Extensions.Caching.Memory;

namespace ManageNew.CacheManageTool
{
    /// <summary>
    /// 缓存管理
    /// </summary>
    public class MemoryCacheMange
    {

        private readonly IMemoryCache _cache;
        public MemoryCacheMange(IMemoryCache cache)
        {
            _cache = cache;
        }

    }
}
