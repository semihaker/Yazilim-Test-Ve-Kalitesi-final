namespace ECommerceApp.Core
{
    /// <summary>
    /// Alışveriş sepetini temsil eder. Ürün ekleme, toplam hesaplama ve temizleme işlevleri sunar.
    /// </summary>
    public class Cart
    {
        private readonly List<CartItem> _items = new();

        /// <summary>
        /// Sepetteki ürün kalemlerinin salt okunur listesi.
        /// </summary>
        public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

        /// <summary>
        /// Sepete belirtilen miktarda ürün ekler.
        /// Aynı ürün zaten sepetteyse miktarını günceller.
        /// </summary>
        /// <param name="product">Eklenecek ürün</param>
        /// <param name="quantity">Eklenecek miktar</param>
        /// <exception cref="ArgumentNullException">Ürün null ise</exception>
        /// <exception cref="ArgumentException">Miktar 0 veya negatif ise</exception>
        /// <exception cref="InvalidOperationException">İstenen miktar stoktan fazla ise</exception>
        public void AddProduct(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Ürün null olamaz.");

            if (quantity <= 0)
                throw new ArgumentException("Miktar sıfırdan büyük olmalıdır.", nameof(quantity));

            if (quantity > product.Stock)
                throw new InvalidOperationException(
                    $"'{product.Name}' için yeterli stok yok. Mevcut: {product.Stock}, İstenen: {quantity}");

            var existingIndex = _items.FindIndex(i => i.Product.Id == product.Id);
            if (existingIndex >= 0)
            {
                _items[existingIndex].Quantity += quantity;
            }
            else
            {
                _items.Add(new CartItem { Product = product, Quantity = quantity });
            }
        }

        /// <summary>
        /// Sepetteki tüm ürünlerin toplam tutarını hesaplar.
        /// </summary>
        /// <returns>Toplam tutar (TL)</returns>
        public decimal GetTotalAmount()
        {
            return _items.Sum(item => item.Product.Price * item.Quantity);
        }

        /// <summary>
        /// Sepeti tamamen temizler, tüm ürünleri kaldırır.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }
    }

    /// <summary>
    /// Sepetteki bir ürün kalemini temsil eder.
    /// </summary>
    public class CartItem
    {
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
    }
}
