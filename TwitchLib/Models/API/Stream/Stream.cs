﻿using System;
using Newtonsoft.Json.Linq;

namespace TwitchLib.Models.API.Stream
{
    /// <summary>Class representing a stream as returned by Twitch API</summary>
    public class Stream
    {
        /// <summary>Property representing whether or not the stream is playlist or live.</summary>
        public bool IsPlaylist { get; protected set; }
        /// <summary>Property representing average frames per second.</summary>
        public double AverageFps { get; protected set; }
        /// <summary>Property representing any delay on the stream (in seconds)</summary>
        public int Delay { get; protected set; }
        /// <summary>Property representing height dimension.</summary>
        public int VideoHeight { get; protected set; }
        /// <summary>Property representing number of current viewers.</summary>
        public int Viewers { get; protected set; }
        /// <summary>Property representing the stream id.</summary>
        public long Id { get; protected set; }
        /// <summary>Property representing the preview images in an object.</summary>
        public Preview Preview { get; protected set; }
        /// <summary>Property representing the date time the stream was created.</summary>
        public DateTime CreatedAt { get; protected set; }
        /// <summary>Property representing the time since the stream was created (essentially uptime)</summary>
        public TimeSpan TimeSinceCreated { get; protected set; }
        /// <summary>Property representing the current game.</summary>
        public string Game { get; protected set; }
        /// <summary>Property representing the channel the stream is from.</summary>
        public Channel.Channel Channel { get; protected set; }

        /// <summary>Stream object constructor.</summary>
        public Stream(JToken twitchStreamData)
        {
            bool isPlaylist;
            long id;
            int viewers, videoHeight, delay;
            double averageFps;

            if (bool.TryParse(twitchStreamData.SelectToken("is_playlist").ToString(), out isPlaylist) && isPlaylist) IsPlaylist = true;
            if (long.TryParse(twitchStreamData.SelectToken("_id").ToString(), out id)) Id = id;
            if (int.TryParse(twitchStreamData.SelectToken("viewers").ToString(), out viewers)) Viewers = viewers;
            if (int.TryParse(twitchStreamData.SelectToken("video_height").ToString(), out videoHeight)) VideoHeight = videoHeight;
            if (int.TryParse(twitchStreamData.SelectToken("delay").ToString(), out delay)) Delay = delay;
            if (double.TryParse(twitchStreamData.SelectToken("average_fps").ToString(), out averageFps)) AverageFps = averageFps;

            Game = twitchStreamData.SelectToken("game").ToString();
            CreatedAt = Common.Helpers.DateTimeStringToObject(twitchStreamData.SelectToken("created_at").ToString());
            TimeSinceCreated = DateTime.UtcNow - CreatedAt;
            Channel = new Channel.Channel((JObject) twitchStreamData.SelectToken("channel"));
            Preview = new Preview(twitchStreamData.SelectToken("preview"));
        }
    }
}