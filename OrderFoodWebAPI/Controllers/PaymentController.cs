using DataLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ZarinpalSandbox;

namespace OrderFoodWebAPI.Controllers
{
    public class PaymentController : Controller
    {
        private readonly IOrderService _orderService;

        public PaymentController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> PayOrder(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);

            if (order == null) return NotFound();
            order.TotalPrice = order.OrderItems.Sum(i => i.Count * i.Price);
            Payment payment = new Payment(order.TotalPrice);
            var result = await payment.PaymentRequest("پرداخت سفارش غذا",
                $"https://localhost:44317/Payment/ConfirmPayment/{orderId}");

            if(result.Status == 100)
            {
                return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + result.Authority);
            }

            return Content("مشکلی در پرداخت اینترنتی شما به وجود آمد، لطفا دوباره امتحان کنید");
        }

        public async Task<IActionResult> ConfirmPayment(int orderId)
        {
            var order = await _orderService.GetOrderById(orderId);
            order.TotalPrice = order.OrderItems.Sum(i => i.Count * i.Price);

            if (HttpContext.Request.Query["Status"] != "" &&
                HttpContext.Request.Query["Status"].ToString().ToLower() == "ok" &&
                HttpContext.Request.Query["Authority"] != "")
            {
                string authority = HttpContext.Request.Query["Authority"].ToString();
                Payment payment = new Payment(order.TotalPrice);
                var response = payment.Verification(authority);

                if (response.Result.Status == 100)
                {
                    order.IsPaid = true;
                    await _orderService.UpdateOrder(order);

                    return Ok();
                }
            }

            return Content("مشکلی در پرداخت اینترنتی شما به وجود آمد، لطفا دوباره امتحان کنید");
        }
    }
}
