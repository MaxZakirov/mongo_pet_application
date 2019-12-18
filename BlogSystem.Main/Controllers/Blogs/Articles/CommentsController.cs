using BlogSystem.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogSystem.Main.Controllers.Blogs.Articles
{
	public class CommentsController : Controller
	{
		private readonly ArticleCommentsRepository articleCommentsRepository;

		public CommentsController(ArticleCommentsRepository articleCommentsRepository)
		{
			this.articleCommentsRepository = articleCommentsRepository;
		}

		public ControllerResult GetCommentsWithReplies()
		{
			try
			{
				var articles = articleCommentsRepository.GetCommentsWithReplies();

				var msg = string.Join(Environment.NewLine, articles.Select(a => a.BuildDescriptionMessage()));

				return Ok(msg);
			}
			catch (Exception ex)
			{
				return Error(ex.Message);
			}
		}
	}
}
