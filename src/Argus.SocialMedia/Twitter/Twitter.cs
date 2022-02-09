/*
 * @author            : Blake Pell
 * @website           : http://www.blakepell.com
 * @initial date      : 2017-08-25
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
 * @license           : MIT
 */

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Argus.SocialMedia.Twitter
{
    /// <summary>
    /// A wrapper around Tweetinvi for common use cases including custom code for working with data from Twitter.
    /// </summary>
    public class Twitter
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="accessToken"></param>
        /// <param name="accessTokenSecret"></param>
        public Twitter(string consumerKey, string consumerSecret, string accessToken, string accessTokenSecret)
        {
            this.ConsumerKey = consumerKey;
            this.ConsumerSecret = consumerSecret;
            this.AccessToken = accessToken;
            this.AccessTokenSecret = accessTokenSecret;

            // Actually pass the credentials into the Tweetinvi API
            Auth.SetUserCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);

            // Additional config we want to set as default.
            TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;
            TweetinviConfig.ApplicationSettings.TweetMode = TweetMode.Extended;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Twitter()
        {
        }

        #endregion

        #region Searching

        /// <summary>
        /// Returns the entire list of requested tweets on a user's timeline.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="excludeReplies"></param>
        /// <param name="includeRetweets"></param>
        public List<ITweet> UserTimeline(string username, bool excludeReplies, bool includeRetweets)
        {
            var param = new UserTimelineParameters
            {
                MaximumNumberOfTweetsToRetrieve = 200,
                ExcludeReplies = excludeReplies,
                IncludeRTS = includeRetweets
            };

            var lastTweets = Timeline.GetUserTimeline(username, param);
            var allTweets = new List<ITweet>(lastTweets);

            while (lastTweets.Any() && allTweets.Count <= 3200)
            {
                long idOfOldestTweet = lastTweets.Select(x => x.Id).Min();
                int numberOfTweetsToRetrieve = allTweets.Count > 3000 ? 3200 - allTweets.Count : 200;

                var timelineRequestParameters = new UserTimelineParameters
                {
                    // MaxId ensures that we only get tweets that have been posted BEFORE the oldest tweet we received
                    MaxId = idOfOldestTweet - 1,
                    MaximumNumberOfTweetsToRetrieve = numberOfTweetsToRetrieve
                };

                lastTweets = Timeline.GetUserTimeline(username, timelineRequestParameters);
                allTweets.AddRange(lastTweets);
            }

            return allTweets;
        }

        /// <summary>
        /// Returns the latest 40 tweets.  Excluding replies and/or retweets will lower the number returned.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="excludeReplies"></param>
        /// <param name="includeRetweets"></param>
        public List<ITweet> UserTimelineRecent(string username, bool excludeReplies, bool includeRetweets)
        {
            var param = new UserTimelineParameters
            {
                ExcludeReplies = excludeReplies,
                IncludeRTS = includeRetweets
            };

            return Timeline.GetUserTimeline(username, param).ToList();
        }

        #endregion

        #region Tweeting

        /// <summary>
        /// Tweet's a message.
        /// </summary>
        /// <param name="message"></param>
        public void Tweet(string message)
        {
            Tweetinvi.Tweet.PublishTweet(message);
        }

        /// <summary>
        /// Tweet's a message as a reply to another tweet.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="replyTweetId"></param>
        public void Tweet(string message, long replyTweetId)
        {
            Tweetinvi.Tweet.PublishTweetInReplyTo(message, replyTweetId);
        }

        /// <summary>
        /// Retweets a tweet.
        /// </summary>
        /// <param name="tweetId"></param>
        public void Retweet(long tweetId)
        {
            Tweetinvi.Tweet.PublishRetweet(tweetId);
        }

        /// <summary>
        /// Retweets a tweet with an additional message.
        /// </summary>
        /// <param name="tweetId"></param>
        /// <param name="message"></param>
        public void Retweet(long tweetId, string message)
        {
            var tweet = Tweetinvi.Tweet.GetTweet(tweetId);
            message = $"{message} {tweet.Url}";
            Tweetinvi.Tweet.PublishTweet(message);
        }

        #endregion

        #region Trends

        /// <summary>
        /// Return trending topics on the Earth.
        /// </summary>
        public List<ITrend> TrendingTweets()
        {
            return Trends.GetTrendsAt(1).Trends;
        }

        /// <summary>
        /// Returns trending topics for a specified WOEID (Where On Earth Identifier)
        /// </summary>
        /// <param name="woeId"></param>
        public List<ITrend> TrendingTweets(long woeId)
        {
            return Trends.GetTrendsAt(woeId).Trends;
        }

        /// <summary>
        /// Returns all of the trends near a given latitude and longitude
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        public List<ITrend> TrendingTweets(double latitude, double longitude)
        {
            var list = new List<ITrend>();
            var closestTrendLocations = Trends.GetClosestTrendLocations(latitude, longitude);

            foreach (var item in closestTrendLocations)
            {
                var trends = Trends.GetTrendsAt(item.WoeId).Trends;

                foreach (var trend in trends)
                {
                    if (!list.Contains(trend))
                    {
                        list.Add(trend);
                    }
                }
            }

            return list;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Returns all of the hashtags in a provided set of text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="includeHashtag">Whether to include the hashtag character in each entry.</param>
        public static List<string> HashtagList(string text, bool includeHashtag)
        {
            var hashtags = new List<string>();
            bool track = false;
            var sb = new StringBuilder();

            foreach (char c in text)
            {
                if (c == '#')
                {
                    sb.Clear();
                    track = true;
                }
                else if (track)
                {
                    if (char.IsLetterOrDigit(c) || c == '_')
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        if (includeHashtag)
                        {
                            hashtags.Add("#" + sb);
                        }
                        else
                        {
                            hashtags.Add(sb.ToString());
                        }

                        track = false;
                    }
                }
            }

            if (track)
            {
                if (includeHashtag)
                {
                    hashtags.Add("#" + sb);
                }
                else
                {
                    hashtags.Add(sb.ToString());
                }
            }

            return hashtags;
        }

        /// <summary>
        /// Returns all of the @ usernames in a provided set of text.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="includeAt">Whether to include the @ character in each entry.</param>
        public static List<string> UserList(string text, bool includeAt)
        {
            var users = new List<string>();
            bool track = false;
            var sb = new StringBuilder();

            foreach (char c in text)
            {
                if (c == '@')
                {
                    sb.Clear();
                    track = true;
                }
                else if (track)
                {
                    if (char.IsLetterOrDigit(c) || c == '_')
                    {
                        sb.Append(c);
                    }
                    else
                    {
                        if (includeAt)
                        {
                            users.Add("@" + sb);
                        }
                        else
                        {
                            users.Add(sb.ToString());
                        }

                        track = false;
                    }
                }
            }

            if (track)
            {
                if (includeAt)
                {
                    users.Add("@" + sb);
                }
                else
                {
                    users.Add(sb.ToString());
                }
            }

            return users;
        }

        /// <summary>
        /// Removes Hashtags from the provided text.
        /// </summary>
        /// <param name="text"></param>
        public static string RemoveHashtags(string text)
        {
            var sb = new StringBuilder(text);
            var list = HashtagList(text, true);

            foreach (string item in list)
            {
                sb.Replace(item, "");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes users from the provided text.
        /// </summary>
        /// <param name="text"></param>
        public static string RemoveUsers(string text)
        {
            var sb = new StringBuilder(text);
            var list = UserList(text, true);

            foreach (string item in list)
            {
                sb.Replace(item, "");
            }

            return sb.ToString();
        }

        #endregion

        #region Properties

        /// <summary>
        /// The Consumer key provided by Twitter for an application.
        /// </summary>
        public string ConsumerKey { get; set; } = "";

        /// <summary>
        /// The Consumer secret provided by Twitter for an application.
        /// </summary>
        public string ConsumerSecret { get; set; } = "";

        /// <summary>
        /// The Access token.
        /// </summary>
        public string AccessToken { get; set; } = "";

        /// <summary>
        /// The access token secret.
        /// </summary>
        public string AccessTokenSecret { get; set; } = "";

        private TweetMode _tweetMode = TweetMode.Extended;

        /// <summary>
        /// The TweetMode to use.
        /// </summary>
        public TweetMode TweetMode
        {
            get => _tweetMode;
            set
            {
                TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;
                TweetinviConfig.ApplicationSettings.TweetMode = TweetMode.Extended;
                _tweetMode = value;
            }
        }

        #endregion
    }
}