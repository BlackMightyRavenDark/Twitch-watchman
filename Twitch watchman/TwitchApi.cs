using System;
using System.Collections.Specialized;
using System.Deployment.Application;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
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

        private const string TWITCH_USHER_HLS_1 = "https://usher.ttvnw.net/api/channel/hls/{0}.m3u8?";
        private const string TWITCH_USHER_HLS_2 =
                "player=twitchweb&p={0}&type=any&allow_source=true&" +
                "allow_audio_only=true&allow_spectre=false&sig={1}&token={2}&fast_bread=True";

        public const int ERROR_USER_NOT_FOUND = -1000;
        public const int ERROR_USER_OFFLINE = -1001;

        public TwitchHelixOauthToken HelixOauthToken { get; private set; } = new TwitchHelixOauthToken();

        public int GetHelixOauthToken(out string resToken)
        {
            int errorCode = HelixOauthToken.Update(TWITCH_CLIENT_ID);
            resToken = HelixOauthToken.AccessToken;
            return errorCode;
        }

        /// <summary>
        /// WARNING!!! Do not use this body if you are signed in!!!
        /// </summary>
        public static JArray GenerateChannelTokenRequestBody(string userLogin)
        {
            const string hashValue = "0828119ded1c13477966434e15800ff57ddacf13ba1911c129dc2200705b0712";
            JObject jPersistedQuery = new JObject();
            jPersistedQuery["version"] = 1;
            jPersistedQuery["sha256Hash"] = hashValue;

            JObject jExtensions = new JObject();
            jExtensions.Add(new JProperty("persistedQuery", jPersistedQuery));

            JObject jVariables = new JObject();
            jVariables["isLive"] = true;
            jVariables["login"] = userLogin;
            jVariables["isVod"] = false;
            jVariables["vodID"] = "";
            jVariables["playerType"] = "site";

            JObject json = new JObject();
            json["operationName"] = "PlaybackAccessToken";
            json.Add(new JProperty("extensions", jExtensions));
            json.Add(new JProperty("variables", jVariables));

            JArray jsonArr = new JArray() { json };
            return jsonArr;
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

        public int GetChannelAccessToken(string channelName, out string response)
        {
            response = null;
            JArray body = GenerateChannelTokenRequestBody(channelName);
            TwitchClientIntegrity integrity = new TwitchClientIntegrity();
            int errorCode = integrity.Update(body);
            if (errorCode == 200)
            {
                NameValueCollection headers = new NameValueCollection();
                headers.Add("User-Agent", USER_AGENT);
                headers.Add("Host", "gql.twitch.tv");
                headers.Add("Client-ID", TWITCH_CLIENT_ID_PRIVATE);
                headers.Add("Client-Session-Id", integrity.DeviceId.ToString());
                headers.Add("Client-Integrity", integrity.Token);

                errorCode = HttpsPost(TWITCH_GQL_API_URL, body.ToString(), headers, out response);
            }
            return errorCode;
        }

        public void ParseChannelToken(string unparsedChannelToken, out string token, out string signature)
        {
            JArray json = JArray.Parse(unparsedChannelToken);
            if (json.Count > 0)
            {
                JObject jData = json[0].Value<JObject>("data");
                if (jData != null)
                {
                    JObject jToken = jData.Value<JObject>("streamPlaybackAccessToken");
                    if (jToken != null)
                    {
                        token = Uri.EscapeDataString(jToken.Value<string>("value"));
                        signature = jToken.Value<string>("signature");
                        return;
                    }
                }
            }

            token = null;
            signature = null;
        }

        public int GetLiveStreamManifestUrl(string channelName, out string manifestUrl)
        {
            int errorCode = GetChannelAccessToken(channelName, out string unparsedToken);
            if (errorCode == 200)
            {
                ParseChannelToken(unparsedToken, out string token, out string sig);
                int randomInt = new Random().Next(999999);
                manifestUrl = string.Format(TWITCH_USHER_HLS_1, channelName) +
                    string.Format(TWITCH_USHER_HLS_2, randomInt.ToString(), sig, token);
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

        public int IsUserLive_Helix(string userId, out string resJson)
        {
            string url = GetUserIsLiveRequestUrl_Helix(userId);
            int res = HttpsGet_Helix(url, out string buf);
            if (res == 200)
            {
                JObject j = JObject.Parse(buf);
                JArray ja = j.Value<JArray>("data");
                if (ja.Count > 0)
                {
                    resJson = buf;
                    return 200;
                }
                else
                {
                    resJson = null;
                    return ERROR_USER_OFFLINE;
                }
            }
            else
            {
                resJson = null;
            }
            return res;
        }

        public int HttpsGet_Helix(string url, out string recvText)
        {
            int errorCode = GetHelixOauthToken(out string token);
            if (errorCode == 200)
            {
                FileDownloader d = new FileDownloader();
                d.Url = url;
                d.Headers.Add("Client-ID", TWITCH_CLIENT_ID);
                d.Headers.Add("Authorization", "Bearer " + token);
                errorCode = d.DownloadString(out recvText);
                return errorCode;
            }
            recvText = null;
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

    public sealed class TwitchClientIntegrity
    {
        public string Token { get; private set; }
        public long Expiration { get; private set; } = -1L;
        public string RequestId { get; private set; }
        public Guid DeviceId { get; private set; }

        public const string INTEGRITY_API_URL = "https://gql.twitch.tv/integrity";

        public int Update(JArray requestBody)
        {
            DeviceId = Guid.NewGuid();

            NameValueCollection headers = new NameValueCollection();
            headers.Add("User-Agent", USER_AGENT);
            headers.Add("Host", "gql.twitch.tv");
            headers.Add("Client-ID", TwitchApi.TWITCH_CLIENT_ID_PRIVATE);
            headers.Add("X-Device-id", DeviceId.ToString());

            int errorCode = HttpsPost(INTEGRITY_API_URL, requestBody.ToString(), headers, out string response);
            if (errorCode == 200)
            {
                JObject j = JObject.Parse(response);
                Token = j.Value<string>("token");
                Expiration = j.Value<long>("expiration");
                RequestId = j.Value<string>("request_id");
            }
            return errorCode;
        }
    }
}
