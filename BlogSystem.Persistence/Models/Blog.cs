using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace BlogSystem.Persistence.Models
{
	public class Blog
	{
		public Blog()
		{
		}

		public Blog(
			string name, 
			string description, 
			DateTime? createdOn = null, 
			DateTime? lastMidified = null,
			Article[] articles = null)
		{
			CreatedOn = createdOn ?? DateTime.Now;
			LastMidified = lastMidified ?? DateTime.Now;
			Name = name;
			Description = description;
			Articles = articles;
		}

		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime LastMidified { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		[BsonIgnore]
		public Article[] Articles { get; set; }
	}
}
