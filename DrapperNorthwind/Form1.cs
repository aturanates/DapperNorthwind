using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dapper;
using DapperNorthwind.Dtos.CategoryDtos;
using DapperNorthwind.Repositories.CategoryRepositories;
using DrapperNorthwind.Repositories.CategoryRepositories;

namespace DrapperNorthwind
{
    public partial class Form1 : Form
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly string _connectionString;

        public Form1()
        {
            InitializeComponent();

            _connectionString = "Server=TURANSMART\\SQLEXPRESS;initial Catalog=DapperNorthwind;Integrated Security=True;TrustServerCertificate=True";

            _categoryRepository = new CategoryRepository(_connectionString);

            SetupForm();
        }

        private void SetupForm()
        {
            this.Text = "Dapper Northwind Yönetim Sistemi";
            this.BackColor = Color.FromArgb(240, 240, 240);

            ConfigureDataGridView();

            SetupButtonStyles();
        }

        private void ConfigureDataGridView()
        {
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(51, 122, 183);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 122, 183);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            dataGridView1.CellClick += dataGridView1_CellClick;
        }

        private void SetupButtonStyles()
        {
            var buttons = new[] { btnCategoryList, btnCategoryAdd, btnCategoryDelete, btnCategoryUpdate };

            foreach (var button in buttons)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.BackColor = Color.FromArgb(51, 122, 183);
                button.ForeColor = Color.White;
                button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                button.Cursor = Cursors.Hand;

                button.MouseEnter += (s, e) => button.BackColor = Color.FromArgb(40, 96, 144);
                button.MouseLeave += (s, e) => button.BackColor = Color.FromArgb(51, 122, 183);
            }

            btnCategoryDelete.BackColor = Color.FromArgb(217, 83, 79);
            btnCategoryDelete.MouseEnter += (s, e) => btnCategoryDelete.BackColor = Color.FromArgb(172, 61, 57);
            btnCategoryDelete.MouseLeave += (s, e) => btnCategoryDelete.BackColor = Color.FromArgb(217, 83, 79);

            btnCategoryList.Text = "📋 Kategori Listesi";
            btnCategoryAdd.Text = "➕ Kategori Ekle";
            btnCategoryDelete.Text = "🗑️ Kategori Sil";
            btnCategoryUpdate.Text = "✏️ Kategori Güncelle";
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadStatisticsAsync();
            await LoadCategoriesAsync();
            ClearForm();
        }

        private async Task LoadStatisticsAsync()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {

                    var categoryCount = await connection.ExecuteScalarAsync<int>(
                        "SELECT COUNT(*) FROM Categories");
                    lblCategoryCount.Text = $"📁 Toplam Kategori: {categoryCount:N0}";


                    var productCount = await connection.ExecuteScalarAsync<int>(
                        "SELECT COUNT(*) FROM Products");
                    lblProductCount.Text = $"📦 Toplam Ürün: {productCount:N0}";


                    var averageStock = await connection.ExecuteScalarAsync<decimal>(
                        "SELECT AVG(CAST(UnitsInStock AS DECIMAL(10,2))) FROM Products WHERE UnitsInStock > 0");
                    lblAverageStock.Text = $"📊 Ortalama Stok: {averageStock:F1}";


                    var seafoodValue = await connection.ExecuteScalarAsync<decimal?>(@"
                        SELECT SUM(UnitPrice * UnitsInStock) 
                        FROM Products p 
                        INNER JOIN Categories c ON p.CategoryID = c.CategoryID 
                        WHERE c.CategoryName = 'Seafood'") ?? 0;
                    lblSeaFoodTotalPrice.Text = $"🐟 Deniz Ürün. Değeri: {seafoodValue:C}";
                }
            }
            catch (Exception ex)
            {
                ShowError($"İstatistikler yüklenirken hata: {ex.Message}");
            }
        }

        private async Task LoadCategoriesAsync()
        {
            try
            {
                SetLoading(true);
                var categories = await _categoryRepository.GetAllCategoriesAsync();
                dataGridView1.DataSource = categories.ToList();

                if (dataGridView1.Columns.Count > 0)
                {
                    dataGridView1.Columns["CategoryID"].HeaderText = "ID";
                    dataGridView1.Columns["CategoryName"].HeaderText = "Kategori Adı";
                    dataGridView1.Columns["Description"].HeaderText = "Açıklama";
                    dataGridView1.Columns["CategoryID"].Width = 50;
                }
            }
            catch (Exception ex)
            {
                ShowError($"Kategoriler yüklenirken hata: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }

        private async void btnCategoryList_Click(object sender, EventArgs e)
        {
            await LoadCategoriesAsync();
        }

        private async void btnAddCategory_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                SetLoading(true);

                var newCategory = new CreateCategoryDto
                {
                    CategoryName = txtCategoryName.Text.Trim(),
                    Description = txtCategoryDescription.Text.Trim()
                };

                var success = await _categoryRepository.CreateCategoryAsync(newCategory);

                if (success)
                {
                    ShowSuccess("Kategori başarıyla eklendi!");
                    await LoadCategoriesAsync();
                    await LoadStatisticsAsync();
                    ClearForm();
                }
                else
                {
                    ShowError("Kategori eklenirken bir hata oluştu.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Kategori eklenirken hata: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }

        private async void btnCategoryUpdate_Click(object sender, EventArgs e)
        {
            if (!ValidateInputsForUpdate()) return;

            try
            {
                SetLoading(true);

                var updateCategory = new UpdateCategoryDto
                {
                    CategoryID = int.Parse(txtCategoryID.Text),
                    CategoryName = txtCategoryName.Text.Trim(),
                    Description = txtCategoryDescription.Text.Trim()
                };

                var success = await _categoryRepository.UpdateCategoryAsync(updateCategory);

                if (success)
                {
                    ShowSuccess("Kategori başarıyla güncellendi!");
                    await LoadCategoriesAsync();
                    ClearForm();
                }
                else
                {
                    ShowError("Kategori bulunamadı veya güncelleme başarısız.");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Kategori güncellenirken hata: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }

        private async void btnCategoryDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCategoryID.Text))
            {
                ShowError("Silinecek kategori ID'sini giriniz.");
                return;
            }

            if (!int.TryParse(txtCategoryID.Text, out int categoryId))
            {
                ShowError("Geçerli bir kategori ID'si giriniz.");
                return;
            }

            var confirmResult = MessageBox.Show(
                "Bu kategoriyi silmek istediğinizden emin misiniz?",
                "Kategori Silme Onayı",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning,
                MessageBoxDefaultButton.Button2);

            if (confirmResult != DialogResult.Yes) return;

            try
            {
                SetLoading(true);

                var success = await _categoryRepository.DeleteCategoryAsync(categoryId);

                if (success)
                {
                    ShowSuccess("Kategori başarıyla silindi!");
                    await LoadCategoriesAsync();
                    await LoadStatisticsAsync();
                    ClearForm();
                }
                else
                {
                    ShowError("Kategori bulunamadı.");
                }
            }
            catch (InvalidOperationException ex)
            {
                ShowError(ex.Message);
            }
            catch (Exception ex)
            {
                ShowError($"Kategori silinirken hata: {ex.Message}");
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Rows[e.RowIndex].DataBoundItem != null)
            {
                var selectedCategory = (ResultCategoryDto)dataGridView1.Rows[e.RowIndex].DataBoundItem;

                txtCategoryID.Text = selectedCategory.CategoryID.ToString();
                txtCategoryName.Text = selectedCategory.CategoryName;
                txtCategoryDescription.Text = selectedCategory.Description;
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtCategoryName.Text))
            {
                ShowError("Kategori adı giriniz.");
                txtCategoryName.Focus();
                return false;
            }

            if (txtCategoryName.Text.Trim().Length < 2)
            {
                ShowError("Kategori adı en az 2 karakter olmalıdır.");
                txtCategoryName.Focus();
                return false;
            }

            return true;
        }

        private bool ValidateInputsForUpdate()
        {
            if (string.IsNullOrWhiteSpace(txtCategoryID.Text))
            {
                ShowError("Güncellenecek kategori ID'sini giriniz.");
                txtCategoryID.Focus();
                return false;
            }

            if (!int.TryParse(txtCategoryID.Text, out _))
            {
                ShowError("Geçerli bir kategori ID'si giriniz.");
                txtCategoryID.Focus();
                return false;
            }

            return ValidateInputs();
        }

        private void ClearForm()
        {
            txtCategoryID.Clear();
            txtCategoryName.Clear();
            txtCategoryDescription.Clear();
            txtCategoryName.Focus();
        }

        private void SetLoading(bool isLoading)
        {
            var buttons = new[] { btnCategoryList, btnCategoryAdd, btnCategoryDelete, btnCategoryUpdate };

            foreach (var button in buttons)
            {
                button.Enabled = !isLoading;
            }

            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowSuccess(string message)
        {
            MessageBox.Show(message, "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _categoryRepository?.Dispose();
            base.OnFormClosed(e);
        }
    }
}