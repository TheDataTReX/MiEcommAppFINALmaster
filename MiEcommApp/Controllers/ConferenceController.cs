using Microsoft.AspNetCore.Mvc;
using MiEcommApp.Models;

namespace MiEcommApp.Controllers
{
    public class ConferenceController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(ConferenceRegistration registration)
        {
            if (ModelState.IsValid)
            {
                // If the form is valid, you can proceed with your logic
                return RedirectToAction("Success");
            }

            // If the form is not valid, return the form view again with the validation messages
            return View(registration);
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}

