using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PocketInterface {
    public class PocketAddItem {
        public enum Encoding { Encode, NotEncode }

        private string _consumerKey;
        private string _accessToken;
        private string _url;
        private string _title;
        private string _tags;
        private string _tweetId;

        public PocketAddItem(string ConsumerKey, string AccessToken, string Url, string Title = null, string Tags = null, string TweetId = null) {
            _consumerKey = ConsumerKey;
            _accessToken = AccessToken;
            _url = Url;
            _title = Title;
            _tags = Tags;
            _tweetId = TweetId;
        }

        public string GetJsonString() {
            var json = new JObject();
            json.Add("url", _url);
            if(_title != null)      json.Add("title", _title);
            if(_tags != null)       json.Add("tags", _tags);
            if(_tweetId != null)    json.Add("tweet_id", _tweetId);
            json.Add("consumer_key", _consumerKey);
            json.Add("access_token", _accessToken);
            return json.ToString();
        }

        public string GetJsonString(Encoding Encode) {
            var json = new JObject();
            if(Encode == Encoding.Encode) json.Add("url", WebUtility.UrlEncode(_url)); else json.Add("url", _url);
            if(_title != null) json.Add("title", _title);
            if(_tags != null) json.Add("tags", _tags);
            if(_tweetId != null) json.Add("tweet_id", _tweetId);
            json.Add("consumer_key", _consumerKey);
            json.Add("access_token", _accessToken);
            return json.ToString();
        }
    }
}
