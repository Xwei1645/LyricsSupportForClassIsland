using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LyricsSupportForClassIsland.Services.LyricsParser
{
    public class LyricsLine
    {
        public TimeSpan Timestamp { get; set; }
        public string Text { get; set; }
    }

    public class LyricsParser
    {
        public List<LyricsLine> Parse(string content)
        {
            var lyricsLines = new List<LyricsLine>();
            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var regex = new Regex(@"\[(\d{2}):(\d{2})\.(\d{2,3})\](.*)");

            foreach (var line in lines)
            {
                var match = regex.Match(line);
                if (match.Success)
                {
                    var minutes = int.Parse(match.Groups[1].Value);
                    var seconds = int.Parse(match.Groups[2].Value);
                    var milliseconds = int.Parse(match.Groups[3].Value.PadRight(3, '0')); // 确保毫秒数是3位数
                    var text = match.Groups[4].Value;

                    var timestamp = new TimeSpan(0, 0, minutes, seconds, milliseconds);
                    lyricsLines.Add(new LyricsLine { Timestamp = timestamp, Text = text });
                }
            }

            return lyricsLines;
        }
    }
}


