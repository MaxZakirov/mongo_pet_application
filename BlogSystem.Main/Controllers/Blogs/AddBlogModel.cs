using System;
using System.Collections.Generic;
using System.Text;

namespace BlogSystem.Main.Controllers.Blogs
{
	public class AddBlogModel
	{
		public AddBlogModel(string name, string description)
		{
			Name = name;
			Description = description;
		}

		public string Name { get; }
		public string Description { get; }
	}
}
