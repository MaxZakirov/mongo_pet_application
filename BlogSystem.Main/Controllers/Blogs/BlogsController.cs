using BlogSystem.Main.Controllers.Blogs;
using BlogSystem.Persistence.Models;
using BlogSystem.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogSystem.Main.Controllers.Blogs
{
	public class BlogsController : Controller
	{
		private readonly UserBlogsRepository blogsRepository;
		private readonly UsersRepository usersRepository;
		private readonly ArticlesRepository articlesRepository;
		private readonly DeleteBlogTransaction deleteBlogTransaction;

		public BlogsController(
			UserBlogsRepository blogsRepository,
			UsersRepository usersRepository,
			ArticlesRepository articlesRepository,
			DeleteBlogTransaction deleteBlogTransaction)
		{
			this.blogsRepository = blogsRepository;
			this.usersRepository = usersRepository;
			this.articlesRepository = articlesRepository;
			this.deleteBlogTransaction = deleteBlogTransaction;
		}

		public ControllerResult AddBlogToUser(string userName, AddBlogModel addBlogModel)
		{
			try
			{
				Blog blog = new Blog(
					addBlogModel.Name,
					addBlogModel.Description);

				blogsRepository.Add(userName, blog);
				return Ok();
			}
			catch (Exception ex)
			{
				return Error(ex.Message);
			}
		}

		public ControllerResult DeleteBlog(string blogId)
		{
			try
			{
				deleteBlogTransaction.Execute(blogId);

				return Ok();
			}
			catch (Exception ex)
			{
				return Error(ex.Message);
			}
		}

		public ControllerResult ListUserBlogs(string userName)
		{
			try
			{
				User user = blogsRepository.GetSortedUserBlogs(userName);

				if (user.Blogs == null)
				{
					return Ok("This user has no blogs yet");
				}

				string blogPresentationMessage = $"{userName}'s blogs: {Environment.NewLine}";

				blogPresentationMessage += string.Join(Environment.NewLine, user.Blogs.Select(b =>
				{
					var articlesCount = articlesRepository.CountArticlesInBlog(b.Id);
					return b.BuildDescriptionMessage(articlesCount);
				}));

				return Ok(blogPresentationMessage);
			}
			catch (Exception ex)
			{
				return Error(ex.Message);
			}
		}
	}
}
