using System;
using System.Collections.Generic;
using System.Text;

namespace Test.Interfaces.Services
{
	public interface IDataAccess
	{
		public string Save(string values);
		public string Read();
	}
}
