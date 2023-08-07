using System;
using System.Collections.Specialized;
using System.IO;
using Newtonsoft.Json.Linq;
using MultiThreadedDownloaderLib;
using static Twitch_watchman.Utils;

namespace Twitch_watchman
{
    public sealed class TwitchApi
    {
        public static string TWITCH_CLIENT_ID = "gs7pui3law5lsi69yzi9qzyaqvlcsy";

        /// <summary>
        /// Used for hidden / GQL API requests.
        /// </summary>
        public const string TWITCH_CLIENT_ID_PRIVATE = "kimne78kx3ncx6brgo4mv6wki5h1ko";

        public const string TWITCH_HELIX_USERS_LOGIN_URL_TEMPLATE = "https://api.twitch.tv/helix/users?login={0}";

        /// <summary>
        /// WARNING!!! Do not use the TWITCH GQL API if you are signed in!!!
        /// </summary>
        public const string TWITCH_GQL_API_URL = "https://gql.twitch.tv/gql";

        private const string TWITCH_USHER_HLS_URL_TEMPLATE = "https://usher.ttvnw.net/api/channel/hls/{0}.m3u8";

        public TwitchHelixOauthToken HelixOauthToken { get; private set; } = new TwitchHelixOauthToken();

        public const int ERROR_USER_NOT_FOUND = -1000;
        public const int ERROR_USER_OFFLINE = -1001;

        public int GetHelixOauthToken(out string resToken)
        {
            int errorCode = HelixOauthToken.Update(TWITCH_CLIENT_ID);
            resToken = HelixOauthToken.AccessToken;
            return errorCode;
        }

        /// <summary>
        /// WARNING!!! Do not use this body if you are signed in!!!
        /// </summary>
        public static JObject GeneratePlaybackAccessTokenRequestBody(string userLogin)
        {
            const string queryString = "query PlaybackAccessToken_Template($login: String!, $isLive: Boolean!, " +
                "$vodID: ID!, $isVod: Boolean!, $playerType: String!) " +
                "{  streamPlaybackAccessToken(channelName: $login, params: " +
                "{platform: \"web\", playerBackend: \"mediaplayer\", playerType: $playerType}) " +
                "@include(if: $isLive) {    value    signature    __typename  }  " +
                "videoPlaybackAccessToken(id: $vodID, params: {platform: \"web\", playerBackend: " +
                "\"mediaplayer\", playerType: $playerType}) @include(if: $isVod) {    value    signature    __typename  }}";

            JObject jVariables = new JObject();
            jVariables["isLive"] = true;
            jVariables["login"] = userLogin;
            jVariables["isVod"] = false;
            jVariables["vodID"] = "";
            jVariables["playerType"] = "site";

            JObject json = new JObject();
            json["operationName"] = "PlaybackAccessToken_Template";
            json["query"] = queryString;
            json.Add(new JProperty("variables", jVariables));

            return json;
        }

        public static string GetUserInfoRequestUrl_Helix(string userLogin)
        {
            string url = string.Format(TWITCH_HELIX_USERS_LOGIN_URL_TEMPLATE, userLogin);
            return url;
        }

        public string GetUserIsLiveRequestUrl_Helix(string userId)
        {
            string url = "https://api.twitch.tv/helix/streams?user_id=" + userId;
            return url;
        }

        public int GetPlaybackAccessToken(string channelName, out string response)
        {
            JObject body = GeneratePlaybackAccessTokenRequestBody(channelName);

            /*
             * Заголовок "Device-ID" нужен для предотвращения показа рекламы в начале видео.
             * Если этого заголовка нет, твич вставляет 15-секундную рекламную вставку в начало каждого плейлиста.
             * TODO: Каким-то образом получить правильное значение "deviceId" из API.
             */
            Guid deviceId = Guid.NewGuid();

            NameValueCollection headers = new NameValueCollection();
            headers.Add("User-Agent", USER_AGENT);
            headers.Add("Host", "gql.twitch.tv");
            headers.Add("Client-ID", TWITCH_CLIENT_ID_PRIVATE);
            headers.Add("Content-Type", "text/plain; charset=UTF-8");
            headers.Add("Device-ID", deviceId.ToString());

            return HttpsPost(TWITCH_GQL_API_URL, body.ToString(), headers, out response);
        }

        public bool ParseChannelToken(string unparsedChannelToken, out string token, out string signature)
        {
            try
            {
                JObject json = JObject.Parse(unparsedChannelToken);
                JObject jToken = json.Value<JObject>("data")?.Value<JObject>("streamPlaybackAccessToken");
                if (jToken != null)
                {
                    token = Uri.EscapeDataString(jToken.Value<string>("value"));
                    signature = jToken.Value<string>("signature");
                    return true;
                }
            } catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            token = null;
            signature = null;
            return false;
        }

        public int GetLiveStreamManifestUrl(string channelName, out string manifestUrl)
        {
            int errorCode = GetPlaybackAccessToken(channelName, out string unparsedToken);
            if (errorCode == 200)
            {
                if (!ParseChannelToken(unparsedToken, out string token, out string signature))
                {
                    manifestUrl = null;
                    return 404;
                }

                int randomInt = new Random((int)DateTime.UtcNow.Ticks).Next(999999);

                NameValueCollection query = System.Web.HttpUtility.ParseQueryString(string.Empty);
                query.Add("acmb", "e30=");
                query.Add("allow_source", "true");
                query.Add("fast_bread", "true");
                query.Add("p", randomInt.ToString());
                query.Add("player_backend", "mediaplayer");
                query.Add("playlist_include_framerate", "true");
                query.Add("reassignments_supported", "true");
                query.Add("sig", signature);
                query.Add("supported_codecs", "avc1");
                query.Add("transcode_mode", "cbr_v1");
                query.Add("cdm", "wv");
                query.Add("player_version", "1.20.0");

                string usherUrl = string.Format(TWITCH_USHER_HLS_URL_TEMPLATE, channelName);
                manifestUrl = $"{usherUrl}?{query}&token={token}";
            }
            else
            {
                manifestUrl = null;
            }
            return errorCode;
        }

        public int GetUserInfo_Helix(string channelName,
            out TwitchUserInfo userInfo, out string errorMessage)
        {
            userInfo = null;
            if (string.IsNullOrEmpty(channelName) || string.IsNullOrWhiteSpace(channelName))
            {
                errorMessage = "Empty channel name";
                return 400;
            }

            string req = GetUserInfoRequestUrl_Helix(channelName.ToLower());
            int res = HttpsGet_Helix(req, out string buf);
            if (res == 200)
            {
                JObject json = JObject.Parse(buf);
                JArray ja = json.Value<JArray>("data");
                if (ja != null && ja.Count > 0)
                {
                    JObject j = ja[0].Value<JObject>();
                    userInfo = new TwitchUserInfo();
                    userInfo.DisplayName = j.Value<string>("display_name");
                    userInfo.Id = j.Value<string>("id");
                    errorMessage = null;
                }
                else
                {
                    errorMessage = buf;
                    return ERROR_USER_NOT_FOUND;
                }
            }
            else
            {
                errorMessage = buf;
            }
            return res;
        }

        public int IsUserLive_Helix(string userId, out string response)
        {
            response = null;
            string url = GetUserIsLiveRequestUrl_Helix(userId);
            int errorCode = HttpsGet_Helix(url, out string buffer);
            if (errorCode == 200)
            {
                JObject j = JObject.Parse(buffer);
                JArray ja = j.Value<JArray>("data");
                response = buffer;
                return ja.Count > 0 ? 200 : ERROR_USER_OFFLINE;
            }

            return errorCode;
        }

        public int HttpsGet_Helix(string url, out string response)
        {
            int errorCode = GetHelixOauthToken(out string token);
            if (errorCode == 200)
            {
                FileDownloader d = new FileDownloader() { Url = url };
                d.Headers.Add("Client-ID", TWITCH_CLIENT_ID);
                d.Headers.Add("Authorization", "Bearer " + token);
                return d.DownloadString(out response);
            }

            response = null;
            return errorCode;
        }

        public static string ErrorCodeToString(int errorCode)
        {
            switch (errorCode)
            {
                case ERROR_USER_NOT_FOUND:
                    return "User not found";

                case ERROR_USER_OFFLINE:
                    return "User offline";

                default:
                    return $"{errorCode} unknown";
            }
        }
    }

    public sealed class TwitchGameInfo
    {
        public string Title { get; set; }
        public string ImagePreviewSmallUrl { get; set; }
        public string ImagePreviewMediumUrl { get; set; }
        public string ImagePreviewLargeUrl { get; set; }
        public Stream ImageData { get; set; } = new MemoryStream();

        ~TwitchGameInfo()
        {
            ImageData?.Dispose();
        }
    }

    public sealed class TwitchUserInfo
    {
        public string DisplayName { get; set; }
        public string Id { get; set; }
    }

    public sealed class TwitchVod
    {
        public string Title { get; set; }
        public string VideoId { get; set; }
        public string StreamId { get; set; }
        public string VideoUrl { get; set; }
        public TimeSpan Length { get; set; }
        public int ViewCount { get; set; }
        public string Type { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateDeletion { get; set; }
        public string ImagePreviewTemplateUrl { get; set; }
        public string ImageAnimatedPreviewUrl { get; set; }
        public Stream ImageData { get; set; } = new MemoryStream();
        public bool IsPrime { get; set; }
        public TwitchUserInfo UserInfo { get; private set; } = new TwitchUserInfo();
        public TwitchGameInfo GameInfo { get; private set; } = new TwitchGameInfo();
        public string InfoStringJson { get; set; }

        public bool IsHighlight()
        {
            return Type == "highlight";
        }

        ~TwitchVod()
        {
            ImageData?.Dispose();
        }
    }

    public sealed class TwitchHelixOauthToken
    {
        public const string TWITCH_HELIX_OAUTH_TOKEN_URL_TEMPLATE = "https://id.twitch.tv/oauth2/token?client_id={0}&" +
            "client_secret={1}&grant_type=client_credentials";
        public const string TWITCH_CLIENT_SECRET = "srr2yi260t15ir6w0wq5blir22i9pq";

        public string AccessToken { get; private set; }
        public string TokenType { get; private set; }
        public DateTime ExpireDate { get; private set; } = DateTime.MinValue;

        public void Reset()
        {
            AccessToken = null;
            TokenType = null;
            ExpireDate = DateTime.MinValue;
        }

        public int Update(string clientId)
        {
            int errorCode = 200;
            if (ExpireDate <= DateTime.Now || string.IsNullOrEmpty(AccessToken))
            {
                string req = string.Format(TWITCH_HELIX_OAUTH_TOKEN_URL_TEMPLATE, clientId, TWITCH_CLIENT_SECRET);
                errorCode = HttpsPost(req, out string buf);
                if (errorCode == 200)
                {
                    JObject j = JObject.Parse(buf);
                    if (j == null)
                    {
                        return 400;
                    }
                    AccessToken = j.Value<string>("access_token");
                    long expiresIn = j.Value<long>("expires_in");
                    ExpireDate = DateTime.Now.Add(TimeSpan.FromSeconds(expiresIn));
                }
            }
            return errorCode;
        }
    }
}
