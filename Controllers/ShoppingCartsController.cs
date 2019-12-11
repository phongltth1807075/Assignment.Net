using ShoppingCart.Models;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace ShoppingCart.Controllers
{
    public class ShoppingCartsController : Controller
    {
        private Context db = new Context();
        private static string SHOPPING_CART_NAME = "shoppingCart";
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddCart(int productId,int quantity)
        {
            if (quantity<=0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Invalid Quantity");
            }
            var product = db.Products.Find(productId);
            if (product==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Product's' not found");
            }
            var sc = LoadShoppingCart();
            sc.AddCart(product, quantity);
            SaveShoppingCart(sc);
            return Redirect("/ShoppingCarts/ShowCart");
        }
        public ActionResult RemoveCart(int productId)
        {
            var product = db.Products.Find(productId);
            if (product == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Product's' not found");
            }
            // Lấy thông tin shopping cart từ session.
            var sc = LoadShoppingCart();
            // Thêm sản phẩm vào shopping cart.
            sc.RemoveCart(product.Id);
            // lưu thông tin cart vào session.
            SaveShoppingCart(sc);
            return Redirect("/ShoppingCarts/ShowCart");
        }
        public ActionResult ShowCart()
        {
            ViewBag.shoppingCart = LoadShoppingCart();
            return View();
        }
        public ActionResult CreateOrder()
        {
            // load cart trong session.
            var shoppingCart = LoadShoppingCart();
            if (shoppingCart.GetCartItems().Count <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Bad request");
            }
            // chuyển thông tin shopping cart thành Order.
            var order = new Order
            {
                TotalPrice = shoppingCart.GetTotalPrice(),
                OrderDetails = new List<OrderDetail>()
            };
            // Tạo order detail từ cart item.
            foreach (var cartItem in shoppingCart.GetCartItems())
            {
                var orderDetail = new OrderDetail()
                {
                    ProductId = cartItem.Value.ProductId,
                    OrderId = order.Id,
                    Quantity = cartItem.Value.Quantity,
                    UnitPrice = cartItem.Value.Price
                };
                order.OrderDetails.Add(orderDetail);
            }
            db.Orders.Add(order);
            db.SaveChanges();
            ClearCart();
            
            return Redirect("/OrderDetail/Index");
        }
        private ShoppingCarts LoadShoppingCart()
        {
            // lấy thông tin giỏ hàng ra.
            if (!(Session[SHOPPING_CART_NAME] is ShoppingCarts sc))
            {
                sc = new ShoppingCarts();
            }
            return sc;
        }
        private void ClearCart()
        {
            Session.Remove(SHOPPING_CART_NAME);
        }

        /**
         * Tham số nhận vào là một đối tượng shopping cart.
         * Hàm sẽ lưu đối tượng vào session với key được define từ trước.
         */
        private void SaveShoppingCart(ShoppingCarts shoppingCart)
        {
            Session[SHOPPING_CART_NAME] = shoppingCart;
        }
    }
}