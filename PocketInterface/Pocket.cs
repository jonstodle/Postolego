using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PocketInterface {
    public class Pocket {
        private const string RequestUri = "https://getpocket.com/v3/oauth/request";
        private const string LoginUri = "https://getpocket.com/auth/authorize?request_token={0}&redirect_uri={1}";
        private const string AuthorizeUri = "https://getpocket.com/v3/oauth/authorize";
        private const string GetUri = "https://getpocket.com/v3/get";

        //private string ConsumerKey;
        //private string RequestToken;
        //private string AccessToken;
        //private string LoginUriString;

        public string ConsumerKey;
        public string RequestToken;
        public string AccessToken;
        public string LoginUriString;

        public bool IsAuthenticated { get { return ConsumerKey != null && AccessToken != null; } }
        public bool HasLoginUriString { get { return LoginUriString != null; } }
        public string Username { private set; get; }
        public string TimeStamp { private set; get; }

        public Pocket() { }

        public Pocket(string ConsumerKey) {
            if(string.IsNullOrWhiteSpace(ConsumerKey)) {
                throw new ArgumentException("The Consumer Key has to consist of letters and numbers");
            }
            this.ConsumerKey = ConsumerKey;
        }

        #region Authorization
        public async Task<string> GenerateUserLoginUriString(string ReturnUri) {
            if(string.IsNullOrWhiteSpace(ReturnUri)) {
                throw new ArgumentException("The Return Uri has to be a non-empty string");
            }

            if(!HasLoginUriString) {
                var request = HttpWebRequest.CreateHttp(RequestUri).MakePocketRequest();
                using(var stream = await request.GetRequestStreamAsync()) {
                    var requestData = Encoding.UTF8.GetBytes(new JObject(new JProperty("consumer_key", ConsumerKey), new JProperty("redirect_uri", ReturnUri)).ToString());
                    await stream.WriteAsync(requestData, 0, requestData.Length);
                }

                var response = await request.GetHttpResponseAsync();
                using(var stream = response.GetResponseStream())
                using(var reader = new StreamReader(stream)) {
                    RequestToken = JObject.Parse(await reader.ReadToEndAsync())["code"].ToString();
                }
                LoginUriString = string.Format(LoginUri, RequestToken, ReturnUri);
            }
            return LoginUriString;
        }

        public async Task FinalizeUserLogin() {
            if(!HasLoginUriString) {
                throw new Exception("You have to make the user log in first");
            }

            var request = HttpWebRequest.CreateHttp(AuthorizeUri).MakePocketRequest();
            using(var stream = await request.GetRequestStreamAsync()) {
                var requestData = Encoding.UTF8.GetBytes(new JObject(new JProperty("consumer_key", ConsumerKey), new JProperty("code", RequestToken)).ToString());
                await stream.WriteAsync(requestData, 0, requestData.Length);
            }

            var response = await request.GetHttpResponseAsync();
            using(var stream = response.GetResponseStream())
            using(var reader = new StreamReader(stream)) {
                var json = JObject.Parse(await reader.ReadToEndAsync());
                AccessToken = json["access_token"].ToString();
                Username = json["username"].ToString();
            }
        }
        #endregion

        #region Retrieve Items
        public async Task<List<PocketItem>> RetrieveItems(PocketRetrieveItem.States State = PocketRetrieveItem.States.Unread, PocketRetrieveItem.Favorites Favorite = PocketRetrieveItem.Favorites.Both, string Tag = null, PocketRetrieveItem.ContentTypes ContentType = PocketRetrieveItem.ContentTypes.All, PocketRetrieveItem.Sorts Sort = PocketRetrieveItem.Sorts.NoSort, PocketRetrieveItem.DetailTypes DetailType = PocketRetrieveItem.DetailTypes.NoType, string Domain = null, string Since = null, int Count = -1, int Offset = -1) {
            var retrieveObject = new PocketRetrieveItem(ConsumerKey, AccessToken, State, Favorite, Tag, ContentType, Sort, DetailType, Domain: Domain, Since: Since, Count: Count, Offset: Offset);
            var request = PocketWebRequest.CreatePocketHttp(GetUri);
            using(var stream = await request.GetRequestStreamAsync()) {
                var retrieveData = Encoding.UTF8.GetBytes(retrieveObject.GetJsonString());
                await stream.WriteAsync(retrieveData, 0, retrieveData.Length);
            }

            var response = await request.GetHttpResponseAsync();
            JObject responseData;
            using(var stream = response.GetResponseStream())
            using(var reader = new StreamReader(stream)) {
                responseData = JObject.Parse(await reader.ReadToEndAsync());
            }
            var list = responseData["list"] as JObject;
            if(list != null) {
                var returnList = new List<PocketItem>();
                foreach(var i in list) {
                    returnList.Add(await JsonConvert.DeserializeObjectAsync<PocketItem>(i.ToString()));
                }
                return returnList;
            } else {
                return null;
            }
        }

        public async Task<List<PocketItem>> SearchItems(string Search, PocketRetrieveItem.States State = PocketRetrieveItem.States.Unread, PocketRetrieveItem.Favorites Favorite = PocketRetrieveItem.Favorites.Both, string Tag = null, PocketRetrieveItem.ContentTypes ContentType = PocketRetrieveItem.ContentTypes.All, PocketRetrieveItem.Sorts Sort = PocketRetrieveItem.Sorts.NoSort, PocketRetrieveItem.DetailTypes DetailType = PocketRetrieveItem.DetailTypes.NoType, string Domain = null, string Since = null, int Count = -1, int Offset = -1) {
            var retrieveObject = new PocketRetrieveItem(ConsumerKey, AccessToken, State, Favorite, Tag, ContentType, Sort, DetailType, Search, Domain, Since, Count, Offset);
            var request = PocketWebRequest.CreatePocketHttp(GetUri);
            using(var stream = await request.GetRequestStreamAsync()) {
                var retrieveData = Encoding.UTF8.GetBytes(retrieveObject.GetJsonString());
                await stream.WriteAsync(retrieveData, 0, retrieveData.Length);
            }

            var response = await request.GetHttpResponseAsync();
            JObject responseData;
            using(var stream = response.GetResponseStream())
            using(var reader = new StreamReader(stream)) {
                responseData = JObject.Parse(await reader.ReadToEndAsync());
            }
            var list = responseData["list"] as JObject;
            if(list != null) {
                var returnList = new List<PocketItem>();
                foreach(var i in list) {
                    returnList.Add(await JsonConvert.DeserializeObjectAsync<PocketItem>(i.ToString()));
                }
                return returnList;
            } else {
                return null;
            }
        }
        #endregion
    }
}