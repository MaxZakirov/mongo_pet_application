using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogSystem.Persistence.Models
{
	public class User
	{
		public User()
		{
		}

		public User(string name, IEnumerable<Blog> blogs = null)
		{
			Name = name;
			Blogs = blogs?.ToList();
		}

		[BsonId]
		public string Name { get; set; }

		[BsonIgnoreIfNull]
		public List<Blog> Blogs { get; set; }
	}
}
