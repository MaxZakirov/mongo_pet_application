using BlogSystem.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSystem.Main.Controllers.Blogs.Articles
{
	public class AddArticleModel
	{
		public AddArticleModel(string name, string text, IEnumerable<string> tags, string userName, 
			int blogNumber, int views = 0, int likes = 0, IEnumerable<Comment> comments = null)
		{
			Name = name;
			Text = text;
			Tags = tags;
			UserName = userName;
			BlogNumber = blogNumber;
			Views = views;
			Likes = likes;
			Comments = comments ?? new Comment[] { };
		}

		public string Name { get; }

		public string Text { get; }

		public IEnumerable<string> Tags { get; }

		public string UserName { get; }

		public int BlogNumber { get; }

		public int Views { get; }

		public int Likes { get; }

		public IEnumerable<Comment> Comments { get; }
	}
}
