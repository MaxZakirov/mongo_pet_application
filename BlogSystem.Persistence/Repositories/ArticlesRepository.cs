using BlogSystem.Persistence.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogSystem.Persistence.Repositories
{
	public class ArticlesRepository : Repository
	{
		private readonly IMongoCollection<Article> articles;

		public ArticlesRepository(string connectionString) : base(connectionString)
		{
			articles = db.GetCollection<Article>("Articles");
		}

		public void Add(Article article)
		{
			articles.InsertOne(article);
		}

		public IEnumerable<Article> GetAll()
		{
			var getAllFilter = new BsonDocument();
			return articles.Find(getAllFilter).ToEnumerable();
		}

		public IEnumerable<Article> GetBlogArticles(string blogId)
		{
			var filter = Builders<Article>.Filter.Eq(nameof(Article.BlogId), blogId);
			return articles.Find(filter).ToEnumerable();
		}

		public long CountArticlesInBlog(string blogId)
		{
			var filter = Builders<Article>.Filter.Eq(nameof(Article.BlogId), blogId);
			return articles.Find(filter).CountDocuments();
		}

		public Article GetMostLikedArticle(IEnumerable<string> userBlogsIds = null)
		{
			var filter = new BsonDocument { { nameof(Article.Likes), -1 } };

			if (userBlogsIds == null || !userBlogsIds.Any())
			{
				return articles.Find(new BsonDocument()).Sort(filter).First();
			}

			var userfilter = Builders<Article>.Filter.In(nameof(Article.BlogId), userBlogsIds);
			return articles.Find(userfilter).Sort(filter).First();
		}

		public Dictionary<string,int> GetArticlesCommentsAmount()
		{
			string stringCommand = @"
function rec(comments) {
        var res = comments.length;

        for(var i=0;i<comments.length; i++)
        {
            var comment = comments[i];

            if(comment.Replies.length > 0)
            {
                res += rec(comment.Replies);
            }
        }

        return res;
}

function map(article) {
    return { id: article._id , count: rec(article.Comments) };
}

db.Articles.find().map(map)";

			BsonDocument command = new BsonDocument("eval", stringCommand);

			dynamic res = db.RunCommand<dynamic>(command);

			Dictionary<string, int> dicitionary = new Dictionary<string, int>();
   
			foreach (dynamic r in res)
			{
				foreach(var l in r.Value)
				{
					BsonObjectId articleId = l.id;
					int count =  Convert.ToInt32(l.count);
					dicitionary.Add(articleId.ToString(), count);
				}

				return dicitionary;
			}

			return dicitionary;
		}

		public string GetMostPopularTag()
		{
			var queryResult = articles.Aggregate<Article>()
				.Unwind(a => a.Tags)
				.Group(new BsonDocument {
					{ "_id", $"${nameof(Article.Tags)}"  },
					{ "total", new BsonDocument("$sum", 1) }
				})
				.Sort(new BsonDocument("total", -1))
				.Limit(1)
				.ToList();

			var v = queryResult.First()["_id"];

			return v.ToString();
		}

		public void Remove(string id)
		{
			throw new NotImplementedException();
		}
	}
}
