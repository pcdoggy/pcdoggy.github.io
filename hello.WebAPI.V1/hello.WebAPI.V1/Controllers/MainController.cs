using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace hello.WebAPI.V1.Controllers
{
    public class MainController : ApiController
    {
        // API请求入口
        [HttpPost]
        public async void DatasInterface(dynamic obj)
        {
            try
            {
                var streamProvider = await Request.Content.ReadAsMultipartAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
