using BlogSystem.Persistence.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSystem.Persistence.Repositories
{
	public class DeleteBlogTransaction
	{
		private readonly MongoClient mongoClient;

		public DeleteBlogTransaction(
			string connectionString)
		{
			this.mongoClient = new MongoClient(connectionString);
		}

		public void Execute(string blogId)
		{
			var database = mongoClient.GetDatabase("BlogSystemDB");

			var articles = database.GetCollection<Article>("Articles");
			var users = database.GetCollection<User>("Users");

			using (var session = mongoClient.StartSession())
			{
				session.StartTransaction();

				try
				{
					var findArticleByBlogId = Builders<Article>.Filter.Eq(a => a.BlogId, blogId);
					articles.DeleteMany(session, findArticleByBlogId);

					var filter = new BsonDocument { { "Blogs._id", BsonObjectId.Create(blogId) } };
					var update = Builders<User>.Update.PullFilter("Blogs", Builders<Blog>.Filter.Eq("_id", BsonObjectId.Create(blogId)));
					users.UpdateOne(session, filter, update);

					session.CommitTransaction();
				}
				catch(Exception ex)
				{
					session.AbortTransaction();
					throw ex;
				}
			}
		}
	}
}
