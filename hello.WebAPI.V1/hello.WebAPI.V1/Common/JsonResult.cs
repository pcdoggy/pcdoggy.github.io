using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace hello.WebAPI.V1.Common
{
    // 接口返回信息
    public class JsonResult
    {
        string value;
        HttpRequestMessage request;

        public JsonResult(string value, HttpRequestMessage request)
        {
            this.value = value;
            this.request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage()
            {
                Content = new StringContent(this.value, Encoding.UTF8, "application/json"),
                RequestMessage = this.request
            };
            return Task.FromResult(response);
        }
    }
}