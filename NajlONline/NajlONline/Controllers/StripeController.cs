using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stripe.Checkout;
using Stripe;
using System.IO;

namespace NajlONline.Controllers
{
    public class StripeController : Controller
    {
        [HttpPost]
        [Route("api/stripe")]
        public ActionResult CreateCheckoutSession(int ukupnaCena, string naziv)
        {
            string price = null;
            var optionspProduct = new ProductCreateOptions
            {
                Name = naziv,
                DefaultPriceData = new ProductDefaultPriceDataOptions
                {
                    Currency = "rsd",
                    UnitAmount = ukupnaCena
                }
            };
            var serviceProduct = new ProductService();
            price = serviceProduct.Create(optionspProduct).DefaultPriceId;



            var domain = "http://localhost:3000/";
            var options = new SessionCreateOptions()
            {
                LineItems = new List<SessionLineItemOptions>()
                {
                    new SessionLineItemOptions()
                    {
                        Price = price,
                        Quantity = 1
                    }
                },
                PaymentMethodTypes = new List<string>()
            {
                "card"
            },
                Mode = "payment",
                SuccessUrl = domain + "success",
                CancelUrl = domain + "cancel"
            };

            var service = new SessionService();
            Session session = service.Create(options);

        /*    var optionsWebhook = new WebhookEndpointCreateOptions
            {
                Url = "https://example.com/my/webhook/endpoint",
                EnabledEvents = new List<string>
  {
    "charge.failed",
    "charge.succeeded",
  },
            };
            var serviceWebhook = new WebhookEndpointService();
            serviceWebhook.Create(optionsWebhook);

            */

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);

        }

        
    }
}
