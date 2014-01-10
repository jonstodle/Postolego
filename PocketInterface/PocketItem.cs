using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketInterface {
    public class PocketItem {
        private int _itemId;
        private int _resolvedId;
        private string _givenUrl;
        private string _resolvedUrl;
        private int _favorite;
        private int _status;
        private string _givenTitle;
        private string _resolvedTitle;
        private string _excerpt;
        private int _isArticle;
        private int _hasVideo;
        private int _hasImage;
        private int _wordCount;
        private object _tags;
        private object _authors;
        private object _images;
        private object _videos;

        [JsonProperty(PropertyName = "item_id")]
        public int ItemId {
            get {
                return _itemId;
            }

            set {
                _itemId = value;
            }
        }

        [JsonProperty(PropertyName = "resolved_id")]
        public int ResolvedId {
            get {
                return _resolvedId;
            }

            set {
                _resolvedId = value;
            }
        }

        [JsonProperty(PropertyName = "given_url")]
        public string GivenUrl {
            get {
                return _givenUrl;
            }

            set {
                _givenUrl = value;
            }
        }

        [JsonProperty(PropertyName = "resolved_url")]
        public string ResolvedUrl {
            get {
                return _resolvedUrl;
            }

            set {
                _resolvedUrl = value;
            }
        }

        [JsonProperty(PropertyName = "favorite")]
        public int Favorite {
            get {
                return _favorite;
            }

            set {
                _favorite = value;
            }
        }

        [JsonProperty(PropertyName = "status")]
        public int Status {
            get {
                return _status;
            }

            set {
                _status = value;
            }
        }

        [JsonProperty(PropertyName = "given_title")]
        public string GivenTitle {
            get {
                if(_givenTitle == null || _givenTitle.Length == 0) {
                    return _resolvedTitle;
                } else {
                    return _givenTitle;
                }
            }

            set {
                _givenTitle = value;
            }
        }

        [JsonProperty(PropertyName = "resolved_title")]
        public string ResolvedTitle {
            get {
                return _resolvedTitle;
            }

            set {
                _resolvedTitle = value;
            }
        }

        [JsonProperty(PropertyName = "excerpt")]
        public string Excerpt {
            get {
                return _excerpt;
            }

            set {
                _excerpt = value;
            }
        }

        [JsonProperty(PropertyName = "is_article")]
        public int IsArticle {
            get {
                return _isArticle;
            }

            set {
                _isArticle = value;
            }
        }

        [JsonProperty(PropertyName = "has_video")]
        public int HasVideo {
            get {
                return _hasVideo;
            }

            set {
                _hasVideo = value;
            }
        }

        [JsonProperty(PropertyName = "has_image")]
        public int HasImage {
            get {
                return _hasImage;
            }

            set {
                _hasImage = value;
            }
        }

        [JsonProperty(PropertyName = "word_count")]
        public int WordCount {
            get {
                return _wordCount;
            }

            set {
                _wordCount = value;
            }
        }

        [JsonProperty(PropertyName = "tags")]
        [JsonIgnore]
        public object Tags {
            get {
                return _tags;
            }

            set {
                _tags = value;
            }
        }

        [JsonProperty(PropertyName = "authors")]
        [JsonIgnore]
        public object Authors {
            get {
                return _authors;
            }

            set {
                _authors = value;
            }
        }

        [JsonProperty(PropertyName = "images")]
        [JsonIgnore]
        public object Images {
            get {
                return _images;
            }

            set {
                _images = value;
            }
        }

        [JsonProperty(PropertyName = "videos")]
        [JsonIgnore]
        public object Videos {
            get {
                return _videos;
            }

            set {
                _videos = value;
            }
        }
    }
}
