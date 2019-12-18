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
	public class ArticleCommentsRepository : Repository
	{
		private readonly IMongoCollection<Article> articles;

		public ArticleCommentsRepository(string connectionString) : base(connectionString)
		{
			articles = db.GetCollection<Article>("Articles");
		}

		public IEnumerable<Article> GetCommentsWithReplies()
		{
			BsonDocument filterDefinition = new BsonDocument {
				{ "Comments", new BsonDocument {
						{ "$filter", new BsonDocument {
								{ "input", "$Comments" },
								{ "as", "comment" },
								{ "cond", new BsonDocument {
											{ "$gt", new BsonArray { new BsonDocument { { "$size", "$$comment.Replies" } }, 0 } }
										}
									}
								}
							}
						}
					},
					{ nameof(Article.BlogId), 1 },
					{ nameof(Article.Likes), 1 },
					{ nameof(Article.Views), 1 },
					{ nameof(Article.Text), 1 },
					{ nameof(Article.Name), 1 },
					{ nameof(Article.Tags), 1 }
				};

			List<BsonDocument> queryResult = articles.Aggregate()
				.Project(filterDefinition)
				.ToList();

			var res = queryResult.Select(r => BsonSerializer.Deserialize<Article>(r));

			return res;
		}
	}
}
