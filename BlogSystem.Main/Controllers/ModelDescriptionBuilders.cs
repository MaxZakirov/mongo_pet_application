using BlogSystem.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogSystem.Main.Controllers
{
	public static class ModelDescriptionBuilders
	{
		public static string BuildDescriptionMessage(this Comment comment)
		{
			var res = $"Author: {comment.Author}" + Environment.NewLine +
				$"Last Modified: {comment.LastModified}" + Environment.NewLine +
				$"Text: {comment.Text}" + Environment.NewLine;

			if(comment.Replies.Any())
			{
				res += $"Replies to {comment.Author}: {string.Join(Environment.NewLine, comment.Replies.Select(c => c.BuildDescriptionMessage()))} " + Environment.NewLine;
			}

			return res;
		}

		public static string BuildDescriptionMessage(this Article article)
		{
			return $"Article Name: {article.Name}" + Environment.NewLine +
				$"Likes: {article.Likes}" + Environment.NewLine +
				$"Views: {article.Views}" + Environment.NewLine +
				$"Tags: {string.Join(", ", article.Tags)} " +
				$"Text: {Environment.NewLine + article.Text}" + Environment.NewLine +
				$"Comments: {Environment.NewLine + string.Join(Environment.NewLine + Environment.NewLine, article.Comments.Select(c => c.BuildDescriptionMessage()))}";
		}

		public static string BuildDescriptionMessage(this Blog blog, long articlesCount)
		{
			return $"Blog Id: {blog.Id + Environment.NewLine} " +
				$"Articles in blog: {articlesCount + Environment.NewLine} " +
				$"Blog Name: {blog.Name + Environment.NewLine}" +
				$"Blog Descrption: {blog.Description + Environment.NewLine} " +
				$"Created Date: {blog.CreatedOn + Environment.NewLine} " +
				$"Last Modified: {blog.LastMidified + Environment.NewLine}";
		}
	}
}
