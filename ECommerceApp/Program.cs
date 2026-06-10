using ECommerceApp.Core;

// ========================================
//   🛒 ECommerceApp - E-Ticaret Sistemi
// ========================================

Console.WriteLine("========================================");
Console.WriteLine("  🛒 ECommerceApp - E-Ticaret Demo");
Console.WriteLine("========================================");
Console.WriteLine();

// Ürünleri oluştur
var laptop = new Product(1, "Laptop", 5000m, 10);
var mouse = new Product(2, "Mouse", 150m, 50);
var keyboard = new Product(3, "Klavye", 350m, 30);

Console.WriteLine("📦 Mevcut Ürünler:");
Console.WriteLine($"  - {laptop.Name,-12} | Fiyat: {laptop.Price,10:F2} TL | Stok: {laptop.Stock}");
Console.WriteLine($"  - {mouse.Name,-12} | Fiyat: {mouse.Price,10:F2} TL | Stok: {mouse.Stock}");
Console.WriteLine($"  - {keyboard.Name,-12} | Fiyat: {keyboard.Price,10:F2} TL | Stok: {keyboard.Stock}");
Console.WriteLine();

// Sepete ürün ekle
var cart = new Cart();
cart.AddProduct(laptop, 1);
cart.AddProduct(mouse, 2);

Console.WriteLine("🛒 Sepet İçeriği:");
foreach (var item in cart.Items)
{
    var subtotal = item.Product.Price * item.Quantity;
    Console.WriteLine($"  - {item.Product.Name,-12} x{item.Quantity} = {subtotal,10:F2} TL");
}
Console.WriteLine($"  ─────────────────────────────────");
Console.WriteLine($"  💰 Toplam:         {cart.GetTotalAmount(),14:F2} TL");
Console.WriteLine();

// Sipariş işle
var orderService = new OrderService();
var result = orderService.ProcessOrder(cart);

Console.WriteLine("📋 Sipariş Sonucu:");
Console.WriteLine($"  Durum : {(result.IsSuccess ? "✅ Başarılı" : "❌ Başarısız")}");
Console.WriteLine($"  Mesaj : {result.Message}");
Console.WriteLine($"  Tutar : {result.FinalAmount:F2} TL");
Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("  Testleri çalıştırmak için:");
Console.WriteLine("  > dotnet test");
Console.WriteLine("========================================");
