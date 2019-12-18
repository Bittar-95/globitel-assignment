using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASSIGNMENT.Models;
using Microsoft.AspNetCore.Http;

namespace ASSIGNMENT.Controllers
{
    public class UsersController : Controller
    {
        private readonly EmployeeDbContext _context;

        public UsersController(EmployeeDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string username ,string password)
        {
            var userInfo = _context.Users.Where(user_auth => user_auth.UserName == username && user_auth.Password == password).FirstOrDefault();
            if (userInfo != null)
            {
                HttpContext.Session.SetInt32("userID", userInfo.UserID);
                return RedirectToAction("Index", "Employees");
            }
            return View();
            
         
        }



        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }


   

    

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserID,UserName,Password,ConfirmPassword")] User user)
        {
            if (ModelState.IsValid)
            {
                TempData["message"] = "Congratulations You Can Login Now!";
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit()
        {
            var id = HttpContext.Session.GetInt32("userID");
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.SingleOrDefaultAsync(m => m.UserID == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserID,UserName,Password,ConfirmPassword")] User user)
        {
            if (id != user.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.UserID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index","Employees");
            }
            return View(user);
        }
        public IActionResult Contact() {
            return View();
        }



        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserID == id);
        }
    }
}
