using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSystem.Persistence.Repositories
{
	public abstract class Repository
	{
		protected readonly IMongoDatabase db;

		public Repository(string connectionString)
		{
			var mongoClient = new MongoClient(connectionString);
			db = mongoClient.GetDatabase("BlogSystemDB");
		}
	}
}
