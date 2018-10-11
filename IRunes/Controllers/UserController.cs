using HTTP.Cookies;
using HTTP.Requests.Contracts;
using HTTP.Responses.Contracts;
using IRunes.Extensions;
using IRunes.Models;
using IRunes.Services;
using IRunes.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebServer.Results;

namespace IRunes.Controllers
{
    public class UserController : BaseController
    {
        private readonly IHashService hashService;

        public UserController()
        {
            this.hashService = new HashSerice();
        }
        
        public IHttpResponse Login(IHttpRequest request)
        {
            var username = this.GetUsername(request);
            if (username != null)
            {
                this.ViewBag["@@username"] = username;
                return this.View("Index");
            }
            return this.View("User/Login");
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim().UrlDecode();
            var password = request.FormData["password"].ToString().UrlDecode();

            var hashedPassword = this.hashService.Hash(password);

            var user = this.Context.Users.FirstOrDefault(u =>
                u.Username == username &&
                u.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password!");
            }

            this.IsUserAuthenticated = true;

            this.ViewBag["@@username"] = username;  

            request.Session.AddParameter("username", username);

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);

            var response = new RedirectResult("/");

            var cookie = new HttpCookie(".auth-IRunes", cookieContent, 7) { HttpOnly = true };
            response.Cookies.Add(cookie);

            return response;
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            var username = this.GetUsername(request);

            if (username == null)
            {
                return new RedirectResult("/");
            }

            var response = new RedirectResult("/");
            var cookie = request.Cookies.GetCookie(".auth-IRunes");
            cookie.Delete();
            response.Cookies.Add(cookie);
            return response;
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View("User/Register");
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim().UrlDecode();
            var password = request.FormData["password"].ToString().UrlDecode();
            var confirmPassword = request.FormData["confirmPassword"].ToString().UrlDecode();
            var email = request.FormData["email"].ToString().UrlDecode();

            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            {
                return this.BadRequestError("Please provide valid username with length of 4 or more characters.");
            }

            if (this.Context.Users.Any(u => u.Username == username))
            {
                return this.BadRequestError("User with the same name already exists.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return this.BadRequestError("Please provide password of length 6 or more.");
            }

            if (password != confirmPassword)
            {
                return this.BadRequestError("Passwords do not match.");
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
                return this.ServerError(e.Message);
            }

            return new RedirectResult("/");
        }
    }
}
