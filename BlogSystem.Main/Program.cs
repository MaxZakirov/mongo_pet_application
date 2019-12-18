using BlogSystem.Main.Controllers;
using BlogSystem.Persistence.Models;
using MongoDB.Bson;
using System;
using System.Linq;

namespace BlogSystem.Main
{
	partial class Program
	{
		//private const string MongoConnectionString = @"mongodb+srv://admin:Password12@cluster0-gqus0.azure.mongodb.net/test?retryWrites=true&w=majority";
		private const string MongoConnectionString = @"mongodb://localhost:27017";
		private static Factory factory;
		private static string currentUser;

		static void Main(string[] args)
		{
			factory = new Factory(MongoConnectionString);
			//SeedData();
			while (true)
			{
				Console.WriteLine("Waiting input....");
				var input = Console.ReadLine();

				if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
				{
					break;
				}

				ProcessInput(input);
			}

			Console.WriteLine("Goodbye");
		}

		private static void ProcessInput(string userInput)
		{
			var arguments = userInput.Split(' ');
			switch (arguments.FirstOrDefault()?.ToLower())
			{
				case "checkin":
					ProcessControllerResult(factory.CreateUserController().CheckIn(arguments[1]));
					break;
				case "login":
					var res = factory.CreateUserController().CheckIn(arguments[1]);
					if (res.ResultStatus == ControllerResultStatus.Ok)
						currentUser = arguments[1];
					ProcessControllerResult(res);
					break;
				case "user":
					ProcessControllerResult(factory.CreateUserController().GetByName(arguments.Length > 1 ? arguments[1] : ""));
					break;
				case "articlesfrom":
					ProcessControllerResult(factory.CreateArticlesController().GetBlogArticles(arguments[1]));
					break;
				case "deleteuser":
					ProcessControllerResult(factory.CreateUserController().Delete(arguments[1]));
					break;
				case "ublogs":
					ProcessControllerResult(factory.CreateBlogController().ListUserBlogs(arguments[1]));
					break;
				case "besta":
					ProcessControllerResult(factory.CreateArticlesController().GetMostLikedArticle(arguments.Length > 1 ? arguments[1] : ""));
					break;
				case "commentsa":
					ProcessControllerResult(factory.CreateArticlesController().GetArticlesCommentsAmount());
					break;
				case "tag":
					ProcessControllerResult(factory.CreateArticlesController().GetMostPopularTag());
					break;
				case "dropblog":
					ProcessControllerResult(factory.CreateBlogController().DeleteBlog(arguments[1]));
					break;
				case "commentsr":
					ProcessControllerResult(factory.CommentsController().GetCommentsWithReplies());
					break;
				default:
					PrintError("Unknown command");
					break;
			}
		}

		private static void ProcessControllerResult(ControllerResult controllerResult)
		{
			if (controllerResult.ResultStatus == ControllerResultStatus.Ok)
				PrintSuccess(controllerResult.Message);
			else
				PrintError(controllerResult.Message);
		}

		private static void PrintError(string message)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(message);
			Console.ForegroundColor = ConsoleColor.White;
		}

		private static void PrintSuccess(string message)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(message);
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}
