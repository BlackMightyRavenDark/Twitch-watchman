using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace Twitch_watchman
{
    internal class Configurator
    {
        public string SelfDirPath { get; }
        public string FilePath { get; }
        public string DownloadingDirPath { get; set; }
        public string FileNameFormat { get; set; }
        public string ChannelListFilePath { get; set; }
        public bool SaveStreamInfo { get; set; }
        public bool SaveChunksInfo { get; set; }
        public bool StopIfPlaylistLost { get; set; }
        public int CheckingIntervalInactive { get; set; }
        public int CheckingIntervalActive { get; set; }

        public delegate void SavingDelegate(object sender, JObject root);
        public delegate void LoadingDelegate(object sender, JObject root);
        public SavingDelegate Saving;
        public LoadingDelegate Loading;

        public Configurator()
        {
            SelfDirPath = Path.GetDirectoryName(Application.ExecutablePath);
            FilePath = Path.Combine(SelfDirPath, "tw_config.json");
            LoadDefaults();
        }

        public void LoadDefaults()
        {
            ChannelListFilePath = Path.Combine(SelfDirPath, "tw_channelList.json");
            DownloadingDirPath = SelfDirPath;
            FileNameFormat = Utils.FILENAME_FORMAT_DEFAULT;
            SaveStreamInfo = true;
            SaveChunksInfo = true;
            StopIfPlaylistLost = true;
            CheckingIntervalInactive = 3;
            CheckingIntervalActive = 10;
        }

        public void Load()
        {
            LoadDefaults();
            if (File.Exists(FilePath))
            {
                JObject json = Utils.TryParseJson(File.ReadAllText(FilePath));
                if (json != null && Loading != null) { Loading.Invoke(this, json); }
            }
        }

        public void Save()
        {
            JObject json = new JObject();
            Saving?.Invoke(this, json);
            if (File.Exists(FilePath))
            {
                File.Delete(FilePath);
            }
            File.WriteAllText(FilePath, json.ToString());
        }
    }
}
