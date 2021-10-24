namespace TwitterSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Tweet
    {
        private ICollection<User> favouredBy;
        private ICollection<Report> reports;
        private ICollection<Tweet> replies;
        private ICollection<Tweet> retweets;

        public Tweet()
        {
            this.favouredBy = new HashSet<User>();
            this.reports = new HashSet<Report>();
            this.replies = new HashSet<Tweet>();
            this.retweets = new HashSet<Tweet>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime TweetedAt { get; set; }

        [Required]
        [MinLength(2)]
        public string Text { get; set; }

        [Required]
        public int RetweetCount { get; set; }

        [Required]
        public int SharedCount { get; set; }

        [Required]
        public string OwnerId { get; set; }

        public virtual User Owner { get; set; }

        public int? RepliedToTweetId { get; set; }

        public virtual Tweet RepliedToTweet { get; set; }

        public int? RetweetedFromId { get; set; }

        public virtual Tweet RetweetedFrom { get; set; }

        public virtual ICollection<User> FavouredBy
        {
            get { return this.favouredBy; }
            set { this.favouredBy = value; }
        }

        public virtual ICollection<Report> Reports
        {
            get { return this.reports; }
            set { this.reports = value; }
        }

        public virtual ICollection<Tweet> Replies
        {
            get { return this.replies; }
            set { this.replies = value; }
        }

        public virtual ICollection<Tweet> Retweets
        {
            get { return this.retweets; }
            set { this.retweets = value; }
        }
    }
}
