# 🏪 Dapper Northwind Yönetim Sistemi

Modern ve güvenli bir Windows Forms uygulaması ile Northwind veritabanı yönetimi.

## 📋 Özellikler

- ✅ **CRUD İşlemleri**: Kategori ekleme, güncelleme, silme ve listeleme
- ✅ **Async/Await**: Tüm veritabanı işlemlerinde asenkron programlama
- ✅ **Repository Pattern**: Temiz kod mimarisi ve test edilebilirlik
- ✅ **DTO Pattern**: Güvenli veri transferi
- ✅ **SQL Injection Koruması**: Parametreli sorgular ile güvenlik
- ✅ **Modern UI**: Kullanıcı dostu arayüz tasarımı
- ✅ **Hata Yönetimi**: Kapsamlı exception handling
- ✅ **Veri Doğrulama**: Input validation ve business rules
- ✅ **Real-time İstatistikler**: Canlı veri gösterimi

## 🛠️ Teknolojiler

- **Framework**: .NET Framework 4.7.2
- **ORM**: Dapper (Micro ORM)
- **UI**: Windows Forms
- **Veritabanı**: SQL Server (Northwind)
- **Pattern**: Repository, DTO, Async/Await

## 📁 Proje Yapısı

```
DapperNorthwind/
├── 📂 Dtos/
│   └── 📂 CategoryDtos/
│       ├── 📄 CreateCategoryDto.cs
│       ├── 📄 GetByIdCategoryDto.cs
│       ├── 📄 ResultCategoryDto.cs
│       └── 📄 UpdateCategoryDto.cs
├── 📂 Repositories/
│   └── 📂 CategoryRepositories/
│       ├── 📄 ICategoryRepository.cs
│       └── 📄 CategoryRepository.cs
├── 📂 NorthwindScripts/
│   └── 📄 NorthwindScript.txt
├── 📄 Form1.cs
├── 📄 Form1.Designer.cs
├── 📄 Program.cs
└── 📄 App.config
```

## 🚀 Kurulum

### Gereksinimler
- Visual Studio 2019 veya üzeri
- .NET Framework 4.7.2
- SQL Server (LocalDB veya Express)
- Northwind veritabanı

### Adımlar

1. **Projeyi klonlayın**
   ```bash
   git clone https://github.com/kullaniciadi/DapperNorthwind.git
   cd DapperNorthwind
   ```

2. **NuGet paketlerini yükleyin**
   ```bash
   dotnet restore
   ```

3. **Veritabanı bağlantısını yapılandırın**
   
   `App.config` dosyasındaki connection string'i düzenleyin:
   ```xml
   <connectionStrings>
     <add name="DefaultConnection