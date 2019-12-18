using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSystem.Persistence.Models
{
	public class Article
	{
		public Article(
			string name, 
			string text,
			List<string> tags, 
			string blogId, 
			int likes = 0,
			int views = 0,
			List<Comment> comments = null)
		{
			Name = name;
			Text = text;
			Likes = likes;
			Views = views;
			Comments = comments ?? new List<Comment>();
			Tags = tags;
			BlogId = blogId;
		}

		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public string Name { get; set; }

		public string Text { get; set; }

		public int Likes { get; set; }

		public int Views { get; set; }

		public List<string> Tags { get; set; }

		public List<Comment> Comments { get; set; }

		[BsonRepresentation(BsonType.ObjectId)]
		public string BlogId { get; set; }

		[BsonIgnore]
		public Blog Blog { get; set; }
	}
}
