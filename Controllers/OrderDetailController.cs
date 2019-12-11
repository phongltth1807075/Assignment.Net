using ShoppingCart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class OrderDetailController : Controller
    {
        private Context db = new Context();
        // GET: OrderDetail
        public ActionResult Index()
        {
            return View(db.Orders.ToList());
        }
        
    }
}