using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace egg.Mvc {
    /// <summary>
    /// 实用工具集
    /// </summary>
    public class Utilities {
        /// <summary>
        /// 获取提交数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetRequestData(HttpRequest request) {
            byte[] buffer = new byte[4096];
            List<byte> ls = new List<byte>();
            int res = 0;
            do {
                res = request.Body.Read(buffer, 0, buffer.Length);
                if (res > 0) {
                    ls.AddRange(new ArraySegment<byte>(buffer, 0, res));
                }
            } while (res > 0);
            return System.Text.Encoding.UTF8.GetString(ls.ToArray());
        }
        /// <summary>
        /// 获取提交Jttp
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Jttp.JttpRequest GetRequestJttp(HttpRequest request) {
            return new Jttp.JttpRequest(GetRequestData(request));
        }
    }
}
