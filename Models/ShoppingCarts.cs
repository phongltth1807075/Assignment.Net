using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoppingCart.Models
{
    public class ShoppingCarts
    {
        private Dictionary<int, CartItem> _CartItem = new Dictionary<int, CartItem>();
        
        private double _totalPrice = 0;
        public double GetTotalPrice()
        {
            this._totalPrice = 0;
            foreach (var item in _CartItem.Values)
            {
                this._totalPrice += item.Price * item.Quantity;
            }
            return this._totalPrice;
        }
        public Dictionary<int, CartItem> GetCartItems()
        {
            return _CartItem;
        }
        public void SetCartItems(Dictionary<int, CartItem> cartItems)
        {
            this._CartItem = cartItems;
        }
        public void AddCart(Product product, int quantity)
        {
            if (_CartItem.ContainsKey(product.Id))
            {
                var item = _CartItem[product.Id];
                item.Quantity += quantity;
                _CartItem[product.Id] = item;
                return;
            }
            var cartItem = new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                Quantity = quantity
            };
            _CartItem.Add(cartItem.ProductId, cartItem);
        }
        public void UpdateCart(Product product, int quantity)
        {
            if (_CartItem.ContainsKey(product.Id))
            {
                var item = _CartItem[product.Id];
                item.Quantity = quantity;
                _CartItem[product.Id] = item;
            }
        }
        public void RemoveCart(int productId)
        {
            _CartItem.Remove(productId);
        }
        // GET: ShoppingCart
        
    }
}