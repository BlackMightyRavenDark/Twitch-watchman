
namespace Twitch_watchman
{
    class TwitchPlaylistsManifestParser
    {
        private string[] medias;
        private string[] urls;
        private int elementsCount;

        public TwitchPlaylistsManifestParser(string inputManifestString)
        {
            string[] strings = inputManifestString.Split('\n');
            elementsCount = (strings.Length - 2) / 3;
            medias = new string[elementsCount];
            urls = new string[elementsCount];
            for (int i = 2; i < strings.Length; i += 3)
            {
                int id = (i - 2) / 3;
                medias[id] = strings[i];
                urls[id] = strings[i + 2];
            }
        }

        public string GetGroupId(int n)
        {
            string t = medias[n];
            int j = t.IndexOf("GROUP-ID=\"");
            t = t.Remove(0, j + 10);
            j = t.IndexOf("\"");
            t = t.Substring(0, j);
            return t;
        }

        public string GetUrl(int n)
        {
            return urls[n];
        }

        public int GetCount()
        {
            return elementsCount;
        }
    }
}
