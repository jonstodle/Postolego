using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PocketInterface {
    public class PocketWebRequest : HttpWebRequest {
        public static PocketWebRequest CreatePocketHttp(string requestUriString) {
            var request = CreateHttp(requestUriString) as PocketWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json; charset=UTF8";
            request.Headers["X-Accept"] = "application/json";
            return request;
        }

        public async Task<Stream> GetRequestStreamAsync() {
            return await Task<Stream>.Factory.FromAsync(BeginGetRequestStream, EndGetRequestStream, this);
        }

        public async Task<HttpWebResponse> GetHttpResponseAsync() {
            return (await Task<WebResponse>.Factory.FromAsync(BeginGetResponse, EndGetResponse, this)) as HttpWebResponse;
        }
    }
}
