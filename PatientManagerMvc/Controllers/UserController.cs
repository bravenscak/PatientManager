using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PatientManagerClassLibrary;
using PatientManagerClassLibrary.Models;
using PatientManagerClassLibrary.Security;
using PatientManagerMvc.Models;
using System.Security.Claims;

namespace PatientManagerMvc.Controllers
{
    public class UserController : Controller
    {
        private readonly PatientManagerContext _context;

        public UserController(PatientManagerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login(string returnUrl)
        {
            ViewData["HideNavbar"] = true;
            var userLoginViewModel = new UserLoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginViewModel userLoginViewModel)
        {
            var existingUser = _context.Users
                .AsEnumerable()
                .FirstOrDefault(u => u.Username.Equals(userLoginViewModel.Username, StringComparison.OrdinalIgnoreCase));
            if (existingUser == null)
            {
                ModelState.AddModelError("Username", "Invalid username");
                return View();
            }

            var passwordHash = PasswordHashProvider.GetHash(userLoginViewModel.Password, existingUser.PasswordSalt);
            if (passwordHash != existingUser.PasswordHash)
            {
                ModelState.AddModelError("Password", "Invalid password");
                return View();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, userLoginViewModel.Username)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            Task.Run(async () =>
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties)
            ).GetAwaiter().GetResult();

            if (!string.IsNullOrEmpty(userLoginViewModel.ReturnUrl))
            {
                return View(userLoginViewModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult Logout()
        {
            Task.Run(async () =>
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme)
            ).GetAwaiter().GetResult();

            return View();
        }

        public IActionResult Register()
        {
            ViewData["HideNavbar"] = true;
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public ActionResult Register(UserViewModel userViewModel)
        {
            var trimmedUsername = userViewModel.Username.Trim();

            if (_context.Users.Any(u => u.Username.Equals(trimmedUsername)))
            {
                ModelState.AddModelError("Username", "Username taken");
                return View();
            }

            return RedirectToAction("ConfirmRegistration", userViewModel);
        }

        public ActionResult ConfirmRegistration(UserViewModel userViewModel)
        {
            return View(userViewModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult CompleteRegistration(UserViewModel userViewModel)
        {
            var b64salt = PasswordHashProvider.GetSalt();
            var b64hash = PasswordHashProvider.GetHash(userViewModel.Password, b64salt);

            var user = new User
            {
                Username = userViewModel.Username,
                PasswordHash = b64hash,
                PasswordSalt = b64salt,
                FirstName = userViewModel.FirstName,
                LastName = userViewModel.LastName,
                Email = userViewModel.Email,
                PhoneNumber = userViewModel.PhoneNumber
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Registration successful! You can now log in.";

            return RedirectToAction("Login");
        }
    }
}
