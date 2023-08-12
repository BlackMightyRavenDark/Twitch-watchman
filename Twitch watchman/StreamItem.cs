using System;
using HlsDumpLib;

namespace Twitch_watchman
{
    public class StreamItem
    {
        public string ChannelName { get; private set; }
        public string PlaylistUrl { get; set; }
        public string DumpingFilePath { get; set; } = null;
        public long DumpingFileSize { get; set; } = -1L;
        public bool IsChecking { get; set; } = false;
        public int TimerRemaining { get; set; } = 10;
        public string Title { get; set; }
        public DateTime DateServer { get; set; } = DateTime.MinValue;
        public DateTime DateLocal { get; set; } = DateTime.MinValue;
        public bool IsImportant { get; set; } = false;
        public bool IsStreamActive => Dumper != null;
        public HlsDumper Dumper { get; set; }

        public StreamItem(string channelName)
        {
            ChannelName = channelName;
        }
    }
}
