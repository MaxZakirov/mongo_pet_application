using BlogSystem.Persistence.Models;
using BlogSystem.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogSystem.Main.Controllers.Blogs.Articles
{
	public class ArticlesController : Controller
	{
		private readonly ArticlesRepository articlesRepository;
		private readonly UserBlogsRepository blogsRepository;

		public ArticlesController(
			ArticlesRepository articles,
			UserBlogsRepository blogsRepository)
		{
			this.articlesRepository = articles;
			this.blogsRepository = blogsRepository;
		}

		public ControllerResult Add(AddArticleModel addmodel)
		{
			try
			{
				var userBlogs = blogsRepository.GetSortedUserBlogs(addmodel.UserName).Blogs.ToArray();
				var blogId = userBlogs[addmodel.BlogNumber].Id;

				Article article = new Article(addmodel.Name, addmodel.Text, addmodel.Tags.ToList(), blogId, addmodel.Likes, addmodel.Views, addmodel.Comments.ToList());

				articlesRepository.Add(article);

				return Ok();
			}
			catch (Exception ex)
			{
				return Error(ex.Message);
			}
		}

		public ControllerResult GetBlogArticles(string blogId)
		{
			try
			{
				IEnumerable<Article> articles = articlesRepository.GetBlogArticles(blogId);
				User blogCreator = blogsRepository.GetByBlogId(blogId);
				Blog blog = blogCreator.Blogs.Single();

				string articlesDescriptions = $"Blog '{blog.Name}' articles:" + Environment.NewLine;
				articlesDescriptions += string.Join(Environment.NewLine, articles.Select(a => a.BuildDescriptionMessage()));

				return Ok(articlesDescriptions);
			}
			catch (Exception ex)
			{
				return Error(ex.Message);
			}
		}

		public ControllerResult GetMostLikedArticle(string userName)
		{
			try
			{
				IEnumerable<string> userBlogsIds = null;
				if (!string.IsNullOrEmpty(userName))
				{
					var user = blogsRepository.GetSortedUserBlogs(userName);
					userBlogsIds = user.Blogs.Select(b => b.Id);
				}

				var mostLiked = articlesRepository.GetMostLikedArticle(userBlogsIds);
				var articlesDescription = mostLiked.BuildDescriptionMessage();

				return Ok(articlesDescription);
			}
			catch (Exception ex)
			{
				return Error(ex.Message);
			}
		}

		public ControllerResult GetArticlesCommentsAmount()
		{
			try
			{
				Dictionary<string, int> articles = articlesRepository.GetArticlesCommentsAmount();

				string msg = string.Join(Environment.NewLine, articles.Select(kv => $"id: {kv.Key}, comments amount: {kv.Value}"));

				return Ok(msg);
			}
			catch (Exception ex)
			{
				return Error(ex.Message);
			}
		}

		public ControllerResult GetMostPopularTag()
		{
			try
			{
				string tag = articlesRepository.GetMostPopularTag();

				string msg = $"The most popular tag for now is {tag}";

				return Ok(msg);
			}
			catch (Exception ex)
			{
				return Error(ex.Message);
			}
		}
	}
}
