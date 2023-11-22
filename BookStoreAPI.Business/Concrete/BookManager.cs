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
using MongoDB.Bson;
using MongoDB.Driver;
using Serilog;
using IResult = BookStoreAPI.Core.Utilities.Result.Abstract.IResult;


namespace BookStoreAPI.Business.Concrete
{
    public class BookManager : IBookService
    {
        //MongoDb ile elaqeni qururuq
        private readonly IMongoCollection<Book> _bookCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromDays(3);

        public BookManager(IMapper mapper, IDatabaseSettings databaseSettings, IMemoryCache memoryCache)
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
        public async Task<IResult> BookCreateAsync(BookCreateDto bookCreateDto)
        {
            try
            {
                var newBook = _mapper.Map<Book>(bookCreateDto);
                newBook.CreatedDate = DateTime.Now;
                newBook.IsFeatured = false;

                if (newBook == null)
                {
                    Log.Error("New book created failed. The created book is null.");
                    return new ErrorResult("Book creation failed");
                }

                await _bookCollection.InsertOneAsync(newBook);

                // Yeni kitabı memory elave et
                var cacheKey = "AllBooks";
                var cachedBooks = _memoryCache.Get<List<BookDto>>(cacheKey);

                if (cachedBooks != null)
                {
                    cachedBooks.Add(_mapper.Map<BookDto>(newBook));
                }
                else
                {
                    // Eger memoryde "AllBooks" acarina sahip bir data yoxdursa, yeni bir liste yaradib onu memorye elave et
                    cachedBooks = new List<BookDto> { _mapper.Map<BookDto>(newBook) };
                }

                _memoryCache.Set(cacheKey, cachedBooks, _cacheDuration);

                Log.Information("Book created successfully. BookId: {BookId}, BookName: {BookName}, BookDescription: {BookDescription}, BookPrice: {BookPrice}, BookQuantity: {BookQuantity}", newBook.Id, newBook.Name, newBook.Description, newBook.Price, newBook.Quantity);

                return new SuccessResult("Book created successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while creating the book.");
                return new ErrorResult($"An error occurred while creating the book: {ex.Message}");
            }
        }

        public async Task<IResult> BookDeleteAsync(string id)
        {
            try
            {
                if (!ObjectId.TryParse(id, out _))
                {
                    Log.Error("Invalid ObjectId format. BookId: {BookId}", id);
                    return new ErrorResult("Invalid ObjectId format");
                }

                var bookToDelete = await _bookCollection.Find<Book>(x => x.Id == id).FirstOrDefaultAsync();
                if (bookToDelete == null)
                {
                    Log.Error("Book not found. BookId: {BookId}", id);
                    return new ErrorResult("Book not found");
                }

                var result = await _bookCollection.DeleteOneAsync(x => x.Id == id);
                if (result.DeletedCount == 0)
                {
                    Log.Error("Book not found. BookId: {BookId}", id);
                    return new ErrorResult("Book not found");
                }

                Log.Information("Book deleted successfully. BookId: {BookId}", id);
                return new SuccessResult("Book deleted successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while deleting the book. BookId: {BookId}", id);
                return new ErrorResult($"An error occurred while deleting the book: {ex.Message}");
            }
        }


        public async Task<IResult> BookUpdateAsync(BookUpdateDto bookUpdateDto)
        {
            try
            {
                var updateBook = _mapper.Map<Book>(bookUpdateDto);
                var result = await _bookCollection.FindOneAndReplaceAsync(x => x.Id == bookUpdateDto.Id, updateBook);
                if (result == null)
                    return new ErrorResult("Book not found");

                Log.Information("Book updated successfully. BookId: {BookId}, BookName: {BookName}, BookDescription: {BookDescription}, BookPrice: {BookPrice}, BookQuantity: {BookQuantity}", result.Id, result.Name, result.Description, result.Price, result.Quantity);
                return new SuccessResult("Book updated successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while update the book.");
                return new ErrorResult($"An error occurred while updated the book: {ex.Message}");
            }
        }

        public async Task<IDataResult<List<BookDto>>> GetAllBookAsync()
        {
            try
            {
                var cacheKey = "AllBooks";

                // Memory de varsa ordan getir
                if (_memoryCache.TryGetValue(cacheKey, out List<BookDto> cachedBooks))
                {
                    // Kitapları zaten önbellekteki listeye ekle
                    cachedBooks.AddRange(_mapper.Map<List<BookDto>>(await _bookCollection.Find(book => true).ToListAsync()));

                    // Güncellenmiş listeyi tekrar önbelleğe ekle
                    _memoryCache.Set(cacheKey, cachedBooks, _cacheDuration);

                    Log.Information("Books retrieved successfully from cache.");
                    return new SuccessDataResult<List<BookDto>>(cachedBooks, "Books retrieved from cache");
                }

                // memoryde yoksa MondoDb den getir
                var books = await _bookCollection.Find(book => true).ToListAsync();

                if (books.Any())
                {
                    foreach (var book in books)
                    {
                        book.Category = await _categoryCollection.Find<Category>(x => x.Id == book.CategoryId).FirstAsync();
                    }
                }
                else
                {
                    books = new List<Book>();
                }

                var mapBooks = _mapper.Map<List<BookDto>>(books);

                // Önbelleğe ekle
                _memoryCache.Set(cacheKey, mapBooks, _cacheDuration);

                Log.Information("Books retrieved successfully from MongoDB.");
                return new SuccessDataResult<List<BookDto>>(mapBooks, "Books retrieved successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while getting all books.");
                return new ErrorDataResult<List<BookDto>>($"An error occurred while getting all books: {ex.Message}");
            }
        }


        public async Task<IDataResult<BookDto>> GetByIdBookAsync(string id)
        {
            try
            {
                var cacheKey = "AllBooks";
                // Memory'de varsa ordan getir
                if (_memoryCache.TryGetValue(cacheKey, out List<BookDto> cachedBooks))
                {
                    // Kitabı cachedBooks listesinen tap
                    var cachedBook = cachedBooks.FirstOrDefault(b => b.Id == id);
                    if (cachedBook != null)
                    {
                        Log.Information("Book retrieved successfully from cache. BookId: {BookId}", id);
                        return new SuccessDataResult<BookDto>(cachedBook, "Book retrieved from cache");
                    }
                }

                // Memory'de yoxdursa MongoDB'den getir
                var book = await _bookCollection.Find<Book>(x => x.Id == id).FirstOrDefaultAsync();
                if (book == null)
                {
                    Log.Error("Book not found. BookId: {BookId}", id);
                    return new ErrorDataResult<BookDto>("Book not found");
                }

                book.Category = await _categoryCollection.Find<Category>(x => x.Id == book.CategoryId).FirstOrDefaultAsync();

                var mappedBook = _mapper.Map<BookDto>(book);

                // Memorye elave ele
                if (cachedBooks != null)
                {
                    cachedBooks.Add(mappedBook);
                }
                else
                {
                    // Eger cachedBooks null ise yeni bir liste yarad ve kitabı elave ele
                    cachedBooks = new List<BookDto> { mappedBook };
                }

                _memoryCache.Set(cacheKey, cachedBooks, _cacheDuration);

                Log.Information("Book retrieved successfully from MongoDB. BookId: {BookId}", id);
                return new SuccessDataResult<BookDto>(mappedBook, "Book retrieved successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while getting the book. BookId: {BookId}", id);
                return new ErrorDataResult<BookDto>($"An error occurred while getting the book: {ex.Message}");
            }
        }


        public async Task<List<CategoryWithBooksDto>> GetAllCategoriesWithBooksAsync()
        {
            try
            {
                var categories = await _categoryCollection.Find(_ => true).ToListAsync();

                var result = new List<CategoryWithBooksDto>();

                foreach (var category in categories)
                {
                    var booksInCategory = await _bookCollection.Find<Book>(x => x.CategoryId == category.Id).ToListAsync();
                    var categoryWithBooks = new CategoryWithBooksDto
                    {
                        Category = _mapper.Map<CategoryDto>(category),
                        Books = _mapper.Map<List<BookDto>>(booksInCategory)
                    };

                    result.Add(categoryWithBooks);
                }
                Log.Information("Categories with books retrieved successfully.");
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error occurred while getting categories with books.");
                return null;
            }
        }

        public IResult BookChangeStatus(string bookId)
        {
            try
            {
                var book = _bookCollection.Find<Book>(x => x.Id == bookId).FirstOrDefault();

                if (book == null)
                {
                    Log.Error($"Book with ID {bookId} not found");
                    return new ErrorResult($"Book with ID {bookId} not found");
                }

                book.IsFeatured = !book.IsFeatured;

                var result = _bookCollection.ReplaceOne(x => x.Id == bookId, book);

                if (result.ModifiedCount == 0)
                {
                    Log.Error($"Failed to update book with ID {bookId}");
                    return new ErrorResult($"Failed to update book with ID {bookId}");
                }

                Log.Information($"Changed IsFeature Status of book with ID {bookId} to {book.IsFeatured}");
                return new SuccessResult("Changed Book IsFeature Status!");
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while changing book IsFeature status: {ex.Message}");
                return new ErrorResult($"An error occurred while changing book IsFeature status: {ex.Message}");
            }
        }
    }
}
