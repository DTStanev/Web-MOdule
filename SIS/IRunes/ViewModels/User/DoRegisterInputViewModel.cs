namespace IRunes.ViewModels.User
{
    public class DoRegisterInputViewModel
    {
        public string Username { get; private set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string Email { get; set; }
    }
}
