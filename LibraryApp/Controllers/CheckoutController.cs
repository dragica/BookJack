using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace LibraryApp.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IBookService _booksService;

        public CheckoutController(IBookService bookService)
        {
            _booksService = bookService;
        }

        public IActionResult Purchase(Guid id)
        {

            var book = _booksService.GetById(id);
            ViewBag.PaymentAmount = book.Price;
            if (book == null) return NotFound();
            return View(book);
        }

        [HttpPost]
        public IActionResult Create(string stripeToken, Guid id)
        {
            var book = _booksService.GetById(id);
            var chargeOptions = new ChargeCreateOptions()
            {
                Amount = (long)(Convert.ToDouble(book.Price) * 100),
                Currency = "usd",
                Source = stripeToken,
                Metadata = new Dictionary<string, string>()
                {
                    {"BookId", book.Id.ToString() },
                    {"BookTitle", book.Title },
                    {"BookAuthor", book.Author }
                }
            };
            var service = new ChargeService();
            Charge charge = service.Create(chargeOptions);
            if (charge.Status == "succeeded")
                return View("Success");
            return View("Failure");
        }

        public IActionResult LoadAllPlans()
        {
            var service = new PlanService();
            var plans = service.List().ToList();
            return View(plans);
        }

        public IActionResult SubscribeToPlan(string id)
        {
            var subscriptionOptions = new SubscriptionCreateOptions
            {
                Customer = "cus_HQJqDQtRh81rgQ",
                Items = new List<SubscriptionItemOptions>
                {
                    new SubscriptionItemOptions
                    {
                        Plan = id
                    }
                }
            };

            var service = new SubscriptionService();
            Subscription subscription = service.Create(subscriptionOptions);

            if (subscription.Created != null)
            {
                return View("Subscribed");
            }
            return View("NotSubscribed");
        }
    }
}