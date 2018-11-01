namespace IRunes.Controllers
{
    using HTTP.Cookies;
    using HTTP.Responses.Contracts;
    using Models;
    using MvcFramework.HttpAttributes;
    using MvcFramework.Services.Contracts;
    using System;
    using System.Linq;
    using ViewModels.User;

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
                return this.Redirect("/");
            }


            return this.View("Login");
        }

        [HttpPost("/users/login")]
        public IHttpResponse DoLogin(DoLoginInputViewModel model)
        {
            var username = model.Username.Trim();
            var password = model.Password;

            var hashedPassword = this.hashService.Hash(password);

            var user = this.Context.Users.FirstOrDefault(u =>
                u.Username == username &&
                u.Password == hashedPassword);
            
            if (user == null)
            {
                return this.BadRequestError("Login", "Invalid Username or Password");
            }

            this.Request.Session.AddParameter("username", username);

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);

            var cookie = new HttpCookie(".auth-IRunes", cookieContent, 7) { HttpOnly = true };
            this.Response.Cookies.Add(cookie);

            return this.Redirect("/");
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

            return this.View("Register");
        }

        [HttpPost("/users/register")]
        public IHttpResponse DoRegister(DoRegisterInputViewModel model)
        {
            var username = model.Username.Trim();
            var password = model.Password;
            var confirmPassword = model.ConfirmPassword;
            var email = model.Email;
            
            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            {
                return this.BadRequestError("Register", "Please provide valid username with length of 4 or more characters.");
            }

            if (this.Context.Users.Any(u => u.Username == username))
            {
                return this.BadRequestError("Register", "User with the same name already exists.");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return this.BadRequestError("Register", "Please provide password of length 6 or more characters.");

            }

            if (password != confirmPassword)
            {
                return this.BadRequestError("Register", "Passwords do not match.");

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

            return this.Redirect("/");
        }
    }
}
