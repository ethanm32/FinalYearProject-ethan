﻿using Npgsql;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication4.DataContext;
using WebApplication4.Models;
namespace WebApplication4.Controllers
{


    public class UserController : Controller
    {
        private ApplicationDbUsers db = new ApplicationDbUsers();


        
        // GET: User
        public ActionResult Index()
        {
            return View(db.UserObj.ToList());
        }

        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModel userModel = db.UserObj.Find(id);
            if (userModel == null)
            {
                return HttpNotFound();
            }
            return View(userModel);
        }

        // GET: User/Create
        public ActionResult Create()
        {

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }


        public ActionResult SignUp()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult HomePage()
        {
            var newSession = Session["login"] as string;
            if (newSession != "logged in")
            {
                return RedirectToAction("SignUp", "User");
            }

            return View();
        }


        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "id,email,pass")] UserModel userModel)
        {
            try
            {
                var sql = "INSERT INTO public.user(email,pass) VALUES(@email, crypt(@pass, gen_salt('bf')))";
                var conn = "Host=localhost;Port=5432;Database=users;User Id=postgres;Password=ethan";

                var newConn = new NpgsqlConnection(conn);
                newConn.Open();
                var cmd = new NpgsqlCommand(sql, newConn);

                cmd.Parameters.AddWithValue("email", userModel.email);
                cmd.Parameters.AddWithValue("pass", userModel.pass);
                // db.UserObj.Add(userModel);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorContent = "<p>Error. Try another email!<p>";
            }

            return View(userModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login([Bind(Include = "id,email,pass")] UserModel userModel)
        {
            ViewBag.loggedIn = "Not Logged in";
            if (ModelState.IsValid)
            {
                var sql = "select count(*) from public.user where email=@email and pass = crypt(@pass, pass); ";
                var conn = "Host=localhost;Port=5432;Database=users;User Id=postgres;Password=ethan";

                var newConn = new NpgsqlConnection(conn);
                newConn.Open();
                var cmd = new NpgsqlCommand(sql, newConn);
                cmd.Parameters.AddWithValue("email", userModel.email);
                cmd.Parameters.AddWithValue("pass", userModel.pass);
                Int64 count = (Int64)cmd.ExecuteScalar();
                if (count > 0)
                {
                    Session["Login"] = "logged in";
                    Console.WriteLine("Logged in");
                    return RedirectToAction("HomePage");
                    
                }
                else
                {
                    Console.WriteLine("Not logged in");
                    Session["Login"] = "not logged in";

                }

            }


            return View(userModel);
        }

            // POST: User/Create
            // To protect from overposting attacks, enable the specific properties you want to bind to, for 
            // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
            [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,email,pass")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var sql = "INSERT INTO public.user(email,pass) VALUES(@email, crypt(@pass, gen_salt('bf')))";
                    var conn = "Host=localhost;Port=5432;Database=users;User Id=postgres;Password=ethan";

                    var newConn = new NpgsqlConnection(conn);
                    newConn.Open();
                    var cmd = new NpgsqlCommand(sql, newConn);

                    cmd.Parameters.AddWithValue("email", userModel.email);
                    cmd.Parameters.AddWithValue("pass", userModel.pass);
                    // db.UserObj.Add(userModel);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    db.SaveChanges();
                    return RedirectToAction("Index");
                } catch(Exception ex)
                {
                    ViewBag.ErrorContent = "<p>Error. Try another email!<p>";
                }
            }
            

            return View(userModel);
        }



        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModel userModel = db.UserObj.Find(id);
            if (userModel == null)
            {
                return HttpNotFound();
            }
            return View(userModel);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,email,pass")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userModel);
        }

        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserModel userModel = db.UserObj.Find(id);
            if (userModel == null)
            {
                return HttpNotFound();
            }
            return View(userModel);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserModel userModel = db.UserObj.Find(id);
            db.UserObj.Remove(userModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Logout()
        {
            Session["Login"] = "not logged in";
            return RedirectToAction("SignUp");

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}