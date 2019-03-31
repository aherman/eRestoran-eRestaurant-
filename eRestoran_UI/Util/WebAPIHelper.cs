using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using eRestoran_API.Models;

namespace eRestoran_UI.Util
{
    public class WebAPIHelper
    {
         private HttpClient client { get; set; }
         private string route { get; set; }

         public WebAPIHelper(string uri, string route)
         {
             client = new HttpClient();
             client.BaseAddress = new Uri(uri);
             this.route = route;

         }
        public  HttpResponseMessage GetResponse(string parameter = "")
        {
            return client.GetAsync(route + "/" + parameter).Result;
        }

        public HttpResponseMessage GetActionResponse(string action, string parameter = "")
        {
            return client.GetAsync(route + "/" + action + "/" + parameter).Result;
        }

        public HttpResponseMessage GetActionResponse2(string action, string parameter1 = "", string parameter2 = "")
        {
            return client.GetAsync(route + "/" + action + "/" + parameter1 + "/" + parameter2).Result;
        }

        public HttpResponseMessage PostResponse(Object newObject)
        {
             return client.PostAsJsonAsync(route, newObject).Result;
        }

        public HttpResponseMessage PostActionResponse(string action, Object newObject)
        {
            return client.PostAsJsonAsync(route + "/" + action, newObject).Result;
        }

        public HttpResponseMessage PutResponse(int id, Object existingObject)
        {
            return client.PutAsJsonAsync(route +"/" + id, existingObject).Result;
        }

        public HttpResponseMessage DeleteResponse(string action, int id)
        {
            return client.DeleteAsync(route + "/" + action + "/" + id).Result;
        }
    }
}
