﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Dynamic;
using System.Net;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebApplication4.Data;
using WebApplication4.DataContext;
using WebApplication4.Models;
namespace WebApplication4.Controllers
{


    public class UserController : Controller
    {
        private ApplicationDbUsers db = new ApplicationDbUsers();
        PlaylistContext PCon = new PlaylistContext();

        
        // GET: User
        public ActionResult Index()
        {

            return View();
        }

        // GET: User/Details/5
      

        // GET: User/Create
        public ActionResult Create()
        {

            return View();
        }

        public ActionResult Profile()
        {

            var newSession = Session["login"] as string;
            if (newSession != "logged in")
            {
                return RedirectToAction("SignUp", "User");
            }

            string username = Session["username"] as string;

            UserDAO UserDAO = new UserDAO();
            PlayListDAO playListDAO = new PlayListDAO();
            BigModel objModel = new BigModel();
            objModel.UserModel = UserDAO.Fetch(username);
            objModel.PlaylistModel = new PlaylistModel();
            objModel.PlaylistModel = playListDAO.PlaylistFetch(username);
            

            return View("Profile", objModel);
            
        }



        public ActionResult SignUp()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }


        public ActionResult Login()
        {
           
            return View();
        }

        public ActionResult SearchResults()
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

        public ActionResult Song()
        {
            string username = Session["username"].ToString();
            PlaylistModel PD = new PlaylistModel();
            PD.PlayListData = new SelectList(PCon.GetPlaylists(username), "playlistname", "playlistname"); 
            return View(PD);
        }

        
        

        public ActionResult Album()
        {
            return View();
        }

        public ActionResult CreatePlaylist()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "email,password, name, username")] UserModel userModel)
        {
            try
            {
                var sql = "INSERT INTO public.users(email,password, name, username) VALUES(@email, crypt(@password, gen_salt('bf')), @name, @username)";
                var conn = "Host=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;password=Ffgtte??";

                var newConn = new NpgsqlConnection(conn);
                newConn.Open();
                var cmd = new NpgsqlCommand(sql, newConn);

                cmd.Parameters.AddWithValue("email", userModel.email);
                cmd.Parameters.AddWithValue("password", userModel.password);
                cmd.Parameters.AddWithValue("name", userModel.name);
                cmd.Parameters.AddWithValue("username", userModel.username);
                // db.UserObj.Add(userModel);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                db.SaveChanges();
                return RedirectToAction("HomePage");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorContent = "<p>Error. Try another email or username!<p>";
                
            }

            return View(userModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Login([Bind(Include = "email,password,name,username")] UserModel userModel)
        {
            ViewBag.loggedIn = "Not Logged in";
            if (ModelState.IsValid)
            {
                var sql = "select count(*) from public.users where username=@username and password = crypt(@password, password); ";
                var conn = "Host=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;password=Ffgtte??";

                var newConn = new NpgsqlConnection(conn);
                newConn.Open();
                var cmd = new NpgsqlCommand(sql, newConn);
                var username = userModel.username;
                Session["username"] = username;
                System.Diagnostics.Debug.WriteLine(">>>>>>>>>" + username);
                cmd.Parameters.AddWithValue("username", userModel.username);
                cmd.Parameters.AddWithValue("password", userModel.password);
                Int64 count = (Int64)cmd.ExecuteScalar();
                
                if (count > 0)
                {

                    TempData["username"] = username;

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
        public ActionResult Create([Bind(Include = "id,email,password")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var sql = "INSERT INTO public.users(email,password) VALUES(@email, crypt(@password, gen_salt('bf')))";
                    var conn = "Host=localhost;Port=5432;Database=users;User Id=admin;password=secret";

                    var newConn = new NpgsqlConnection(conn);
                    newConn.Open();
                    var cmd = new NpgsqlCommand(sql, newConn);

                    cmd.Parameters.AddWithValue("email", userModel.email);
                    cmd.Parameters.AddWithValue("password", userModel.password);
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePlaylist([Bind(Include = "username,trackname,playlistname,genre,artist")] PlaylistModel playlistModel)
        {
            try
            {
                var sql = "INSERT INTO public.playlists(username, playlistname) select @username, @playlistname where not exists (select playlistname from public.playlists where playlistname = @playlistname)";
                var conn = "Host=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;password=Ffgtte??";

                var newConn = new NpgsqlConnection(conn);
                newConn.Open();
                var cmd = new NpgsqlCommand(sql, newConn);

                string username = Session["username"].ToString();
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("playlistname", playlistModel.playlistname);
                
                // db.UserObj.Add(userModel);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                db.SaveChanges();
                return RedirectToAction("Profile");

            }
            catch (Exception ex)
            {
                ViewBag.ErrorContent = "<p>This is already a playlist.<p>";


            }

            return View(playlistModel);
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
        public ActionResult Edit([Bind(Include = "id,email,password")] UserModel userModel)
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


        
        public ActionResult TestFunction(string playlistname,string trackname, string genre, string artist, string img, PlaylistModel playlistModel)
        {
            try
            {
                string username = Session["username"].ToString();

                var sql = "Insert into public.playlists (username, trackname,playlistname,genre,artist,img) VALUES(@username, @trackname, @playlistname, @genre, @artist, @img)";
                var conn = "Server=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;Password=Ffgtte??";

                var newConn = new NpgsqlConnection(conn);
                newConn.Open();
                        var cmd = new NpgsqlCommand(sql, newConn);

                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("trackname", trackname);
                cmd.Parameters.AddWithValue("playlistname", playlistname);
                cmd.Parameters.AddWithValue("genre", genre);
                cmd.Parameters.AddWithValue("artist", artist);
                cmd.Parameters.AddWithValue("img", img);
                // db.UserObj.Add(userModel);
                cmd.Prepare();
                        cmd.ExecuteNonQuery();
                        db.SaveChanges();
    }
            catch (Exception ex)
            {
                ViewBag.ErrorContent = "<p>Error. Try another email!<p>";
            }

            return Json(new
            {
                result = "ok"
            });

      }


        public ActionResult Logout()
        {
            Session["Login"] = "not logged in";
            Session["username"] = "";
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
