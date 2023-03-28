using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
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

        private readonly MockDatabaseConnection _connection;
        
        public UserController()
        {
            var connectionString = "Host=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;password=Ffgtte??;";
            _connection = new NpgsqlDatabaseConnection(connectionString);
        }
        public UserController(MockDatabaseConnection connection)
        {
            _connection = connection;
        }
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


            var newSession = Session["login"] as string;
            if (newSession != "logged in")
            {
                return RedirectToAction("SignUp", "User");
            }

            string username = Session["username"] as string;

            UserDAO UserDAO = new UserDAO();
            BigModel objModel = new BigModel();
            objModel.UserModel = UserDAO.Fetch(username);
            return View(objModel);
        }


        public ActionResult Login()
        {
           
            return View();
        }

        public ActionResult SearchResults()
        {
            return View();
        }

        public ActionResult HomePageNL()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult HomePage()
        {
            var newSession = Session["login"] as string;
            if (newSession != "logged in")
            {
                return RedirectToAction("HomePageNL", "User");
            }

            string username = Session["username"] as string;

            UserDAO UserDAO = new UserDAO();
            PlayListDAO playListDAO = new PlayListDAO();
            BigModel objModel = new BigModel();
            objModel.UserModel = UserDAO.Fetch(username);
            objModel.PlaylistModel = new PlaylistModel();
            objModel.PlaylistModel = playListDAO.PlaylistFetch(username);


            return View("HomePage", objModel);
        }

        public ActionResult Song()
        {
            var newSession = Session["login"] as string;
            var track = Session["track"] as string;
            if (newSession != "logged in")
            {
                return RedirectToAction("SignUp", "User");
            }



            string username = Session["username"].ToString();
            ReviewDAO reviewDAO = new ReviewDAO();
            UserDAO UserDAO = new UserDAO();
            BigModel objModel = new BigModel();
            RatingDAO ratingDAO = new RatingDAO();
            YourReviewDAO yourReview = new YourReviewDAO();
            objModel.PlaylistModel = new PlaylistModel();
            objModel.PlaylistModel.PlayListData = new SelectList(PCon.GetPlaylists(username), "playlistname", "playlistname");
            objModel.ReviewModel = reviewDAO.ReviewAll(track);
            objModel.RatingModel = ratingDAO.RatingFetch(track);
            objModel.YourReviewModel = yourReview.ReviewFetch(username, track);
            objModel.UserModel = UserDAO.Fetch(username);
            return View("Song", objModel);
        }

        public ActionResult Playlist(string playlistname)
        {
            string username = Session["username"].ToString();
            
            List<PlaylistModel> playlists = new List<PlaylistModel>();


            PlaylistFetchAll playlistFetching = new PlaylistFetchAll();

            playlists = playlistFetching.PlaylistAll(username, playlistname);
           

            return View("Playlist", playlists);
            
            
        }
        

        public ActionResult Album()
        {
            var newSession = Session["login"] as string;
            var track = Session["track"] as string;
            if (newSession != "logged in")
            {
                return RedirectToAction("HomePageNL", "User");
            }



            
            string username = Session["username"].ToString();
            ReviewDAO reviewDAO = new ReviewDAO();
            UserDAO UserDAO = new UserDAO();
            BigModel objModel = new BigModel();
            RatingDAO ratingDAO = new RatingDAO();
            YourReviewDAO yourReview = new YourReviewDAO();
            objModel.PlaylistModel = new PlaylistModel();
            objModel.PlaylistModel.PlayListData = new SelectList(PCon.GetPlaylists(username), "playlistname", "playlistname");
            objModel.ReviewModel = reviewDAO.ReviewAll(track);
            objModel.RatingModel = ratingDAO.RatingFetch(track);
            objModel.YourReviewModel = yourReview.ReviewFetch(username, track);
            objModel.UserModel = UserDAO.Fetch(username);
            return View("Album", objModel);
            
        }

        public ActionResult CreatePlaylist()
        {

            return View();
        }

        public ActionResult AddImage()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp([Bind(Include = "email,password, name, username, picture")] UserModel userModel)
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
               
                return RedirectToAction("Login");
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
                    ViewBag.ErrorContent = "<p>Error. Try another email or username!<p>";


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
                    cmd.Dispose();
                    newConn.Close();
                    newConn.Dispose();
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
        public ActionResult CreatePlaylist([Bind(Include = "username,trackname,playlistname,genre,artist, picture")] PlaylistModel playlistModel)
        {
            try
            {
               
                    var sql = "INSERT INTO public.playlists(username, playlistname, picture) select @username, @playlistname, @picture where not exists (select playlistname from public.playlists where playlistname = @playlistname)";
                    var conn = "Host=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;password=Ffgtte??";

                    var newConn = new NpgsqlConnection(conn);
                    newConn.Open();
                    var cmd = new NpgsqlCommand(sql, newConn);

                    string username = Session["username"].ToString();
                    cmd.Parameters.AddWithValue("username", username);
                    cmd.Parameters.AddWithValue("playlistname", playlistModel.playlistname);

                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        if (file != null && file.ContentLength > 0)
                        {
                            byte[] imageData = null;
                            using (var binaryReader = new BinaryReader(file.InputStream))
                            {
                                imageData = binaryReader.ReadBytes(file.ContentLength);
                            }
                            cmd.Parameters.AddWithValue("picture", imageData);
                        }
                    }

                


                // db.UserObj.Add(userModel);
                cmd.Prepare();
                    cmd.ExecuteNonQuery();
                cmd.Dispose();
                newConn.Close();
                newConn.Dispose();
                db.SaveChanges();
                TempData["ErrorMessage"] = "PlaylistCreated";
                return RedirectToAction("Profile");

            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Can't create playlist right now";
                return RedirectToAction("Profile");

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


        
        public ActionResult TestFunction(string playlistname,string trackname, string genre, string artist, string img, string track, PlaylistModel playlistModel)
        {
            try
            {
                string username = Session["username"].ToString();
                
                var sql = "Insert into public.playlists (username, trackname,playlistname,genre,artist,img, picture, trackid) VALUES(@username, @trackname, @playlistname, @genre, @artist, @img,(SELECT distinct picture FROM public.playlists WHERE playlistname = @playlistname), @trackid)";
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
                cmd.Parameters.AddWithValue("trackid", track);
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


        public ActionResult TrackId(string track)
        {
            try
            {
                Session["track"] = track;
                
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

        public ActionResult AddToReview(string track, string review)
        {
            try
            {
                string username = Session["username"].ToString();

                var sql = "UPDATE reviews SET trackid=@trackid, review_desc = @review_desc WHERE username = @username;INSERT INTO reviews (username, trackid, review_desc) SELECT @username, @trackid, @review_desc WHERE NOT EXISTS (SELECT 1 FROM reviews WHERE username = @username and trackid=@trackid);";
                var conn = "Server=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;Password=Ffgtte??";

                var newConn = new NpgsqlConnection(conn);
                newConn.Open();
                var cmd = new NpgsqlCommand(sql, newConn);

                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("trackid", track);
                cmd.Parameters.AddWithValue("review_desc", review);

                // db.UserObj.Add(userModel);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                
                
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                ViewBag.ErrorContent = "<p>This cannot be added at the moment<p>";
            }

            return Json(new
            {
                result = "ok"
            });

        }

        public ActionResult DeleteReview(string track)
        {
            try
            {
                string username = Session["username"].ToString();

                var sql = "DELETE from public.reviews where trackid=@track and username=@username;";
                var conn = "Server=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;Password=Ffgtte??";

                var newConn = new NpgsqlConnection(conn);
                newConn.Open();
                var cmd = new NpgsqlCommand(sql, newConn);

                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("track", track);
                

                // db.UserObj.Add(userModel);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                db.SaveChanges();

                Response.Redirect(Request.RawUrl);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorContent = "<p>This cannot be deleted at the moment<p>";
            }

            return Json(new
            {
                result = "ok"
            });

        }
        public ActionResult AddToRating(string track, int rating)
        {
            try
            {
                string username = Session["username"].ToString();

                var sql = "Insert into public.ratings VALUES(@username, @trackid, @rating)";
                var conn = "Server=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;Password=Ffgtte??";

                var newConn = new NpgsqlConnection(conn);
                newConn.Open();
                var cmd = new NpgsqlCommand(sql, newConn);

                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("trackid", track);
                cmd.Parameters.AddWithValue("rating", rating);

                // db.UserObj.Add(userModel);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                

                db.SaveChanges();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorContent = "<p>This cannot be added at the moment<p>";
            }

            return Json(new
            {
                result = "ok"
            });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult AddImage([Bind(Include = "email,password,name,username, picture")] UserModel userModel)
        {
            try
            {

                var sql = "UPDATE public.users set picture=@picture where username=@username";
                var conn = "Host=playback-db.postgres.database.azure.com;Port=5432;Database=users;User Id=ethanm1;password=Ffgtte??";

                var newConn = new NpgsqlConnection(conn);
                newConn.Open();
                var cmd = new NpgsqlCommand(sql, newConn);

                string username = Session["username"].ToString();
                cmd.Parameters.AddWithValue("username", username);

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(file.InputStream))
                        {
                            imageData = binaryReader.ReadBytes(file.ContentLength);
                        }
                        cmd.Parameters.AddWithValue("picture", imageData);
                    }
                }


                // db.UserObj.Add(userModel);
                cmd.Prepare();
                cmd.ExecuteNonQuery();
                
                db.SaveChanges();
                return RedirectToAction("Profile");


             }
                    
                catch (Exception ex)
            {
                ViewBag.ErrorContent = "<p>Can't add this.<p>";


            }

            return View(userModel);
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
