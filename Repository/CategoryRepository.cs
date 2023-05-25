using BusinessObject;
using DataAccess;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly ICategoryManagement _categoryManagement;

    public CategoryRepository(ICategoryManagement categoryManagement)
    {
        _categoryManagement = categoryManagement;
    }
    public Category? GetCategoryByID(int? categoryId) => CategoryManagement.Instance.GetByID(categoryId);
    public List<Category> GetCategories => CategoryManagement.Instance.GetAll().ToList();
    public void InsertCategory(Category category) => CategoryManagement.Instance.AddNew(category);

    public void DeleteCategory(Category category) => CategoryManagement.Instance.Remove(category);

    public void UpdateCategory(Category category) => CategoryManagement.Instance.Update(category);


}
