using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RestSharp;

namespace cs_api_dotnet
{
    public static class RestSharpExtensions
    {
        public static IRestResponse<T> HandleResponse<T>(this IRestResponse<T> response)
        {
            var type = typeof (T);
            if (response.ErrorException != null || response.StatusCode != HttpStatusCode.OK)
                throw new HttpException((int)response.StatusCode, "Error retrieving " + type.Name, response.ErrorException);
      
            return response;
        }

        public static IRestResponse HandleResponse(this IRestResponse response)
        {
            
            if (response.ErrorException != null || response.StatusCode != HttpStatusCode.OK)
                throw new HttpException((int)response.StatusCode, "Error retrieving resource", response.ErrorException);
        
            return response;
        }


    }
}
