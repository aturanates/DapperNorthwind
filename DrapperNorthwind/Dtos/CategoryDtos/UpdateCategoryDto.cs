using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace DapperNorthwind.Dtos.CategoryDtos
{
    public class UpdateCategoryDto
    {
        [Required(ErrorMessage = "Kategori ID gereklidir.")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir kategori ID giriniz.")]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Kategori adı gereklidir.")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "Kategori adı 2-15 karakter arasında olmalıdır.")]
        public string CategoryName { get; set; }

        [StringLength(500, ErrorMessage = "Açıklama maksimum 500 karakter olabilir.")]
        public string Description { get; set; }
    }
}