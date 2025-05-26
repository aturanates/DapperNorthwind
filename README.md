# 🏪 Dapper Northwind Management System

A modern and secure Windows Forms application for Northwind database management.

## 📋 Features

- ✅ **CRUD Operations**: Category add, update, delete and list operations
- ✅ **Async/Await**: Asynchronous programming for all database operations
- ✅ **Repository Pattern**: Clean code architecture and testability
- ✅ **DTO Pattern**: Secure data transfer
- ✅ **SQL Injection Protection**: Security with parameterized queries
- ✅ **Modern UI**: User-friendly interface design
- ✅ **Error Handling**: Comprehensive exception handling
- ✅ **Data Validation**: Input validation and business rules
- ✅ **Real-time Statistics**: Live data display

## 🛠️ Technologies

- **Framework**: .NET Framework 4.7.2
- **ORM**: Dapper (Micro ORM)
- **UI**: Windows Forms
- **Database**: SQL Server (Northwind)
- **Patterns**: Repository, DTO, Async/Await

## 📁 Project Structure

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

## 🚀 Installation

### Requirements
- Visual Studio 2019 or later
- .NET Framework 4.7.2
- SQL Server (LocalDB or Express)
- Northwind database

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/username/DapperNorthwind.git
   cd DapperNorthwind
   ```

2. **Install NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Configure database connection**
   
   Edit the connection string in `App.config`:
   ```xml
   <connectionStrings>
     <add name="DefaultConnection" 
          connectionString="Server=YOUR_SERVER;Database=DapperNorthwind;Integrated Security=True;TrustServerCertificate=True;" 
          providerName="System.Data.SqlClient" />
   </connectionStrings>
   ```

4. **Create Northwind database**
   
   Execute SQL scripts from `NorthwindScripts/NorthwindScript.txt` file.

5. **Build and run the project**
   ```bash
   dotnet build
   dotnet run
   ```

## 💡 Usage

### Category Management

1. **Display Category List**
   - Click "📋 Category List" button
   - All categories will be displayed in DataGridView

2. **Add New Category**
   - Enter category name and description
   - Click "➕ Add Category" button

3. **Update Category**
   - Select a category from DataGridView
   - Edit the information
   - Click "✏️ Update Category" button

4. **Delete Category**
   - Select a category from DataGridView
   - Click "🗑️ Delete Category" button
   - Accept the confirmation message

## 🏗️ Architecture Explanation

### Repository Pattern
```csharp
public interface ICategoryRepository
{
    Task<IEnumerable<ResultCategoryDto>> GetAllCategoriesAsync();
    Task<bool> CreateCategoryAsync(CreateCategoryDto category);
    Task<bool> UpdateCategoryAsync(UpdateCategoryDto category);
    Task<bool> DeleteCategoryAsync(int id);
}
```

### DTO (Data Transfer Object) Pattern
```csharp
public class CreateCategoryDto
{
    public string CategoryName { get; set; }
    public string Description { get; set; }
}
```

### Async/Await Implementation
```csharp
private async void btnCategoryList_Click(object sender, EventArgs e)
{
    var categories = await _categoryRepository.GetAllCategoriesAsync();
    dataGridView1.DataSource = categories.ToList();
}
```

## 🔒 Security Features

- **SQL Injection Protection**: Parameterized queries in all operations
- **Input Validation**: User input validation
- **Error Handling**: Secure error messages
- **Connection Management**: Proper resource disposal
- **Business Rules**: Implementation of business rules

## 📊 Performance Optimizations

- **Dapper ORM**: 30-40% faster than Entity Framework
- **Async Operations**: UI thread is not blocked
- **Connection Pooling**: Database connection management
- **Lazy Loading**: Data loading on demand

## 🧪 Testability

The project is designed for testable code:

```csharp
// Unit test example
[Test]
public async Task CreateCategory_ShouldReturnTrue_WhenValidData()
{
    // Arrange
    var mockRepo = new Mock<ICategoryRepository>();
    var category = new CreateCategoryDto { CategoryName = "Test" };
    
    // Act
    var result = await mockRepo.Object.CreateCategoryAsync(category);
    
    // Assert
    Assert.IsTrue(result);
}
```

## 🔄 Future Improvements

- [ ] **Logging System**: NLog or Serilog integration
- [ ] **Unit Tests**: XUnit test projects
- [ ] **API Integration**: Web API support
- [ ] **Excel Export**: Data export/import features
- [ ] **Reporting**: Crystal Reports integration
- [ ] **Caching**: Redis cache support
- [ ] **Authentication**: User authorization system




## 👨‍💻 Developer

**[Turan Ates]** - *Full Stack Developer*
- GitHub: [@aturanates](https://github.com/aturanates)
- LinkedIn: [linkedin.com/in/aturanates](https://linkedin.com/in/aturanates)
- Email: aturanatess@gmail.com


---

⭐ If you liked this project, don't forget to give it a star!