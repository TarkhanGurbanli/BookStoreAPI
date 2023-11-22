using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Core.Utilities.Result.Concrete.ErrorResult;
using BookStoreAPI.Core.Utilities.Result.Concrete.SuccessResult;
using BookStoreAPI.DataAccess.Settings;
using BookStoreAPI.Entities.Dtos.BasketDtos;
using MongoDB.Driver;

public class BasketManager : IBasketService
{
    private readonly IMongoCollection<BasketDto> _basketCollection;

    public BasketManager(IDatabaseSettings databaseSettings)
    {
        var client = new MongoClient(databaseSettings.ConnectionString);
        var database = client.GetDatabase(databaseSettings.DatabaseName);
        _basketCollection = database.GetCollection<BasketDto>(databaseSettings.BasketCollectionName);
    }

    public async Task<IResult> DeleteAsync(string userId, string bookId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(bookId))
                return new ErrorResult("Invalid userId or bookId");

            var existingBasket = await _basketCollection.Find(x => x.UserId == userId).FirstOrDefaultAsync();

            if (existingBasket != null)
            {
                var bookToRemove = existingBasket.basketItems.FirstOrDefault(b => b.BookId == bookId);

                if (bookToRemove != null)
                {
                    existingBasket.basketItems.Remove(bookToRemove);

                    if (existingBasket.basketItems.Count == 0)
                    {
                        await _basketCollection.DeleteOneAsync(x => x.UserId == userId);
                    }
                    else
                    {
                        var updateResult = await _basketCollection.ReplaceOneAsync(x => x.UserId == userId, existingBasket);

                        if (updateResult.ModifiedCount == 0)
                            return new ErrorResult("Basket not found or cannot be updated");
                    }

                    return new SuccessResult("Book deleted from the basket successfully");
                }
                    return new ErrorResult("Book not found in the basket");
            }
                return new ErrorResult("Basket not found");
        }
        catch (Exception ex)
        {
            return new ErrorResult($"An error occurred while deleting the book from the basket: {ex.Message}");
        }
    }



    public async Task<IDataResult<BasketDto>> GetBasket(string userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new ErrorDataResult<BasketDto>("Invalid userId");
            }

            var basket = await _basketCollection.Find(x => x.UserId == userId).FirstOrDefaultAsync();

            if (basket == null)
            {
                return new ErrorDataResult<BasketDto>("Basket not found");
            }

            return new SuccessDataResult<BasketDto>(basket);
        }
        catch (Exception ex)
        {
            return new ErrorDataResult<BasketDto>($"An error occurred while getting the basket: {ex.Message}");
        }
    }

    public async Task<IResult> AddOrUpdateBasket(BasketDto basket)
    {
        try
        {
            if (basket == null)
                return new ErrorResult("Invalid basket object");

            if (string.IsNullOrWhiteSpace(basket.UserId))
                return new ErrorResult("Invalid userId in the basket");

            var existingBasket = await _basketCollection.Find(x => x.UserId == basket.UserId).FirstOrDefaultAsync();

            if (existingBasket == null)
            {
                await _basketCollection.InsertOneAsync(basket);
            }
            else
            {
                foreach (var newBook in basket.basketItems)
                {
                    var existingBook = existingBasket.basketItems.FirstOrDefault(b => b.BookId == newBook.BookId);

                    if (existingBook == null)
                    {
                        // Eğer aynı kitap sepete eklenmemişse, yeni kitap eklenir.
                        existingBasket.basketItems.Add(newBook);
                    }
                    else
                    {
                        // Eğer aynı kitap sepete eklenmişse, miktarı artır.
                        existingBook.Quantity += newBook.Quantity;
                    }
                }

                // Sepeti güncelle.
                var updateResult = await _basketCollection.ReplaceOneAsync(x => x.UserId == basket.UserId, existingBasket);

                if (updateResult.ModifiedCount == 0)
                    return new ErrorResult("Basket not found or cannot be updated");
            }

            return new SuccessResult("Basket saved or updated successfully");
        }
        catch (Exception ex)
        {
            return new ErrorResult($"An error occurred while saving or updating the basket: {ex.Message}");
        }
    }



}
