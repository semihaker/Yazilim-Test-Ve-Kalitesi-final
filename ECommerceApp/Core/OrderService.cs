namespace ECommerceApp.Core
{
    /// <summary>
    /// Sipariş işleme sonucunu temsil eder.
    /// </summary>
    public class OrderResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public decimal FinalAmount { get; set; }
    }

    /// <summary>
    /// Sipariş işleme servisi.
    /// DİKKAT: Bu sınıf test amaçlı 3 adet kasıtlı hata (bug) içermektedir.
    /// </summary>
    public class OrderService
    {
        private const decimal MinimumOrderAmount = 100m;

        /// <summary>
        /// Verilen sepet ve isteğe bağlı indirim koduyla siparişi işler.
        /// </summary>
        /// <param name="cart">Alışveriş sepeti</param>
        /// <param name="discountCode">İndirim kodu (opsiyonel)</param>
        /// <returns>Sipariş sonucu</returns>
        public OrderResult ProcessOrder(Cart cart, string? discountCode = null)
        {
            // ── Sepet Doğrulama ──
            if (cart == null || !cart.Items.Any())
            {
                return new OrderResult
                {
                    IsSuccess = false,
                    Message = "Sepet boş. Sipariş işlenemez."
                };
            }

            // ══════════════════════════════════════════════════════════════
            // BUG 1 (Stok Hatası):
            // DOĞRU OLAN  → item.Product.Stock < item.Quantity
            //                (stok, sipariş miktarından azsa reddet)
            // HATALI OLAN → item.Product.Stock < 0
            //                (sadece negatif stokta reddeder, stok 0 iken
            //                 siparişi onaylar)
            // ══════════════════════════════════════════════════════════════
            foreach (var item in cart.Items)
            {
                if (item.Product.Stock < 0) // ❌ BUG: Doğrusu → item.Product.Stock < item.Quantity
                {
                    return new OrderResult
                    {
                        IsSuccess = false,
                        Message = $"'{item.Product.Name}' ürünü için yeterli stok bulunmamaktadır. " +
                                  $"Mevcut stok: {item.Product.Stock}, İstenen: {item.Quantity}"
                    };
                }
            }

            // ── Toplam Tutar Hesaplama ──
            decimal totalAmount = cart.GetTotalAmount();

            // ── İndirim Kodu Uygulama ──
            if (!string.IsNullOrEmpty(discountCode))
            {
                if (discountCode == "DISCOUNT50")
                {
                    // ══════════════════════════════════════════════════════════════
                    // BUG 3 (İndirim Hatası):
                    // DOĞRU OLAN  → totalAmount = totalAmount * 0.50m
                    //                (%50 indirim uygular)
                    // HATALI OLAN → totalAmount = 50m
                    //                (sepet tutarını sabit 50 TL yapar)
                    // ══════════════════════════════════════════════════════════════
                    totalAmount = 50m; // ❌ BUG: Doğrusu → totalAmount *= 0.50m
                }
                else if (discountCode == "DISCOUNT10")
                {
                    totalAmount *= 0.90m; // ✅ %10 indirim - bu doğru çalışıyor
                }
                else
                {
                    return new OrderResult
                    {
                        IsSuccess = false,
                        Message = $"Geçersiz indirim kodu: '{discountCode}'",
                        FinalAmount = totalAmount
                    };
                }
            }

            // ══════════════════════════════════════════════════════════════
            // BUG 2 (Minimum Sipariş Hatası):
            // DOĞRU OLAN  → totalAmount >= MinimumOrderAmount
            //                (100 TL ve üzeri siparişleri kabul eder)
            // HATALI OLAN → totalAmount > MinimumOrderAmount
            //                (tam 100 TL olduğunda siparişi reddeder)
            // ══════════════════════════════════════════════════════════════
            if (totalAmount > MinimumOrderAmount) // ❌ BUG: Doğrusu → totalAmount >= MinimumOrderAmount
            {
                // Stok düş
                foreach (var item in cart.Items)
                {
                    item.Product.Stock -= item.Quantity;
                }

                return new OrderResult
                {
                    IsSuccess = true,
                    Message = "Sipariş başarıyla işlendi.",
                    FinalAmount = totalAmount
                };
            }
            else
            {
                return new OrderResult
                {
                    IsSuccess = false,
                    Message = $"Sipariş tutarı ({totalAmount:F2} TL), minimum sipariş tutarının ({MinimumOrderAmount:F2} TL) altındadır.",
                    FinalAmount = totalAmount
                };
            }
        }
    }
}
