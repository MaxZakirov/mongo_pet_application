using BlogSystem.Persistence.Exceptions;
using BlogSystem.Persistence.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogSystem.Persistence.Repositories
{
	public class UserBlogsRepository : Repository
	{
		private readonly IMongoCollection<User> users;
		private readonly string connectionString;

		public UserBlogsRepository(
			string connectionString) : base(connectionString)
		{
			users = db.GetCollection<User>("Users");
			this.connectionString = connectionString;
		}

		public void Add(string userName, Blog blog)
		{
			blog.Id = ObjectId.GenerateNewId().ToString();

			users.UpdateOne(
						 Builders<User>.Filter.Eq(nameof(User.Name), userName),
						 Builders<User>.Update.Push(nameof(User.Blogs), blog));
		}

		public User GetSortedUserBlogs(string userId)
		{
			var res = users.Aggregate<User>()
				.Match(new BsonDocument { { "_id", userId } })
				.Unwind(u => u.Blogs)
				.Sort(new BsonDocument { { $"{nameof(User.Blogs)}.{nameof(Blog.CreatedOn)}", -1 } })
				.Group(new BsonDocument {
					{ "_id", "$_id" },
					{ $"{nameof(User.Blogs)}", new BsonDocument { { "$push", $"${nameof(User.Blogs)}" } } }
				}).ToList();

			if(res.Count == 0)
			{
				throw new UserNotFoundException();
			}

			var user = BsonSerializer.Deserialize<User>(res.First());

			return user;
		}

		public User GetByBlogId(string id)
		{
			var filterDefinition = new BsonDocument {
				{ "Blogs", new BsonDocument {
						{ "$filter", new BsonDocument {
								{ "input", "$Blogs" },
								{ "as", "blog" },
								{ "cond", new BsonDocument { 
										{ "$eq", new BsonArray { "$$blog._id", BsonObjectId.Create(id) } } 
									} 
								}
							}
						}
					} 
				}
			};

			var res = users.Aggregate<User>()
				.Match(new BsonDocument { { "Blogs._id", BsonObjectId.Create(id) } })
				.Project(filterDefinition)
				.ToList();

			var user = BsonSerializer.Deserialize<User>(res.First());

			return user;
		}

		public void Remove(string id)
		{
			throw new NotImplementedException("PLease use DeleteBlogTransaction transaction instead");
		}

		public IEnumerable<User> GetAll()
		{
			throw new NotImplementedException();
		}
	}
}
