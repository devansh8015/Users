using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserBusinessEntities
{
	public class UserListcs
	{
		public List<GetUserList> list { get; set; }
		public Int32 TotalRecords { get; set; }
	}
	public class GetUserList
	{
		public int UserId { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string EmailId { get; set; }
		public string DateOfBirth { get; set; }
	}

	public class SPParametersBE
	{
		public Int32 PageNo { get; set; }
		public Int32 PageSize { get; set; }
		public string SortColumn { get; set; }
		public string WhereClause { get; set; }
		public Int32 TotalRecord { get; set; }
		
	}
}
