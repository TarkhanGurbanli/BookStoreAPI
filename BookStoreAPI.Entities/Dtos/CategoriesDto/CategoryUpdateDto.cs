using System.ComponentModel.DataAnnotations;

namespace BookStoreAPI.Entities.Dtos.CategoriesDto
{
    public class CategoryUpdateDto
    {
        public string Id { get; set; }
        public string CategoryName { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime UpdatedDate { get; set; }
    }
}
