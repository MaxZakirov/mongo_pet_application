using BlogSystem.Persistence.Models;
using BlogSystem.Persistence.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogSystem.Main.Controllers
{
	public class UserController : Controller
	{
		private const int DuplicatedKeyErrorCode = 11000;

		private readonly UsersRepository userRepository;

		public UserController(UsersRepository userRepository)
		{
			this.userRepository = userRepository;
		}

		public ControllerResult CheckIn(string userName)
		{
			try
			{
				userRepository.Add(new User(userName));
				return Ok();
			}
			catch(MongoWriteException ex)
			{
				string message = ex.Message;
				if (ex.WriteError.Code == DuplicatedKeyErrorCode)
					message = "User with such name already exists";
				return Error(message);
			}
			catch (Exception ex)
			{
				return Error(ex.Message);
			}
		}

		public ControllerResult GetByName(string userName)
		{
			try
			{
				if (string.IsNullOrEmpty(userName))
				{
					var allUsers = userRepository.GetAll();
					string usersPresentation = string.Join(Environment.NewLine, allUsers.Select(u => GetUserInfoLine(u)));
					return Ok(usersPresentation);
				}

				var user = userRepository.GetByBlogId(userName);
				return Ok(GetUserInfoLine(user));
			}
			catch (Exception ex)
			{
				return Error(ex.Message);
			}
		}

		public ControllerResult Delete(string userName)
		{
			try
			{
				userRepository.Remove(userName);
				return new ControllerResult(ControllerResultStatus.Ok, "User was sucessfully deleted.");
			}
			catch (Exception ex)
			{
				return Error(ex.Message);
			}
		}

		private string GetUserInfoLine(User user)
		{
			var blogAmount = user.Blogs?.Count() ?? 0;
			return $"Name: {user.Name}, Blogs: {blogAmount}";
		}
	}
}
