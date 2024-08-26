using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.ConfigOptions
{
    public  class ConnectionConfig
    {
        private static string? _goodJobConnection;
        private static string? _goodJobInfoConnection;
        private static string? _goodJobOtherConnection;
        private static string? _goodJobReSumConnection;
        private static string? _goodBossConnection;
        private readonly IConfiguration _configuration;

        public ConnectionConfig(IConfiguration configuration)
        {
            _configuration = configuration;
            _goodJobConnection = _configuration["ConnectionStrings:GoodJobConnection"];
            _goodJobInfoConnection = _configuration["ConnectionStrings:GoodJobInfoConnection"];
            _goodJobOtherConnection = _configuration["ConnectionStrings:GoodJobOtherConnection"];
            _goodJobReSumConnection = _configuration["ConnectionStrings:GoodJobReSumConnection"];
            _goodBossConnection = _configuration["ConnectionStrings:GoodBossConnection"];
        }

        public static string GoodJobConnection
        {
            get {
                if (_goodJobConnection == null)
                {
                    return "";
                }

                return _goodJobConnection;
            }
        }
        public static string GoodJobInfoConnection
        {
            get
            {
                if (_goodJobInfoConnection == null)
                {
                    return "";
                }

                return _goodJobInfoConnection;
            }
        }
        public static string GoodJobOtherConnection
        {
            get
            {
                if (_goodJobOtherConnection == null)
                {
                    return "";
                }

                return _goodJobOtherConnection;
            }
        }
        public static string GoodJobReSumConnection
        {
            get
            {
                if (_goodJobReSumConnection == null)
                {
                    return "";
                }

                return _goodJobReSumConnection;
            }
        }
        public static string GoodBossConnection
        {
            get
            {
                if (_goodBossConnection == null)
                {
                    return "";
                }

                return _goodBossConnection;
            }
        }
    }
}
