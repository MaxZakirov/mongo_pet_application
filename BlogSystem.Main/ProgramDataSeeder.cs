using BlogSystem.Main.Controllers;
using BlogSystem.Main.Controllers.Blogs;
using BlogSystem.Main.Controllers.Blogs.Articles;
using BlogSystem.Persistence.Models;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogSystem.Main
{
	public partial class Program
	{
		private const string UserToSeed = "test1";

		private static void SeedData()
		{
			GenerateNewUser(UserToSeed);
			GenerateNewUser();
			GenerateNewUser();
			GenerateNewUser();

			AddNewBlog();
			AddNewBlog();
			AddNewBlog();
			AddNewBlog();

			AddNewArticle(0);
			AddNewArticle(3);
			AddNewArticle(1);
			AddNewArticle(1);
			AddNewArticle(2);
			AddNewArticle(2);
		}

		private static void AddNewBlog()
		{
			Name nameGenerator = new Name();
			Lorem textGenerator = new Lorem();

			string blogName = textGenerator.Sentence();
			string blogDescription = textGenerator.Sentences(3);

			AddBlogModel addBlogModel = new AddBlogModel(blogName, blogDescription);

			var c = factory.CreateBlogController();
			c.AddBlogToUser(UserToSeed, addBlogModel);
		}

		private static void AddNewArticle(int blogNumber = 0)
		{
			Name nameGenerator = new Name();
			Lorem textGenerator = new Lorem();

			string name = textGenerator.Sentence();
			string text = textGenerator.Paragraph(45);
			int tagsAmount = new Random().Next(5, 15);
			var tags = textGenerator.Words(tagsAmount);

			var r = new Random();
			int likes = r.Next(100,10000);
			int views = r.Next(10000, 100000);

			var comments = Enumerable.Range(0, r.Next(5, 50)).Select(i => GetNewRandomComment());

			AddArticleModel addArticleModel = new AddArticleModel(name,text,tags, UserToSeed, blogNumber, views, likes, comments);

			var controller = factory.CreateArticlesController();
			controller.Add(addArticleModel);
		}

		private static Comment GetNewRandomComment()
		{
			var users = factory.UsersRepository.GetAll().ToArray();

			var random = new Random();
			var userIndex = random.Next(0,3);
			var likes = random.Next(10, 100);

			Lorem textGenerator = new Lorem();
			Date date = new Date();
			var text = textGenerator.Paragraph(random.Next(0,5));

			var lastModified = new Date().Between(DateTime.Now.AddDays(-360), DateTime.Now.AddDays(-360));

			List<Comment> replies = new List<Comment>();
			while (random.Next(0, 3) == 1)
			{
				replies.Add(GetNewRandomComment());
			}

			return new Comment(users[userIndex].Name, likes, lastModified, text, replies.ToArray());
		}

		private static string GenerateNewUser(string userName = "")
		{
			Name nameGenerator = new Name();
			if(string.IsNullOrEmpty(userName))
			{
				userName = $"{nameGenerator.FirstName()}.{nameGenerator.LastName()}";
			}

			var c = factory.CreateUserController();
			c.CheckIn(userName);

			return userName;
		}
	}
}
