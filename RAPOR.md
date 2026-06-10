# 🛒 ECommerceApp - Yazılım Test ve Kalite Güvencesi Final Projesi

> **Proje Türü:** Üniversite Final Projesi  
> **Teknolojiler:** C# (.NET 8.0), NUnit 3.14  
> **Amaç:** Kasıtlı hatalar (intentional bugs) içeren bir e-ticaret uygulamasının kapsamlı test sürecini göstermek

---

## 📁 Proje Yapısı

```
ECommerceApp/
├── ECommerceApp.sln
├── README.md
├── ECommerceApp/                          # Ana Uygulama Projesi
│   ├── ECommerceApp.csproj
│   ├── Program.cs                         # Konsol demo uygulaması
│   └── Core/
│       ├── Product.cs                     # Ürün modeli (Id, Name, Price, Stock)
│       ├── Cart.cs                        # Sepet yönetimi (AddProduct, GetTotalAmount, Clear)
│       └── OrderService.cs               # Sipariş servisi (ProcessOrder) — 3 kasıtlı bug içerir
└── ECommerceApp.Tests/                    # Test Projesi (NUnit)
    ├── ECommerceApp.Tests.csproj
    ├── UnitTests/
    │   ├── CartTests.cs                   # 7 birim testi (tümü PASS)
    │   └── OrderServiceTests.cs           # 9 birim testi (6 PASS, 3 FAIL)
    └── IntegrationTests/
        └── CartAndOrderIntegrationTests.cs # 4 entegrasyon testi (1 PASS, 3 FAIL)
```

---

## 🔄 STLC (Software Testing Life Cycle) Süreci

Bu projede STLC'nin 5 temel adımı aşağıdaki şekilde uygulanmıştır:

### 1. 📋 Requirement Analysis (Gereksinim Analizi)
E-ticaret sisteminin temel gereksinimleri belirlenmiştir:
- Ürünlerin `Id`, `Name`, `Price`, `Stock` bilgileri olmalıdır
- Sepete ürün ekleme, toplam hesaplama ve temizleme işlevleri bulunmalıdır
- Sipariş işleme servisi; **stok kontrolü**, **minimum tutar kontrolü (100 TL)** ve **indirim kodu uygulama** (`DISCOUNT50`, `DISCOUNT10`) özelliklerini içermelidir
- Negatif/sıfır miktar, null ürün gibi geçersiz girdiler ele alınmalıdır

### 2. 📝 Test Planning (Test Planlama)
- **Kapsam:** Cart ve OrderService sınıflarının tüm public metotları
- **Test Türleri:** Unit Test (White/Black/Gray Box) ve Integration Test
- **Test Teknikleri:** Equivalence Partitioning (EP) ve Boundary Value Analysis (BVA)
- **Araçlar:** NUnit 3.14, .NET 8.0 Test SDK
- **Hedef:** 20 test case, kasıtlı 3 bug'ı tespit eden 6 başarısız test

### 3. 🎯 Test Case Design (Test Tasarımı)
Her test metodu için:
- Hangi test türünün (Unit/Integration) ve tekniğin (EP/BVA) kullanıldığı belgelendi
- `// Arrange → // Act → // Assert` yapısı tutarlı şekilde uygulandı
- White Box testleri iç yapıyı (Items listesi, hesaplama mantığı) doğruladı
- Black Box testleri yalnızca girdi-çıktı ilişkisine odaklandı
- Gray Box testleri stok durumu gibi iç veri bilgisini kullanarak senaryolar oluşturdu

### 4. ▶️ Test Execution (Test Yürütme)
```bash
dotnet test --verbosity normal
```
Testler NUnit çatısı ile çalıştırılmış, her testin bağımsızlığı `[SetUp]` metodu ile sağlanmıştır.

### 5. 📊 Test Result & Reporting (Test Sonuçları ve Raporlama)
Sonuçlar bu `README.md` dosyasında detaylı olarak raporlanmıştır. (Aşağıdaki bölümlere bakınız.)

---

## 📚 Temel Kavramlar: Error, Fault, Failure ve Defect/Bug

| Kavram | Tanım | Projeden Örnek |
|--------|-------|----------------|
| **Error (Hata)** | Geliştiricinin düşünce veya kodlama sürecindeki insan hatası. Yanlış bir mantık veya yanlış bir operatör kullanımı. | Geliştirici, stok kontrolünde `Stock < item.Quantity` yerine `Stock < 0` yazmıştır. Bu bir **düşünce hatası**dır. |
| **Fault (Kusur)** | Error'un kaynak koduna yansımış hali. Programdaki hatalı kod satırı veya yapısı. | `OrderService.cs` satır: `if (item.Product.Stock < 0)` — Bu satır, Error'un koddaki somut yansımasıdır (Fault). |
| **Failure (Başarısızlık)** | Fault'un çalışma zamanında gözlemlenen yanlış davranışı. Sistemin beklenen çıktıyı üretememesi. | Stok 0 olan bir ürün sipariş edildiğinde, sistem siparişi **reddeteceğine onaylar** — bu gözlemlenen Failure'dır. |
| **Defect / Bug** | Test sürecinde tespit edilen ve raporlanan sorun. Fault + Failure'ın dokümante edilmiş hali. | Bug raporlarında belgelenen 3 kasıtlı bug (stok hatası, sınır değer hatası, indirim hatası). |

### 🔗 Kavramlar Arası İlişki (Bu Projedeki BUG 1 Örneği)
```
Error (İnsan Hatası)        →  Geliştirici stok kontrolünü yanlış düşündü
        ↓
Fault (Kod Kusuru)          →  if (Stock < 0) yazıldı, doğrusu: if (Stock < Quantity)
        ↓
Failure (Başarısızlık)      →  Stock=0 iken sipariş onaylandı (yanlış davranış)
        ↓
Defect/Bug (Raporlanan)     →  Test tarafından tespit edildi ve raporlandı
```

---

## 🎯 Test Stratejileri

### 🔄 Agile Testing
**Tanım:** Yazılım geliştirme sürecinin her iterasyonunda test aktivitelerinin paralel yürütülmesidir. "Test-Last" yerine "Test-First" veya "Test-Along" yaklaşımı benimsenir.

**Bu Projede Uygulanması:**
- Kod ve testler eş zamanlı geliştirilmiştir
- Her özellik (sepet, sipariş, indirim) için test case'ler özellik geliştirilirken tasarlanmıştır
- Hızlı geri bildirim döngüsü ile bug'lar anında tespit edilebilir

### ⚠️ Risk-Based Testing
**Tanım:** Test çabasının, en yüksek iş riski taşıyan alanlara öncelikli olarak yönlendirilmesidir.

**Bu Projede Uygulanması:**
- **Yüksek Risk:** `OrderService.ProcessOrder` → Para işlemi ve stok yönetimi (8 test)
- **Orta Risk:** `Cart` işlemleri → Sepet tutarlılığı (7 test)
- **Kritik Noktalar:** Sınır değerler (100 TL), stok tükenmesi (Stock=0), indirim hesaplaması
- En fazla test, en yüksek riskli bileşen olan OrderService'e ayrılmıştır

### 🔁 Regression Testing
**Tanım:** Mevcut işlevselliğin yeni değişikliklerden etkilenmediğini doğrulamak için daha önce başarılı olan testlerin tekrar çalıştırılmasıdır.

**Bu Projede Uygulanması:**
- Bug düzeltmesi yapıldığında tüm 20 test yeniden çalıştırılarak yan etki kontrolü yapılır
- `Cart` testleri regresyon koruma ağı görevi görür — `OrderService` değişikliklerinin `Cart`'ı etkilemediğini doğrular
- `dotnet test` komutu ile tüm suite tek seferde çalıştırılabilir

---

## 📊 Test Özet Tablosu (Test Summary)

| Metrik | Değer |
|--------|-------|
| **Toplam Test Sayısı** | 20 |
| **Başarılı (PASSED)** | ✅ 14 |
| **Başarısız (FAILED)** | ❌ 6 |
| **Başarı Oranı** | %70 |
| **Tespit Edilen Bug Sayısı** | 3 |

### Test Dağılımı

| Test Dosyası | Tür | Toplam | PASS | FAIL |
|-------------|------|--------|------|------|
| `CartTests.cs` | Unit Test (White/Black/Gray Box) | 7 | 7 | 0 |
| `OrderServiceTests.cs` | Unit Test (Black/Gray Box) | 9 | 6 | 3 |
| `CartAndOrderIntegrationTests.cs` | Integration Test | 4 | 1 | 3 |
| **TOPLAM** | | **20** | **14** | **6** |

### Teknik Dağılımı

| Test Tekniği | Kullanıldığı Test Sayısı |
|-------------|------------------------|
| Equivalence Partitioning (EP) | 11 |
| Boundary Value Analysis (BVA) | 9 |

### Tür Dağılımı

| Test Türü | Test Sayısı |
|-----------|-------------|
| Unit Test - White Box | 3 |
| Unit Test - Black Box | 10 |
| Unit Test - Gray Box | 3 |
| Integration Test | 4 |
| **TOPLAM** | **20** |

---

## ❌ Başarısız Testler ve Bug Analizi

### 🐛 BUG 1: Stok Kontrol Hatası (Severity: Critical)

| Özellik | Detay |
|---------|-------|
| **Konum** | `OrderService.cs` → `ProcessOrder()` metodu |
| **Hatalı Kod** | `if (item.Product.Stock < 0)` |
| **Doğru Kod** | `if (item.Product.Stock < item.Quantity)` |
| **Açıklama** | Stok kontrolü yalnızca negatif stok durumunu kontrol eder. Stok tam olarak 0 olduğunda sipariş onaylanır ve stok -1'e düşer. |
| **Etki** | Stoksuz ürün satışı, negatif stok, müşteri memnuniyetsizliği |
| **Etkileyen Testler** | Test 10, Test 18 |

**Başarısız Olan Testler:**

| # | Test Adı | Dosya | Teknik | Beklenen | Gerçekleşen |
|---|----------|-------|--------|----------|-------------|
| 10 | `ProcessOrder_ZeroStockProduct_ShouldRejectOrder` | OrderServiceTests.cs | Gray Box / BVA | `IsSuccess = false` | `IsSuccess = true` |
| 18 | `FullOrderFlow_ZeroStockAtProcessTime_ShouldRejectOrder` | CartAndOrderIntegrationTests.cs | Integration / BVA | `IsSuccess = false` | `IsSuccess = true` |

---

### 🐛 BUG 2: Minimum Sipariş Tutarı Sınır Değer Hatası (Severity: Medium)

| Özellik | Detay |
|---------|-------|
| **Konum** | `OrderService.cs` → `ProcessOrder()` metodu |
| **Hatalı Kod** | `if (totalAmount > MinimumOrderAmount)` |
| **Doğru Kod** | `if (totalAmount >= MinimumOrderAmount)` |
| **Açıklama** | Minimum sipariş tutarı 100 TL olarak tanımlanmıştır. Ancak `>` operatörü kullanıldığı için tam 100 TL'lik siparişler reddedilir. |
| **Etki** | Sınır değerdeki geçerli siparişlerin kaybı, potansiyel gelir kaybı |
| **Etkileyen Testler** | Test 12, Test 19 |

**Başarısız Olan Testler:**

| # | Test Adı | Dosya | Teknik | Beklenen | Gerçekleşen |
|---|----------|-------|--------|----------|-------------|
| 12 | `ProcessOrder_TotalExactly100_ShouldAcceptOrder` | OrderServiceTests.cs | Black Box / BVA | `IsSuccess = true` | `IsSuccess = false` |
| 19 | `FullOrderFlow_BoundaryAmount100_ShouldAcceptOrder` | CartAndOrderIntegrationTests.cs | Integration / BVA | `IsSuccess = true` | `IsSuccess = false` |

---

### 🐛 BUG 3: İndirim Kodu Hesaplama Hatası (Severity: Critical)

| Özellik | Detay |
|---------|-------|
| **Konum** | `OrderService.cs` → `ProcessOrder()` metodu |
| **Hatalı Kod** | `totalAmount = 50m;` |
| **Doğru Kod** | `totalAmount = totalAmount * 0.50m;` veya `totalAmount *= 0.50m;` |
| **Açıklama** | `DISCOUNT50` indirim kodu uygulandığında sepet tutarına %50 indirim yapmak yerine, tutar sabit 50 TL olarak ayarlanır. |
| **Etki** | Yanlış fiyatlandırma, müşteriye fazla/eksik ücret, finansal kayıp |
| **Etkileyen Testler** | Test 15, Test 20 |

**Başarısız Olan Testler:**

| # | Test Adı | Dosya | Teknik | Beklenen | Gerçekleşen |
|---|----------|-------|--------|----------|-------------|
| 15 | `ProcessOrder_Discount50_ShouldApply50PercentDiscount` | OrderServiceTests.cs | Black Box / EP | `FinalAmount = 150 TL` | `FinalAmount = 50 TL` |
| 20 | `FullOrderFlow_WithDiscount50_ShouldCalculateCorrectly` | CartAndOrderIntegrationTests.cs | Integration / EP | `IsSuccess = true, FinalAmount = 200 TL` | `IsSuccess = false, FinalAmount = 50 TL` |

---

## 📋 Tüm Test Case'lerin Detaylı Listesi

| # | Test Adı | Dosya | Tür | Teknik | Sonuç |
|---|----------|-------|-----|--------|-------|
| 1 | `AddProduct_ValidProduct_ShouldAddToCart` | CartTests.cs | Unit / White Box | EP | ✅ PASS |
| 2 | `AddProduct_NullProduct_ShouldThrowArgumentNullException` | CartTests.cs | Unit / Black Box | EP | ✅ PASS |
| 3 | `AddProduct_ZeroQuantity_ShouldThrowArgumentException` | CartTests.cs | Unit / Black Box | BVA | ✅ PASS |
| 4 | `AddProduct_NegativeQuantity_ShouldThrowArgumentException` | CartTests.cs | Unit / Black Box | BVA | ✅ PASS |
| 5 | `AddProduct_ExceedsStock_ShouldThrowInvalidOperationException` | CartTests.cs | Unit / Gray Box | BVA | ✅ PASS |
| 6 | `GetTotalAmount_MultipleProducts_ShouldReturnCorrectTotal` | CartTests.cs | Unit / White Box | EP | ✅ PASS |
| 7 | `Clear_NonEmptyCart_ShouldRemoveAllItems` | CartTests.cs | Unit / White Box | EP | ✅ PASS |
| 8 | `ProcessOrder_NullCart_ShouldReturnFailure` | OrderServiceTests.cs | Unit / Black Box | EP | ✅ PASS |
| 9 | `ProcessOrder_EmptyCart_ShouldReturnFailure` | OrderServiceTests.cs | Unit / Black Box | EP | ✅ PASS |
| 10 | `ProcessOrder_ZeroStockProduct_ShouldRejectOrder` | OrderServiceTests.cs | Unit / Gray Box | BVA | ❌ FAIL |
| 11 | `ProcessOrder_NegativeStock_ShouldRejectOrder` | OrderServiceTests.cs | Unit / Gray Box | BVA | ✅ PASS |
| 12 | `ProcessOrder_TotalExactly100_ShouldAcceptOrder` | OrderServiceTests.cs | Unit / Black Box | BVA | ❌ FAIL |
| 13 | `ProcessOrder_TotalAbove100_ShouldAcceptOrder` | OrderServiceTests.cs | Unit / Black Box | BVA | ✅ PASS |
| 14 | `ProcessOrder_TotalBelow100_ShouldRejectOrder` | OrderServiceTests.cs | Unit / Black Box | EP | ✅ PASS |
| 15 | `ProcessOrder_Discount50_ShouldApply50PercentDiscount` | OrderServiceTests.cs | Unit / Black Box | EP | ❌ FAIL |
| 16 | `ProcessOrder_InvalidDiscountCode_ShouldReturnFailure` | OrderServiceTests.cs | Unit / Black Box | EP | ✅ PASS |
| 17 | `FullOrderFlow_AddProductsAndProcess_ShouldSucceed` | CartAndOrderIntegrationTests.cs | Integration | E2E | ✅ PASS |
| 18 | `FullOrderFlow_ZeroStockAtProcessTime_ShouldRejectOrder` | CartAndOrderIntegrationTests.cs | Integration | BVA | ❌ FAIL |
| 19 | `FullOrderFlow_BoundaryAmount100_ShouldAcceptOrder` | CartAndOrderIntegrationTests.cs | Integration | BVA | ❌ FAIL |
| 20 | `FullOrderFlow_WithDiscount50_ShouldCalculateCorrectly` | CartAndOrderIntegrationTests.cs | Integration | EP | ❌ FAIL |

> **Not:** Entegrasyon testleri, birim testlerinde bulunan aynı 3 bug'ı uçtan uca senaryolarda tekrar tespit ederek, bug'ların sistem genelindeki etkisini ortaya koyar.

---

## 🛠️ Projeyi Çalıştırma

### Gereksinimler
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

### Derleme ve Çalıştırma
```bash
# Projeyi derle
dotnet build

# Konsol demo uygulamasını çalıştır
dotnet run --project ECommerceApp

# Tüm testleri çalıştır
dotnet test --verbosity normal

# Sadece birim testlerini çalıştır
dotnet test --filter "Category=UnitTest"

# Sadece entegrasyon testlerini çalıştır
dotnet test --filter "Category=IntegrationTest"
```

### Beklenen Test Çıktısı
```
Toplam test sayısı: 20
     Geçen:  14
     Başarısız:  6
```

---

## 👤 Geliştirici Bilgileri

| Bilgi | Değer |
|-------|-------|
| **Proje** | Yazılım Test ve Kalite Güvencesi - Final Projesi |
| **Konu** | E-Ticaret Uygulaması Test Otomasyonu |
| **Framework** | NUnit 3.14 / .NET 8.0 |

---

> **📌 Not:** Bu proje eğitim amaçlı olarak kasıtlı hatalar içermektedir. `OrderService.cs` dosyasındaki bug'lar, gerçek dünya senaryolarında test sürecinin önemini göstermek amacıyla bilinçli olarak bırakılmıştır.
