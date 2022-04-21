using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEntities
{
	public class UserList
	{
		public List<GetUserList>? GetUserList { get; set; }
		public Int32 TotalRecords { get; set; }
	}
	public class GetUserList
	{
		public int UserId { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? EmailId { get; set; }
		public string? DateOfBirth { get; set; }
	}


}
