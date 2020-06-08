using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryApp.Data.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace LibraryApp.Controllers
{
    public class StripeDashboardController : Controller
    {
        public IActionResult Index()
        {
            var balanceService = new BalanceService();
            var balanceResult = balanceService.Get();
            var transactionService = new BalanceTransactionService();
            var transactionResult = transactionService.List();
            var customerService = new CustomerService();
            var customerResult = customerService.List();
            var chargeService = new ChargeService();
            var chargeResult = chargeService.List();
            var disputeService = new DisputeService();
            var disputeResult = disputeService.List();
            var refundService = new RefundService();
            var refundResult = refundService.List();
            var productService = new ProductService();
            var productResult = productService.List();

            var result = new StripeDashboardVM()
            {
                Balance = balanceResult,
                Transactions = transactionResult.ToList(),
                Customers = customerResult.ToList(),
                Charges = chargeResult.ToList(),
                Disputes = disputeResult.ToList(),
                Refunds = refundResult.ToList(),
                Products = productResult.ToList()
            };
            return View(result);
        }
    }
}