using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASSIGNMENT.Models;
using Microsoft.AspNetCore.Http;
using System.Data;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace ASSIGNMENT.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeDbContext _context;

        public EmployeesController(EmployeeDbContext context)
        {
            
  
            _context = context;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            

                var userID = HttpContext.Session.GetInt32("userID");
            if (userID != null)
            {
                ViewBag.ID = userID;
                return View(await _context.Employees.Where(userId => userId.AdminID.Equals(userID)).ToListAsync());
            }
            return RedirectToAction("Index", "Users");
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetInt32("userID") != null)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employees
                    .SingleOrDefaultAsync(m => m.EmployeeID == id);
                if (employee == null)
                {
                    return NotFound();
                }

                return View(employee);
            }

                return NotFound();
            
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetInt32("userID") != null)
            {
                return View();
            }
            return NotFound();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
          public async Task<IActionResult> Create([Bind("EmployeeID,FirstName,LastName,Gender,EmStatus,MyProperty")] Employee employee)

        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user = _context.Users.Where(userid => userid.UserID.Equals(HttpContext.Session.GetInt32("userID"))).FirstOrDefault();
                user.UserID = (int)HttpContext.Session.GetInt32("userID");             
                employee.AdminID = user;
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetInt32("userID") != null)
            {
                if (id == null)
                {
                    return NotFound();
                }

                var employee = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeID == id);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
            return NotFound();
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeID,FirstName,LastName,Gender,EmStatus,MyProperty")] Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (HttpContext.Session.GetInt32("userID") != null) { 
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .SingleOrDefaultAsync(m => m.EmployeeID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
            }
            return NotFound();
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.SingleOrDefaultAsync(m => m.EmployeeID == id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            if (HttpContext.Session.GetInt32("userID") != null)
            {
                HttpContext.Session.SetString("userID", "");
                return RedirectToAction("Index", "Users");
            }
            return NotFound();
        }

        public ActionResult ExportStatisticalReport() {
            var userID = HttpContext.Session.GetInt32("userID");
            var Male = _context.Employees.Where(active_users => active_users.Gender==0 && active_users.AdminID.Equals(userID) && active_users.EmStatus ==0).Count();
            var Female = _context.Employees.Where(active_users => active_users.Gender!=0 && active_users.AdminID.Equals(userID) && active_users.EmStatus == 0).Count();
            var maxDate = _context.Employees.Where(user => user.AdminID.Equals(userID) && user.EmStatus == 0).Max(bod => bod.MyProperty);
            var minDate = _context.Employees.Where(user=>user.AdminID.Equals(userID) && user.EmStatus==0).Min(dop => dop.MyProperty);



            var tbl = new System.Data.DataTable("EmpExcelStatistical");
            tbl.Columns.Add("Number Of Female", typeof(string));
            tbl.Columns.Add("Number Of Male", typeof(string));
            tbl.Columns.Add("Largest Employee", typeof(string));
            tbl.Columns.Add("Youngest Employee", typeof(string));
            DataRow row = tbl.NewRow();
                 row["Number Of Female"] = Female;
                 row["Number Of Male"] = Male;
                 row["Largest Employee"] = maxDate;
                 row["Youngest Employee"] = minDate;
                 tbl.Rows.Add(row);
            

            var memorystrream = new MemoryStream();
            var excelPackage = new ExcelPackage(memorystrream);
            var worksheet = excelPackage.Workbook.Worksheets.Add("StatisticalINFO");
            worksheet.Cells["A1"].LoadFromDataTable(tbl, true, TableStyles.None);
            worksheet.Cells["A1:D"].Style.Font.Bold = true;
            worksheet.DefaultRowHeight = 20;

            worksheet.Column(1).AutoFit();
            worksheet.Column(2).AutoFit();
            worksheet.Column(3).AutoFit();
            worksheet.Column(4).AutoFit();



            byte[] data = excelPackage.GetAsByteArray() as byte[];
            return File(data, "application/octet-stream", "StatisticalReport.xlsx");

        }

        public ActionResult ExportExcel() {
              var userID = HttpContext.Session.GetInt32("userID");
               var EmployeeInfo = _context.Employees.Where(employee =>employee.AdminID.Equals(userID) && employee.EmStatus==0);

               var tbl =new System.Data.DataTable("EmpExcel");
               tbl.Columns.Add("First Name", typeof(string));
               tbl.Columns.Add("Last Name", typeof(string));
               tbl.Columns.Add("Gender", typeof(string));
               foreach (var employee in EmployeeInfo) {
                   DataRow row = tbl.NewRow();
                   row["First Name"] = employee.FirstName;
                   row["Last Name"] = employee.LastName;
                   row["Gender"] =employee.Gender;
                   tbl.Rows.Add(row);
               }
             
            var memorystrream = new MemoryStream();
            var excelPackage = new ExcelPackage(memorystrream);
                var worksheet = excelPackage.Workbook.Worksheets.Add("EmployeeINFO");
                worksheet.Cells["A1"].LoadFromDataTable(tbl, true, TableStyles.None);
                worksheet.Cells["A1:C"].Style.Font.Bold = true;
                worksheet.DefaultRowHeight = 20;

                worksheet.Column(1).AutoFit();
                 worksheet.Column(2).AutoFit();
                 worksheet.Column(3).AutoFit();
                byte[] data = excelPackage.GetAsByteArray() as byte[];
                return File(data, "application/octet-stream", "EmployeeInfo.xlsx");
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeID == id);
        }
    }
}
