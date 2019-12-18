using BlogSystem.Persistence.Exceptions;
using BlogSystem.Persistence.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSystem.Persistence.Repositories
{
	public class UsersRepository : Repository
	{
		private readonly IMongoCollection<User> users;

		public UsersRepository(string connectionString) : base(connectionString)
		{
			users = db.GetCollection<User>("Users");
		}

		public void Add(User model)
		{
			users.InsertOne(model);
		}

		public IEnumerable<User> GetAll()
		{
			var emptyBson = new BsonDocument();
			return users.Find(emptyBson).ToEnumerable();
		}

		public User GetByBlogId(string id)
		{
			var idFilter = Builders<User>.Filter.Eq(nameof(User.Name), id);
			return users.Find(idFilter).FirstOrDefault() ?? throw new UserNotFoundException();
		}

		public void Remove(string id)
		{
			var idFilter = Builders<User>.Filter.Eq(nameof(User.Name), id);
			var res = users.DeleteOne(idFilter);

			if (res.DeletedCount < 1)
				throw new UserNotFoundException();
		}
	}
}
