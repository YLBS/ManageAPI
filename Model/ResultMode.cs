using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.enums;

namespace Model
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class ResultMode<T>
    {
        public ResponseCode Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResultMode(ResponseCode code, string message, T data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public static ResultMode<T> Success(T data, string message = "Success")
        {
            return new ResultMode<T>(ResponseCode.Success, message, data);
        }

        public static ResultMode<T> Failed(string message = "Failed")
        {
            return new ResultMode<T>(ResponseCode.Failed, message, default);
        }
        public static ResultMode<T> Failed500(string message = "Failed")
        {
            return new ResultMode<T>(ResponseCode.Failed, message, default);
        }
        public static ResultMode<T> NotFound(string message = "Not Found")
        {
            return new ResultMode<T>(ResponseCode.NotFound, message, default);
        }
    }
}
