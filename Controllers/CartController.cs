using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eStoreASP.Data;
using eStoreASP.Models;
using System.Text.Json;

namespace eStoreASP.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        public IActionResult Add(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null) return NotFound();

            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(c => c.ProductId == id);

            int currentQuantityInCart = existingItem?.Quantity ?? 0;

            if (currentQuantityInCart >= product.Stock)
            {
                TempData["Error"] = "Not enough stock available.";
                return RedirectToAction("Index", "Store");
            }

            if (existingItem != null)
                existingItem.Quantity++;
            else
                cart.Add(new CartItem
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1
                });

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == id);

            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                    cart.Remove(item);
            }

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(c => c.ProductId == id);

            if (item != null)
                cart.Remove(item);

            SaveCart(cart);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var cart = GetCart();

            if (!cart.Any())
                return RedirectToAction("Index");

            var userId = User?.Identity?.Name;

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(c => c.Price * c.Quantity),
                Status = OrderStatus.Pending,
                OrderItems = cart.Select(c => new OrderItem
                {
                    ProductId = c.ProductId,
                    ProductName = c.Name,
                    Price = c.Price,
                    Quantity = c.Quantity
                }).ToList()
            };

            foreach (var cartItem in cart)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == cartItem.ProductId);

                if (product == null || product.Stock < cartItem.Quantity)
                {
                    TempData["Error"] = "One or more products are no longer available.";
                    return RedirectToAction("Index");
                }
            }

            //Reduce stock
            foreach (var cartItem in cart)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.Id == cartItem.ProductId);

                if (product != null)
                {
                    product.Stock -= cartItem.Quantity;
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Confirmation", new { id = order.Id });
        }

        public IActionResult Confirmation(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        private List<CartItem> GetCart()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");
            if (sessionCart == null)
                return new List<CartItem>();

            return JsonSerializer.Deserialize<List<CartItem>>(sessionCart);
        }

        private void SaveCart(List<CartItem> cart)
        {
            HttpContext.Session.SetString("Cart",
                JsonSerializer.Serialize(cart));
        }

        public IActionResult MyOrders()
        {
            var userId = User?.Identity?.Name;

            if (userId == null)
                return RedirectToAction("Login", "Account");

            var orders = _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }
    }
}
