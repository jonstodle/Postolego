using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketInterface {
    public class Pocket {
        private const string RequestUri = "https://getpocket.com/v3/oauth/request";
        private const string AuthorizeUri = "https://getpocket.com/auth/authorize?request_token={0}&redirect_uri={1}";

        private string ConsumerKey;
        private string RequestToken;
        private string AccessToken;

        public bool IsAuthenticated { get { return ConsumerKey != null && AccessToken != null; } }
        public string Username { private set; get; }

        public Pocket(string ConsumerKey) {
            if(string.IsNullOrWhiteSpace(ConsumerKey)) {
                throw new ArgumentException("The Consumer Key has to consist of letters and numbers");
            }
            this.ConsumerKey = ConsumerKey;
        }

        public async string GenerateUserLoginUriString(string ReturnUri) {
            if(string.IsNullOrWhiteSpace(ReturnUri)) {
                throw new ArgumentException("The Return Uri has to be a non-empty string");
            }

            var request = PocketWebRequest.CreatePocketHttp(RequestUri);
            using(var stream = await request.GetRequestStreamAsync()) {
                var requestData = Encoding.UTF8.GetBytes(new JObject(new JProperty("consumer_key", ConsumerKey), new JProperty("redirect_uri", "postolego:")).ToString());
                await stream.WriteAsync(requestData, 0, requestData.Length);
            }

            var response = await request.GetHttpResponseAsync();
            using(var stream = response.GetResponseStream())
            using(var reader = new StreamReader(stream)) {
                RequestToken = JObject.Parse(await reader.ReadToEndAsync())["code"].ToString();
            }
            return string.Format(AuthorizeUri, RequestToken, "postolego:AuthResponse");
        }

        public void FinalizeUserLogin() {

        }
    }
}
