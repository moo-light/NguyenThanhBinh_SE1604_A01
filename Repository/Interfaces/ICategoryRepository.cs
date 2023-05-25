using BusinessObject;

namespace Repository.Interfaces
{
    public interface ICategoryRepository
    {
        List<Category> GetCategories { get; }

        void DeleteCategory(Category category);
        Category? GetCategoryByID(int? categoryId);
        void InsertCategory(Category category);
        void UpdateCategory(Category category);
    }
}