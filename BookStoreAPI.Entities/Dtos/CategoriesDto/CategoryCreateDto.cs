using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Entities.Dtos.CategoriesDto
{
    public class CategoryCreateDto
    {
        public string CategoryName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
