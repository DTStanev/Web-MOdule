using HTTP.Cookies;
using HTTP.Responses.Contracts;
using IRunes.Models;
using IRunes.ViewModels;
using MvcFramework.Extensions;
using MvcFramework.HttpAttributes;
using MvcFramework.Services;
using MvcFramework.Services.Contracts;
using System;
using System.Linq;

namespace IRunes.Controllers
{
    public class UserController : BaseController
    {
        private readonly IHashService hashService;

        public UserController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        [HttpGet("/users/login")]
        public IHttpResponse Login()
        {
            if (this.User != null)
            {
                this.ViewBag["@@username"] = this.User;
                return this.Redirect("/");
            }

            this.ViewBag["@showError"] = "none";

            return this.View("Login");
        }

        [HttpPost("/users/login")]
        public IHttpResponse DoLogin(DoLoginInputViewModel model)
        {
            var username = model.Username.Trim().UrlDecode();
            var password = model.Password.UrlDecode();

            var hashedPassword = this.hashService.Hash(password);

            var user = this.Context.Users.FirstOrDefault(u =>
                u.Username == username &&
                u.Password == hashedPassword);

            string errorMessage = string.Empty;

            IHttpResponse response = null;

            if (user == null)
            {
                this.ViewBag["@showError"] = "";
                this.ViewBag["@errorMessage"] = "Invalid username or password!";
                return this.View("Login");
            }

            this.ViewBag["@@username"] = username;

            this.Request.Session.AddParameter("username", username);

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);

            response = this.Redirect("/");

            var cookie = new HttpCookie(".auth-IRunes", cookieContent, 7) { HttpOnly = true };
            response.Cookies.Add(cookie);

            return response;
        }

        [HttpGet("/users/logout")]
        public IHttpResponse Logout()
        {
            if (this.User == null)
            {
                return this.Redirect("/");
            }

            var response = this.Redirect("/");
            var cookie = this.Request.Cookies.GetCookie(".auth-IRunes");
            cookie.Delete();
            response.Cookies.Add(cookie);
            return response;
        }

        [HttpGet("/users/register")]
        public IHttpResponse Register()
        {
            if (this.User != null)
            {
                return this.Redirect("/");
            }

            this.ViewBag["@showError"] = "none";

            return this.View("Register");
        }

        public IHttpResponse DoRegister(DoRegisterInputViewModel model)
        {
            var username = model.Username.Trim().UrlDecode();
            var password = model.Password.UrlDecode();
            var confirmPassword = model.ConfirmPassword.UrlDecode();
            var email = model.Email.UrlDecode();



            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            {
                this.ViewBag["@showError"] = "";
                this.ViewBag["@errorMessage"] = "Please provide valid username with length of 4 or more characters.";
                return this.View("Register");
            }

            if (this.Context.Users.Any(u => u.Username == username))
            {
                this.ViewBag["@showError"] = "";
                this.ViewBag["@errorMessage"] = "User with the same name already exists.";
                return this.View("Register");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                this.ViewBag["@showError"] = "";
                this.ViewBag["@errorMessage"] = "Please provide password of length 6 or more characters.";
                return this.View("Register");
            }

            if (password != confirmPassword)
            {
                this.ViewBag["@showError"] = "";
                this.ViewBag["@errorMessage"] = "Passwords do not match.";
                return this.View("Register");
            }

            var hashedPassword = this.hashService.Hash(password);

            var user = new User
            {
                Username = username,
                Password = hashedPassword,
                Email = email
            };

            this.Context.Users.Add(user);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
               // return this.ServerError(e.Message);
            }

            return this.Redirect("/");
        }
    }
}
