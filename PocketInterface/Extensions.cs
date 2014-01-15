using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PocketInterface {
    public static class Extensions {
        public static HttpWebRequest MakePocketRequest(this HttpWebRequest hwr) {
            hwr.Method = "POST";
            hwr.ContentType = "application/json; charset=UTF8";
            hwr.Headers["X-Accept"] = "application/json";
            return hwr;
        }

        public static async Task<Stream> GetRequestStreamAsync(this HttpWebRequest hwr) {
            return await Task<Stream>.Factory.FromAsync(hwr.BeginGetRequestStream, hwr.EndGetRequestStream, null);
        }

        public static async Task<HttpWebResponse> GetHttpResponseAsync(this HttpWebRequest hwr) {
            return (await Task<WebResponse>.Factory.FromAsync(hwr.BeginGetResponse, hwr.EndGetResponse, null)) as HttpWebResponse;
        }
    }
}
