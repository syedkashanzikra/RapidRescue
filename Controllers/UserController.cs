﻿using System.Net.Mail;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Mvc;
using RapidRescue.Context;
using RapidRescue.Models;
using RapidRescue.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using RapidRescue.Filters;


namespace RapidRescue.Controllers
{
    public class UserController : Controller
    {
        private readonly RapidRescueContext _context;
        private readonly IConfiguration _configuration;


        public UserController(RapidRescueContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        [HttpGet]
        [Route("/register")]
        public IActionResult Register_User()
        {

            return View();
        }

        [HttpGet]
        public IActionResult RegistrationConfirmation()
        {
            return View();  
        }


        private async Task SendVerificationEmail(Users user)
        {
            var emailSettings = _configuration.GetSection("EmailSettings");

            var verifyUrl = Url.Action("VerifyEmail", "User", new { id = user.User_id }, protocol: HttpContext.Request.Scheme);
            var message = new MailMessage
            {
                From = new MailAddress(emailSettings["SenderEmail"], emailSettings["SenderName"]),
                Subject = "Complete Your Registration",
                Body = $"Please verify your account by clicking <a href='{verifyUrl}'>here</a>.",
                IsBodyHtml = true,
            };
            message.To.Add(new MailAddress(user.Email));

            using (var smtp = new SmtpClient(emailSettings["SmtpServer"]))
            {
                smtp.Port = int.Parse(emailSettings["SmtpPort"]);
                smtp.Credentials = new System.Net.NetworkCredential(emailSettings["Username"], emailSettings["Password"]);
                smtp.EnableSsl = bool.Parse(emailSettings["EnableSsl"]);

                try
                {
                    await smtp.SendMailAsync(message);
                }
                catch (SmtpException ex)
                {
                    Console.WriteLine("SMTP Exception: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("General Exception: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }
                }
            }
        }



        


        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register_User(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the email already exists
                var emailExists = await _context.Users.AnyAsync(u => u.Email == model.Email);
                if (emailExists)
                {
                    ModelState.AddModelError("Email", "An account with this email already exists.");
                    return View(model);
                }

                var passwordHasher = new PasswordHasher<Users>();

                var user = new Users
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = false,
                    RememberToken = "",
                    Role_Id = 2,
                };

                user.Password = passwordHasher.HashPassword(user, model.Password);

                _context.Add(user);
                await _context.SaveChangesAsync();

                await SendVerificationEmail(user);

                TempData["Message"] = "Your Token Has been Generated Go to Your Email!";
                return RedirectToAction("RegistrationConfirmation");
            }

            return View(model);
        }

        private string GenerateRandomToken()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[32];
                rng.GetBytes(randomBytes);
                return Convert.ToBase64String(randomBytes);
            }
        }




        [HttpGet]
        public async Task<IActionResult> VerifyEmail(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
         
                user.RememberToken = GenerateRandomToken();
                user.IsActive = true;
                _context.Update(user);
                await _context.SaveChangesAsync();

                TempData["Message"] = "Your Account Has Been Activated Now You Can Securely Login!";
                return View("TokenDisplay", user);  
            }
            return NotFound();  
        }

        [HttpGet]
        [Route("/login")]     
        public IActionResult Login_User()
        {

            return View();
        }



        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login_User(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }

                var passwordHasher = new PasswordHasher<Users>();
                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.Password, model.Password);

                if (passwordVerificationResult == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
                }

               
                HttpContext.Session.SetInt32("user_id", user.User_id);
                HttpContext.Session.SetInt32("role_id", user.Role_Id);

              
                return RedirectToAction("Home", "Home");
            }

            
            return View(model);
        }

        [HttpPost]
        [Route("/logout")]
        public IActionResult Logout_User()
        {
            // Clear the session to log the user out
            HttpContext.Session.Clear();

            // Optionally, delete session cookies
            Response.Cookies.Delete(".AspNetCore.Session");

            // You can return a status code or an empty result for the AJAX success handler
            return Ok();
        }


        [HttpGet]
        [Route("/edit-profile")]
        [ServiceFilter(typeof(UserSessionCheckAttribute))]
        public async Task<IActionResult> EditProfile()
        {
            // Fetch the user ID from session
            int? userId = HttpContext.Session.GetInt32("user_id");

            if (userId == null)
            {
                // If no user is logged in, redirect to login page
                return RedirectToAction("Login_User", "User");
            }

            // Get the user from the database using the session user_id
            var user = await _context.Users.FirstOrDefaultAsync(u => u.User_id == userId);

            if (user == null)
            {
                // If the user does not exist, redirect to an error page or home
                return RedirectToAction("Home", "Home");
            }

            // Prepare the EditProfileViewModel with the user's current information
            var model = new EditProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                // Password is left empty intentionally so the user can update it if they want
                Password = ""
            };

            return View(model); // Pass the model to the view
        }

        [HttpPost]
        [Route("/edit-profile")]
        [ServiceFilter(typeof(UserSessionCheckAttribute))]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                int? userId = HttpContext.Session.GetInt32("user_id");

                if (userId == null)
                {
                    return RedirectToAction("Login_User", "User");
                }

                var user = await _context.Users.FirstOrDefaultAsync(u => u.User_id == userId);

                if (user == null)
                {
                    return RedirectToAction("Home", "Home");
                }

                // Update user details
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;

                // Only update the password if a new password is provided
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    var passwordHasher = new PasswordHasher<Users>();
                    user.Password = passwordHasher.HashPassword(user, model.Password);
                }

                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction("Admin", "Admin"); // Redirect to user profile page
            }

            return View(model);
        }

    }
}
