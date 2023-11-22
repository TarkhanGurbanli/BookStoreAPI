using AutoMapper;
using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Core.Utilities.Result.Concrete.ErrorResult;
using BookStoreAPI.Core.Utilities.Result.Concrete.SuccessResult;
using BookStoreAPI.DataAccess.Settings;
using BookStoreAPI.Entities.Concrete;
using BookStoreAPI.Entities.Dtos.BooksDto;
using BookStoreAPI.Entities.Dtos.CategoriesDto;
using Microsoft.Extensions.Caching.Memory;
using MongoDB.Driver;

namespace BookStoreAPI.Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        //MongoDb ile elaqeni qururuq
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMongoCollection<Book> _bookCollection;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromDays(3);

        public CategoryManager(IMapper mapper, IDatabaseSettings databaseSettings, IMemoryCache memoryCache)
        {
            //MongoDb ile ConnectionString elaqesini qururuq
            var client = new MongoClient(databaseSettings.ConnectionString);
            //MongoDb ile DatabaseName elaqesini qururuq
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _bookCollection = database.GetCollection<Book>(databaseSettings.BookCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
            _memoryCache = memoryCache;
        }
        public async Task<IResult> CategoryCreateAsync(CategoryCreateDto categoryCreateDto)
        {
            try
            {
                if (categoryCreateDto == null)
                    return new ErrorResult("Category data is null");

                var category = _mapper.Map<Category>(categoryCreateDto);

                if (category == null)
                    return new ErrorResult("Mapped category is null");

                if (string.IsNullOrWhiteSpace(category.CategoryName))
                    return new ErrorResult("Category name cannot be empty");

                var existingCategory = await _categoryCollection.Find(x => x.CategoryName == category.CategoryName).FirstOrDefaultAsync();
                if (existingCategory != null)
                    return new ErrorResult("Category with the same name already exists");

                category.CreatedDate = DateTime.Now;
                await _categoryCollection.InsertOneAsync(category);

                var cacheKey = "AllCategories";
                var cachedCategories = _memoryCache.Get<List<CategoryDto>>(cacheKey);

                if (cachedCategories != null)
                {
                    cachedCategories.Add(_mapper.Map<CategoryDto>(category));
                }
                else
                {
                    cachedCategories = new List<CategoryDto> { _mapper.Map<CategoryDto>(category) };
                }

                _memoryCache.Set(cacheKey, cachedCategories, _cacheDuration);

                return new SuccessResult("Category added successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while adding the category: {ex.Message}");
            }
        }



        public async Task<IResult> CategoryRemoveAsync(string id)
        {
            try
            {
                var category = await _categoryCollection.DeleteOneAsync(x => x.Id == id);
                if (category.DeletedCount > 0)
                    return new SuccessResult("Category Delete successfully");

                return new ErrorResult("Category not found");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while deleting the category: {ex.Message}");
            }
        }

        public async Task<IResult> CategoryUpdateAsync(CategoryUpdateDto categoryUpdateDto)
        {
            try
            {
                var updateCategory = _mapper.Map<Category>(categoryUpdateDto);
                updateCategory.CreatedDate = DateTime.Now;

                var result = await _categoryCollection.FindOneAndReplaceAsync(x => x.Id == categoryUpdateDto.Id, updateCategory);
                if (result == null)
                    return new ErrorResult("Category not found");

                return new SuccessResult("Category Update successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while updating the category: {ex.Message}");
            }

        }

        public async Task<IDataResult<List<CategoryDto>>> GetAllCategoryAsync()
        {
            try
            {
                var cacheKey = "AllCategories";

                if (_memoryCache.TryGetValue(cacheKey, out List<CategoryDto> cachedCategories))
                {
                    return new SuccessDataResult<List<CategoryDto>>(cachedCategories, "Categories retrieved from cache");
                }

                var categories = await _categoryCollection.Find(category => true).ToListAsync();

                if (categories == null || categories.Count == 0)
                    return new ErrorDataResult<List<CategoryDto>>("Category list is empty.");

                var mappedCategories = _mapper.Map<List<CategoryDto>>(categories);

                _memoryCache.Set(cacheKey, mappedCategories, _cacheDuration);

                return new SuccessDataResult<List<CategoryDto>>(mappedCategories, "Categories retrieved successfully.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<CategoryDto>>($"An error occurred while getting all categories: {ex.Message}");
            }
        }


        public async Task<IDataResult<CategoryDto>> GetByIdCategoryAsync(string id)
        {
            try
            {
                var cacheKey = "AllCategories";

                if (_memoryCache.TryGetValue(cacheKey, out List<CategoryDto> cachedCategories))
                {
                    var cachedCategory = cachedCategories.FirstOrDefault(c => c.Id == id);
                    if (cachedCategory != null)
                    {
                        return new SuccessDataResult<CategoryDto>(cachedCategory, "Category retrieved from cache");
                    }
                }

                var category = await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
                if (category == null)
                    return new ErrorDataResult<CategoryDto>("Category not found");

                var mappedCategory = _mapper.Map<CategoryDto>(category);

                if (cachedCategories != null)
                {
                    cachedCategories.Add(mappedCategory);
                }
                else
                {
                    cachedCategories = new List<CategoryDto> { mappedCategory };
                }

                _memoryCache.Set(cacheKey, cachedCategories, _cacheDuration);

                return new SuccessDataResult<CategoryDto>(mappedCategory, "Category retrieved successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<CategoryDto>($"An error occurred while getting the category: {ex.Message}");
            }
        }


        public async Task<IDataResult<CategoryWithBooksDto>> GetSingleCategoryByIdWithProductsAsync(string categoryId)
        {
            try
            {
                var category = await _categoryCollection.Find<Category>(x => x.Id == categoryId).FirstOrDefaultAsync();

                if (category == null)
                    return new ErrorDataResult<CategoryWithBooksDto>("Category not found");

                var booksInCategory = await _bookCollection.Find<Book>(x => x.CategoryId == categoryId).ToListAsync();
                var categoryWithBooksDto = new CategoryWithBooksDto
                {
                    Category = _mapper.Map<CategoryDto>(category),
                    Books = _mapper.Map<List<BookDto>>(booksInCategory)
                };

                return new SuccessDataResult<CategoryWithBooksDto>(categoryWithBooksDto, "Category with books retrieved successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<CategoryWithBooksDto>($"An error occurred while getting category with books: {ex.Message}");
            }
        }
    }
}
