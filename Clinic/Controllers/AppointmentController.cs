using Clinic.Database;
using Clinic.Identity;
using Clinic.Interfaces;
using Clinic.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Clinic.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository repository;
        private readonly ShoppingCart _shoppingCart;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private UserManager<ApplicationUser> _userManager;
        public AppointmentController(IAppointmentRepository appointmentRepository, ShoppingCart shoppingCart, UserManager<ApplicationUser> userManager)
        {
            repository = appointmentRepository;
            _shoppingCart = shoppingCart;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Checkout(Appointment appointment)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            if (_shoppingCart.ShoppingCartItems.Count == 0)
            {
                ModelState.AddModelError("", "Ваша корзина пуста, сначала добавьте услуги");
                log.Error($"Ошибка создания заказа: корзина пуста (услуг {_shoppingCart.ShoppingCartItems.Count}");
            }
            else
            {
                foreach (ShoppingCartItem shoppingCart in _shoppingCart.ShoppingCartItems) 
                {
                    repository.CreateAppointment(new Appointment()
                    {
                        DoctorId = shoppingCart.Service.DoctorId,
                       
                    }) ;
                }
                _shoppingCart.ClearCart();
                new Task(delegate { SendMessage("Спасибо за заказ"); }).RunSynchronously();
                return RedirectToAction("CheckoutComplete");
            }
         
            
            return View(appointment);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Спасибо за заказ. Инормацию о заказе можно проверить на почте";

            return View();
        }


        public async Task<IActionResult> SendMessage(string text)
        {  
            string idUser = this.User.Claims.Where(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier).Select(c => c.Value).SingleOrDefault();
            ApplicationUser applicationUser = _userManager.Users.Where(x => x.Id == idUser).FirstOrDefault();
            EmailService emailService = new EmailService();
            await emailService.SendEmailAsync(applicationUser.Email, "Заказ на oTicket", text);
            return RedirectToAction("Index");
            
        }
    }
}