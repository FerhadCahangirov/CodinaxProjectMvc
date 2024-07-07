using CodinaxProjectMvc.Business.Abstract.PersistenceServices;
using CodinaxProjectMvc.DataAccess.Abstract.Repositories;
using CodinaxProjectMvc.DataAccess.Models;
using CodinaxProjectMvc.ViewModel;
using CodinaxProjectMvc.ViewModel.CategoryVm;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CodinaxProjectMvc.Business.PersistenceServices
{
    public class CategoryService : ICategoryService
    {
        private readonly IReadRepository<Category> _categoryReadRepository;
        private readonly IWriteRepository<Category> _categoryWriteRepository;
        private readonly IActionContextAccessor _actionContextAccessor;

        public CategoryService(IReadRepository<Category> categoryReadRepository, IWriteRepository<Category> categoryWriteRepository, IActionContextAccessor actionContextAccessor)
        {
            _categoryReadRepository = categoryReadRepository;
            _categoryWriteRepository = categoryWriteRepository;
            _actionContextAccessor = actionContextAccessor;
        }

        public async Task<bool> CreateAsync(CategoryCreateVm categoryCreateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            Category category = new Category()
            {
                Content= categoryCreateVm.CategoryName,
                ContentRu= categoryCreateVm.CategoryNameRu,
                ContentTr= categoryCreateVm.CategoryNameTr,
            };

            await _categoryWriteRepository.AddAsync(category);
            await _categoryWriteRepository.SaveAsync();

            return true;
        }

        public async Task<PaginationVm<IEnumerable<Category>>> GetCategoriesPaginationAsync(string? searchFilter = null, string? statusFilter = null)
        {
            var query = _categoryReadRepository.GetWhere(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(searchFilter))
            {
                searchFilter = searchFilter.ToLower();
                query = query.Where(x => x.Content.ToLower().Contains(searchFilter));
            }

            var categories = await query.ToListAsync();

            int total = await query.CountAsync();

            PaginationVm<IEnumerable<Category>> pagination = new PaginationVm<IEnumerable<Category>>(total, categories);

            return pagination;
        }

        public async Task<CategoryUpdateVm> GetCategoryUpdateDataAsync(Guid id)
        {
            Category? category = await _categoryReadRepository.GetSingleAsync(x => x.Id == id);
            if (category == null) return new CategoryUpdateVm();

            return new CategoryUpdateVm()
            {
                CategoryName = category.Content,
                CategoryNameRu = category.ContentRu,
                CategoryNameTr = category.ContentTr,
                Id = id
            };
        }

        public async Task<bool> UpdateAsync(CategoryUpdateVm categoryUpdateVm)
        {
            if (!_actionContextAccessor.ActionContext.ModelState.IsValid)
            {
                return false;
            }

            Category? category = await _categoryReadRepository.GetSingleAsync(x => x.Id == categoryUpdateVm.Id);
            if (category == null)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(nameof(CategoryUpdateVm.Id), "Category Item Not Found");
                return false;
            }

            category.Content = categoryUpdateVm.CategoryName;
            category.ContentRu = categoryUpdateVm.CategoryNameRu;
            category.ContentTr = categoryUpdateVm.CategoryNameTr;

            _categoryWriteRepository.Update(category);
            await _categoryWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> ArchiveAsync(Guid id)
        {
            Category? category = await _categoryReadRepository.GetSingleAsync(x => x.Id == id);
            if (category == null) return false;

            category.IsArchived = true;

            _categoryWriteRepository.Update(category);
            await _categoryWriteRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UnArchiveAsync(Guid id)
        {
            Category? category = await _categoryReadRepository.GetSingleAsync(x => x.Id == id);
            if (category == null)
            {
                return false;
            }

            category.IsArchived = false;

            _categoryWriteRepository.Update(category);
            await _categoryWriteRepository.SaveAsync();

            return false;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            Category? category = await _categoryReadRepository.Table
                .Include(x => x.Courses).SingleOrDefaultAsync(x => x.Id == id);

            if (category == null) return false;

            if(category.Courses.Any(x => !x.IsDeleted))
            {
                return false;
            }

            category.IsDeleted = true;

            _categoryWriteRepository.Update(category);
            await _categoryWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> RestoreAsync(Guid id)
        {
            Category? category = await _categoryReadRepository.GetSingleAsync(x => x.Id == id);
            if (category == null)
            {
                return false;
            };

            bool is_category_exists = await CheckIsExistsAsync(category.Content); 

            if(is_category_exists)
            {
                return false;
            }

            category.IsDeleted = false;

            _categoryWriteRepository.Update(category);
            await _categoryWriteRepository.SaveAsync();

            return true;
        }

        public async Task<bool> CheckIsExistsAsync(string name)
        {
            Category? category = await _categoryReadRepository.GetSingleAsync(x => x.Content == name && !x.IsDeleted);
            return category != null;
        }
    }
}
    