using Clinic.Interfaces;
using Clinic.Models;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Clinic.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly IAppointmentRepository repository;
        private readonly ShoppingCart _shoppingCart;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AppointmentController(IAppointmentRepository appointmentRepository, ShoppingCart shoppingCart)
        {
            repository = appointmentRepository;
            _shoppingCart = shoppingCart;
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

            if (ModelState.IsValid)
            {
                repository.CreateAppointment(appointment);
                _shoppingCart.ClearCart();

                return RedirectToAction("CheckoutComplete");
            }

            return View(appointment);
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Спасибо за заказ";

            return View();
        }
    }
}