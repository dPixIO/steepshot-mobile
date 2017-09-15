﻿using System;

namespace Steepshot.Core.Models.Requests
{
    public class CensoredPostsRequests : NamedRequestWithOffsetLimitFields
    {
		public bool ShowNsfw { get; set; }
		public bool ShowLowRated { get; set; }
    }

    public class UserPostsRequest : CensoredPostsRequests
    {
        public UserPostsRequest(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));

            Username = username;
        }

        public string Username { get; }
    }

    public enum PostType
    {
        Top,
        Hot,
        New
    }

    public class PostsRequest : CensoredPostsRequests
    {
        public PostsRequest(PostType type)
        {
            Type = type;
        }

        public PostType Type { get; }
    }

    public class PostsByCategoryRequest : PostsRequest
    {
        public PostsByCategoryRequest(PostType type, string category) : base(type)
        {
            Category = category;
        }

        public string Category { get; set; }
    }
}