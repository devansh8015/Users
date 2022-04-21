using System;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;
using UserBusinessEntities;

namespace Users.Controllers
{
	public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult UserListData(int deletedFlag = 1)
        {
            string draw = Request.Form.GetValues("draw")[0];
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var search = Request.Form.GetValues("search[value]").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            if (draw.Equals("1"))
            {
                sortColumn = " FirstName ";
                sortColumnDir = " asc ";
            }
            else
            {
                if (sortColumn.Equals("FirstName"))
                {
                    if (sortColumnDir.Equals("desc"))
                    {
                        sortColumnDir = " asc ";
                    }
                    else
                    {
                        sortColumnDir = " desc ";
                    }
                }
            }
           

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            Int32 recordsTotal = 0;

            int pageNo = (Convert.ToInt32(start) / Convert.ToInt32(length)) <= 0 ? 1 : (Convert.ToInt32(start) / Convert.ToInt32(length)) + 1;
            var SortColoumn = sortColumn + " " + sortColumnDir;
            UserListcs salesInvoiceList = new UserListcs();
            GetUserList List = new GetUserList();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44353/");

                SPParametersBE parameters = new SPParametersBE();
                var invoice = new GetUserList();
               
                parameters.PageNo = pageNo;
                parameters.PageSize = pageSize;
                parameters.SortColumn = SortColoumn;
                var WhereClause = "";
                if (!string.IsNullOrWhiteSpace(search))
                {
                    WhereClause = " (FirstName like '%" + search + "%' OR DateOfBirth like '%" + search + "%' OR LastName like '%" + search + "%' OR EmailId like '%" + search + "%' ";
                    
                    WhereClause += ")";
                }
                
                parameters.WhereClause = WhereClause;

                int totalRecords = parameters.TotalRecord;

                var response = client.PostAsJsonAsync<SPParametersBE>("api/User/GetUserList/", parameters);

                response.Wait();

                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<UserListcs>();
                    readTask.Wait();

                    salesInvoiceList = readTask.Result;

                }

            }
            var data = salesInvoiceList.list;
            recordsTotal = salesInvoiceList.TotalRecords;

            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal, data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}