using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using DapperNorthwind.Dtos.CategoryDtos;

namespace DapperNorthwind.Repositories.CategoryRepositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<ResultCategoryDto>> GetAllCategoriesAsync();
        Task<ResultCategoryDto> GetCategoryByIdAsync(int id);
        Task<bool> CreateCategoryAsync(CreateCategoryDto category);
        Task<bool> UpdateCategoryAsync(UpdateCategoryDto category);
        Task<bool> DeleteCategoryAsync(int id);
        Task<int> GetCategoryCountAsync();
        Task<bool> CategoryExistsAsync(int id);
        Task<IEnumerable<ResultCategoryDto>> SearchCategoriesAsync(string searchTerm);
    }

    public class CategoryRepository : ICategoryRepository, IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection _connection;

        public CategoryRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        private SqlConnection GetConnection()
        {
            if (_connection == null || _connection.State != System.Data.ConnectionState.Open)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            return _connection;
        }

        public async Task<IEnumerable<ResultCategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                const string query = @"
                    SELECT CategoryID, CategoryName, Description 
                    FROM Categories 
                    ORDER BY CategoryName";


                using (var connection = new SqlConnection(_connectionString))
                {
                    return await connection.QueryAsync<ResultCategoryDto>(query);
                }
            }
            catch (SqlException ex)
            {
               
                throw new ApplicationException($"Kategoriler getirilirken hata oluştu: {ex.Message}", ex);
            }
        }

        public async Task<ResultCategoryDto> GetCategoryByIdAsync(int id)
        {
            try
            {
                const string query = @"
                    SELECT CategoryID, CategoryName, Description 
                    FROM Categories 
                    WHERE CategoryID = @CategoryID";

                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new { CategoryID = id };
                    return await connection.QueryFirstOrDefaultAsync<ResultCategoryDto>(query, parameters);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Kategori getirilirken hata oluştu: {ex.Message}", ex);
            }
        }

        public async Task<bool> CreateCategoryAsync(CreateCategoryDto category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Kategori adı boş olamaz.", nameof(category.CategoryName));

            try
            {
                const string query = @"
                    INSERT INTO Categories (CategoryName, Description) 
                    VALUES (@CategoryName, @Description)";

                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new
                    {
                        CategoryName = category.CategoryName.Trim(),
                        Description = category.Description?.Trim()
                    };

                    var rowsAffected = await connection.ExecuteAsync(query, parameters);
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Kategori eklenirken hata oluştu: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateCategoryAsync(UpdateCategoryDto category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            if (string.IsNullOrWhiteSpace(category.CategoryName))
                throw new ArgumentException("Kategori adı boş olamaz.", nameof(category.CategoryName));

            try
            {
                const string query = @"
                    UPDATE Categories 
                    SET CategoryName = @CategoryName, Description = @Description 
                    WHERE CategoryID = @CategoryID";

                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new
                    {
                        CategoryID = category.CategoryID,
                        CategoryName = category.CategoryName.Trim(),
                        Description = category.Description?.Trim()
                    };

                    var rowsAffected = await connection.ExecuteAsync(query, parameters);
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Kategori güncellenirken hata oluştu: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                const string checkQuery = "SELECT COUNT(*) FROM Products WHERE CategoryID = @CategoryID";
                const string deleteQuery = "DELETE FROM Categories WHERE CategoryID = @CategoryID";

                using (var connection = new SqlConnection(_connectionString))
                {
                    var productCount = await connection.QuerySingleAsync<int>(checkQuery, new { CategoryID = id });

                    if (productCount > 0)
                    {
                        throw new InvalidOperationException($"Bu kategoriye ait {productCount} ürün bulunduğu için silinemez.");
                    }

                    var rowsAffected = await connection.ExecuteAsync(deleteQuery, new { CategoryID = id });
                    return rowsAffected > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Kategori silinirken hata oluştu: {ex.Message}", ex);
            }
        }

        public async Task<int> GetCategoryCountAsync()
        {
            try
            {
                const string query = "SELECT COUNT(*) FROM Categories";
                using (var connection = new SqlConnection(_connectionString))
                {
                    return await connection.QuerySingleAsync<int>(query);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Kategori sayısı getirilirken hata oluştu: {ex.Message}", ex);
            }
        }

        public async Task<bool> CategoryExistsAsync(int id)
        {
            try
            {
                const string query = "SELECT COUNT(*) FROM Categories WHERE CategoryID = @CategoryID";
                using (var connection = new SqlConnection(_connectionString))
                {
                    var count = await connection.QuerySingleAsync<int>(query, new { CategoryID = id });
                    return count > 0;
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Kategori varlığı kontrol edilirken hata oluştu: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ResultCategoryDto>> SearchCategoriesAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetAllCategoriesAsync();

            try
            {
                const string query = @"
                    SELECT CategoryID, CategoryName, Description 
                    FROM Categories 
                    WHERE CategoryName LIKE @SearchTerm 
                       OR Description LIKE @SearchTerm
                    ORDER BY CategoryName";

                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new { SearchTerm = $"%{searchTerm.Trim()}%" };
                    return await connection.QueryAsync<ResultCategoryDto>(query, parameters);
                }
            }
            catch (SqlException ex)
            {
                throw new ApplicationException($"Kategori arama yapılırken hata oluştu: {ex.Message}", ex);
            }
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}