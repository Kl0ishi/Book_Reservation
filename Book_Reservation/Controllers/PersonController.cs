using Book_Reservation.Interface;
using Book_Reservation.Interface.AdminResult;
using Book_Reservation.Interface.jwt;
using Book_Reservation.Interface.UserResult;
using Book_Reservation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Book_Reservation.Controllers
{
    public class PersonController : Controller
    {
        private readonly ReservationDevContext _context;
        private readonly InterfaceApi _interfaceApi;
        public PersonController(ReservationDevContext context, InterfaceApi interfaceApi)
        {
            _context = context;
            _interfaceApi = interfaceApi;
        }
        public async Task<IActionResult> Index()
        {
            return _context.People != null ?
            View(await _context.People.ToListAsync()) :
            Problem("Entity set 'ReservationDevContext.BookStocks'  is null.");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Person obj)
        {
            if (ModelState.IsValid)
            {
                _context.People.Add(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _context.People.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Person obj)
        {
            if (ModelState.IsValid)
            {
                _context.People.Update(obj);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var obj = await _context.People
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            if (_context.People == null)
            {
                return Problem("Entity set 'ReservationDevContext.People'  is null.");
            }
            var person = await _context.People.FindAsync(id);
            if (person != null)
            {
                _context.People.Remove(person);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.People == null)
            {
                return NotFound();
            }

            var person = await _context.People
                .FirstOrDefaultAsync(m => m.PersonId == id);
            if (person == null)
            {
                return NotFound();
            }

            return View(person);
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string PersonCode, string Password)
        {
            try
            {
                // Intentionally throw an exception for testing
                //throw new Exception("This is a test exception for error handling.");

                // Check insider credentials using the API
                //bool isInsider = await CheckInsiderRamaCredentialsWithApi(PersonCode, Password);
                bool isUserValid = await CheckUserCredentialsWithApi(PersonCode, Password);

                if (isUserValid)
                {
                    UserPass userPass = new UserPass
                    {
                        PersonCode = PersonCode,
                        Password = Password
                    };

                    AuthResult token = await _interfaceApi.GetToken(userPass);
                    Useritem useritem = await _interfaceApi.GetUserInfo(token.Token);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["PreservedPersonCode"] = PersonCode;
                    TempData["ErrorMessage"] = "Invalid login attempt.";
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                string io = "1";
                var apiStatus = _context.Flagnotis.FirstOrDefaultAsync(u => u.FlagnotiName == io);
                if(apiStatus != null)
                {
                    // Log the exception for debugging
                    Console.WriteLine($"An error occurred during login: {ex.Message}");

                    // Send Line Notify notification
                    await _interfaceApi.LineNotifyAsync("P3LqOb3JsZDs2B22CHTugGVIpdVkG1oi6NFGz9exlTg", "Fumo Fumo ๐w๐");
                }
                else
                {
                    Console.WriteLine("Line notify ไม่ใช้แล้ว");
                }
                
                throw;
            }
            
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Person person, string ConfirmPassword)
        {
            var existingUser = await _context.People.FirstOrDefaultAsync(u => u.PersonCode == person.PersonCode);
            if (existingUser != null)
            {
                TempData["ErrorMessage"] = "User with this PersonUsersID already exists.";
            }
            else if (ConfirmPassword == null)
            {
                TempData["ErrorMessage"] = "Please ConfirmPassword";
            }
            // Check if the passwords match
            else if (person.Password != ConfirmPassword)
            {
                TempData["ErrorMessage"] = "Passwords do not match.";
            }
            else
            {
                string salt = GetSalt();
                person.Salt = salt;

                string hashedPassword = CalculateSHA256Hash(person.Password + salt);
                person.Password = hashedPassword;

                _context.People.Add(person);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }

            return View("Register", person);

        }

        public static string CalculateSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2")); // Convert to hexadecimal representation
                }
                return sb.ToString();
            }
        }

        private static string GetSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        //private async Task<bool> CheckInsiderRamaCredentialsWithApi(string personCode, string password)
        //{
        //    Adminparam adminParams = new Adminparam
        //    {
        //        user = personCode,
        //        password = password,
        //        system = "appRFID",
        //        ipAddres = "10.6.165.559",
        //        terminal = "MB-C180256-B"
        //    };

        //    try
        //    {
        //        Resultloginitem resultLoginItem = await _interfaceApi.FloginAdminRama(adminParams);

        //        // Check the "result" field in the response
        //        bool result = resultLoginItem.result;
                
        //        return await HandleLoginLogic(personCode, result, resultLoginItem.resultDetails.fullName_TH);

        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}


        //private async Task<bool> HandleLoginLogic(string personCode, bool result, string fullname)
        //{
        //    var existingUser = await _context.People.FirstOrDefaultAsync(u => u.PersonCode == personCode);

        //    if (existingUser != null)
        //    {
        //        if (result)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Invalid login attempt.";
        //            return false;
        //        }

        //    }
        //    else
        //    {
        //        if (result)
        //        {
        //            await AddInsiderToDatabase(personCode, fullname);
        //            return true;
        //        }
        //        else
        //        {
        //            TempData["ErrorMessage"] = "Invalid login attempt.";
        //            return false;
        //        }
        //    }
        //}

        //private async Task AddInsiderToDatabase(string personCode, string fullname)
        //{ 
        //    Person person = new Person
        //     {
        //         PersonCode = personCode,
        //         PersonName = fullname,
        //     };

        //    _context.People.Add(person);
        //    await _context.SaveChangesAsync();
        //}

        private async Task<bool> CheckUserCredentialsWithApi(string personCode, string password)
        {
            var user = await _context.People.FirstOrDefaultAsync(u =>u.PersonCode == personCode);

            if(user == null)
            {
                return false;
            }

            string hashedPassword = CalculateSHA256Hash(password+user.Salt);

            if(hashedPassword == user.Password)
            {
                Userparam userParams = new Userparam
                {
                    PersonId = user.PersonId
                };

                try
                {
                    // Call the FloginUser method of the injected InterfaceApi instance
                    Resultuseritem resultUserItem = await _interfaceApi.FloginUser(userParams);

                    // Check the "result" field in the response
                    bool result = resultUserItem.Status;

                    return result;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            return false;
        }

    }
}
