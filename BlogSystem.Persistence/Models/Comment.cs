using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSystem.Persistence.Models
{
	public class Comment
	{
		public Comment()
		{

		}

		public Comment(string author, int likes, DateTime lastModified, string text, Comment[] replies = null)
		{
			Author = author;
			Likes = likes;
			LastModified = lastModified;
			Text = text;
			Replies = replies ?? new Comment[] { };
		}

		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string Author { get; set; }

		public int Likes { get; set; }

		public string Text { get; set; }

		public DateTime LastModified { get; set; }

		[BsonIgnoreIfNull]
		public Comment[] Replies { get; set; }
	}
}
