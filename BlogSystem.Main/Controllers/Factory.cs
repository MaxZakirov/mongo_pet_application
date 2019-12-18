using BlogSystem.Main.Controllers.Blogs;
using BlogSystem.Main.Controllers.Blogs.Articles;
using BlogSystem.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSystem.Main.Controllers
{
	public class Factory
	{
		private readonly string connectionString;

		public UsersRepository UsersRepository => new UsersRepository(connectionString);

		public UserBlogsRepository BlogsRepository => new UserBlogsRepository(connectionString);

		public ArticlesRepository ArticlesRepository => new ArticlesRepository(connectionString);

		public ArticleCommentsRepository ArticleCommentsRepository => new ArticleCommentsRepository(connectionString);

		public Factory(string connectionString)
		{
			this.connectionString = connectionString;
		}

		public UserController CreateUserController() => new UserController(UsersRepository);

		public CommentsController CommentsController() => new CommentsController(ArticleCommentsRepository);

		public BlogsController CreateBlogController() => new BlogsController(BlogsRepository, UsersRepository, ArticlesRepository, DeleteBlogTransaction);

		public ArticlesController CreateArticlesController() => new ArticlesController(ArticlesRepository, BlogsRepository);

		public DeleteBlogTransaction DeleteBlogTransaction => new DeleteBlogTransaction(connectionString);
	}
}
