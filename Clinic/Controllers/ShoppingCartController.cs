using Clinic.Interfaces;
using Clinic.Models;
using Clinic.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Clinic.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(IServiceRepository serviceRepository, ShoppingCart shoppingCart)
        {
            _serviceRepository = serviceRepository;
            _shoppingCart = shoppingCart;
        }

        [Authorize]
        public ViewResult Index()
        {
            var items = _shoppingCart.GetShoppingCartItems();
            _shoppingCart.ShoppingCartItems = items;

            var shoppingCartViewModel = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };

            return View(shoppingCartViewModel);
        }

        [Authorize]
        public RedirectToActionResult AddToShoppingCart(int serviceId)
        {
            var selectedService = _serviceRepository.Services.FirstOrDefault(p => p.ServiceId == serviceId);

            if (selectedService != null)
            {
                _shoppingCart.AddToCart(selectedService, 1);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public RedirectToActionResult RemoveFromShoppingCart(int serviceId)
        {
            int test = serviceId;
            var selectedService = _serviceRepository.Services.FirstOrDefault(p => p.ServiceId == serviceId);

            if (selectedService != null)
            {
                _shoppingCart.RemoveFromCart(selectedService);
            }

            return RedirectToAction("Index");
        }
    }
}