using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace PocketInterface {
    public class PocketRetrieveItem {
        public enum Encoding { Encode, NotEncode }
        public enum States { Unread, Archive, All }
        public enum Favorites { Both, Favorited, NotFavorited }
        public enum ContentTypes {All, Article, Video, Image }
        public enum Sorts {NoSort, Newest, Oldest, Title, Site }
        public enum DetailTypes { NoType, Simple, Complete }

        private string _consumerKey;
        private string _accessToken;
        private States _state;
        private Favorites _favorite;
        private string _tag;
        private ContentTypes _contentType;
        private Sorts _sort;
        private DetailTypes _detailType;
        private string _search;
        private string _domain;
        private string _since;
        private int _count;
        private int _offset;

        public PocketRetrieveItem(string ConsumerKey, string AccessToken, States State = States.Unread, Favorites Favorite = Favorites.Both, string Tag = null, ContentTypes ContentType = ContentTypes.All, Sorts Sort = Sorts.NoSort, DetailTypes DetailType = DetailTypes.NoType, string Search = null, string Domain = null, string Since = null, int Count = -1, int Offset = -1) {
            _consumerKey = ConsumerKey;
            _accessToken = AccessToken;
            _state = State;
            _favorite = Favorite;
            _tag = Tag;
            _contentType = ContentType;
            _sort = Sort;
            _detailType = DetailType;
            _search = Search;
            _domain = Domain;
            _since = Since;
            _count = Count;
            _offset = Offset;
        }

        public string GetJsonString() {
            string state;
            switch(_state) {
                case States.Unread:
                    state = "unread";
                    break;
                case States.Archive:
                    state = "archive";
                    break;
                case States.All:
                    state = "all";
                    break;
                default:
                    state = "unread";
                    break;
            }

            int favorite;
            switch(_favorite) {
                case Favorites.Both:
                    favorite = -1;
                    break;
                case Favorites.Favorited:
                    favorite = 1;
                    break;
                case Favorites.NotFavorited:
                    favorite = 0;
                    break;
                default:
                    favorite = -1;
                    break;
            }

            string contentType;
            switch(_contentType) {
                case ContentTypes.All:
                    contentType = null;
                    break;
                case ContentTypes.Article:
                    contentType = "article";
                    break;
                case ContentTypes.Video:
                    contentType = "video";
                    break;
                case ContentTypes.Image:
                    contentType = "image";
                    break;
                default:
                    contentType = null;
                    break;
            }

            string sort;
            switch(_sort) {
                case Sorts.NoSort:
                    sort = null;
                    break;
                case Sorts.Newest:
                    sort = "newest";
                    break;
                case Sorts.Oldest:
                    sort = "oldest";
                    break;
                case Sorts.Title:
                    sort = "title";
                    break;
                case Sorts.Site:
                    sort = "site";
                    break;
                default:
                    sort = null;
                    break;
            }

            string detailType;
            switch(_detailType) {
                case DetailTypes.NoType:
                    detailType = null;
                    break;
                case DetailTypes.Simple:
                    detailType = "simple";
                    break;
                case DetailTypes.Complete:
                    detailType = "complete";
                    break;
                default:
                    detailType = null;
                    break;
            }

            var json = new JObject();
            json.Add("consumer_key", _consumerKey);
            json.Add("access_token", _accessToken);
            json.Add("state", state);
            if(favorite > -1) json.Add("favorite", favorite);
            if(_tag != null) json.Add("tag", _tag);
            if(contentType != null) json.Add("contentType", contentType);
            if(sort != null) json.Add("sort", sort);
            if(detailType != null) json.Add("detailType", detailType);
            if(_search != null) json.Add("search", _search);
            if(_domain != null) json.Add("domain", _domain);
            if(_since != null) json.Add("since", _since);
            if(_count > -1 ) json.Add("count", _count);
            if(_offset > -1) json.Add("offset", _offset);

            return json.ToString();
        }

        public string GetJsonString(Encoding Encode) {
            string state;
            switch(_state) {
                case States.Unread:
                    state = "unread";
                    break;
                case States.Archive:
                    state = "archive";
                    break;
                case States.All:
                    state = "all";
                    break;
                default:
                    state = "unread";
                    break;
            }

            int favorite;
            switch(_favorite) {
                case Favorites.Both:
                    favorite = -1;
                    break;
                case Favorites.Favorited:
                    favorite = 1;
                    break;
                case Favorites.NotFavorited:
                    favorite = 0;
                    break;
                default:
                    favorite = -1;
                    break;
            }

            string contentType;
            switch(_contentType) {
                case ContentTypes.All:
                    contentType = null;
                    break;
                case ContentTypes.Article:
                    contentType = "article";
                    break;
                case ContentTypes.Video:
                    contentType = "video";
                    break;
                case ContentTypes.Image:
                    contentType = "image";
                    break;
                default:
                    contentType = null;
                    break;
            }

            string sort;
            switch(_sort) {
                case Sorts.NoSort:
                    sort = null;
                    break;
                case Sorts.Newest:
                    sort = "newest";
                    break;
                case Sorts.Oldest:
                    sort = "oldest";
                    break;
                case Sorts.Title:
                    sort = "title";
                    break;
                case Sorts.Site:
                    sort = "site";
                    break;
                default:
                    sort = null;
                    break;
            }

            string detailType;
            switch(_detailType) {
                case DetailTypes.NoType:
                    detailType = null;
                    break;
                case DetailTypes.Simple:
                    detailType = "simple";
                    break;
                case DetailTypes.Complete:
                    detailType = "complete";
                    break;
                default:
                    detailType = null;
                    break;
            }

            var json = new JObject();
            json.Add("consumer_key", _consumerKey);
            json.Add("access_token", _accessToken);
            json.Add("state", state);
            if(favorite > -1) json.Add("favorite", favorite);
            if(_tag != null) json.Add("tag", _tag);
            if(contentType != null) json.Add("contentType", contentType);
            if(sort != null) json.Add("sort", sort);
            if(detailType != null) json.Add("detailType", detailType);
            if(_search != null) json.Add("search", _search);
            if(_domain != null) if(Encode == Encoding.Encode) json.Add("domain", WebUtility.UrlEncode(_domain)); else json.Add("domain", _domain);
            if(_since != null) json.Add("since", _since);
            if(_count > -1) json.Add("count", _count);
            if(_offset > -1) json.Add("offset", _offset);

            return json.ToString();
        }
    }
}
