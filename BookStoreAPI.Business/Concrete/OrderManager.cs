using AutoMapper;
using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Core.Utilities.Result.Concrete.ErrorResult;
using BookStoreAPI.Core.Utilities.Result.Concrete.SuccessResult;
using BookStoreAPI.DataAccess.Settings;
using BookStoreAPI.Entities.Concrete;
using BookStoreAPI.Entities.Dtos.BooksDto;
using BookStoreAPI.Entities.Dtos.OrdersDto;
using MongoDB.Driver;

namespace BookStoreAPI.Business.Concrete
{
    public class OrderManager : IOrderService
    {
        private readonly IMapper _mapper;
        private readonly IMongoCollection<Order> _orderCollection;
        private readonly IMongoCollection<Book> _bookCollection;


        public OrderManager(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            //MongoDb ile ConnectionString elaqesini qururuq
            var client = new MongoClient(databaseSettings.ConnectionString);
            //MongoDb ile DatabaseName elaqesini qururuq
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _orderCollection = database.GetCollection<Order>(databaseSettings.OrderCollectionName);
            _bookCollection = database.GetCollection<Book>(databaseSettings.BookCollectionName);
            _mapper = mapper;
        }

        public IResult CancelOrder(string userId, string orderId)
        {
            try
            {
                var filter = Builders<Order>.Filter.Eq(x => x.Id, orderId) & Builders<Order>.Filter.Eq(x => x.UserId, userId);

                var deleteResult = _orderCollection.DeleteOne(filter);

                if (deleteResult.DeletedCount > 0)
                {
                    return new SuccessResult("Order canceled successfully");
                }
                else
                {
                    return new ErrorResult("Order not found or cannot be canceled.");
                }
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while canceling the order: {ex.Message}");
            }
        }


        public IResult CreateOrder(string userId, List<OrderCreateDTO> orderCreateDTOs)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || orderCreateDTOs == null || !orderCreateDTOs.Any())
                    return new ErrorResult("Invalid user ID or order data.");

                var existingOrderIds = _orderCollection.Find(x => x.UserId == userId).Project(x => x.BookId).ToList();
                var newOrderIds = orderCreateDTOs.Select(x => x.BookId).ToList();
                var duplicateOrderIds = existingOrderIds.Intersect(newOrderIds).ToList();

                if (duplicateOrderIds.Any())
                    return new ErrorResult("Cannot create duplicate orders.");

                var orders = _mapper.Map<List<Order>>(orderCreateDTOs);

                orders.ForEach(order =>
                {
                    order.UserId = userId;
                    order.Price = orderCreateDTOs.FirstOrDefault(x => x.BookId == order.BookId)?.Price ?? 0;
                });

                _orderCollection.InsertMany(orders);

                var products = orderCreateDTOs.Select(x => new BookDecrementQuantityDTO
                {
                    BookId = x.BookId,
                    Quantity = x.Quantity,
                }).ToList();

                RemoveProductCount(products);

                return new SuccessResult("Order created successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while creating the order: {ex.Message}");
            }
        }



        public IDataResult<UserOrderDto> GetOrdersByUser(string userId)
        {
            try
            {
                var userOrders = _orderCollection.Find(x => x.UserId == userId).ToList();

                if (userOrders == null || !userOrders.Any())
                    return new ErrorDataResult<UserOrderDto>("User orders not found");

                var userOrderDTO = new UserOrderDto
                {
                    Id = userId,
                    OrderUserDTOs = _mapper.Map<List<OrderUserDto>>(userOrders)
                };

                return new SuccessDataResult<UserOrderDto>(userOrderDTO, "User orders retrieved successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<UserOrderDto>($"An error occurred while getting orders by user: {ex.Message}");
            }
        }

        private void RemoveProductCount(List<BookDecrementQuantityDTO> bookDecrementQuantityDTOs)
        {
            try
            {
                var bookIds = bookDecrementQuantityDTOs.Select(x => x.BookId).ToList();
                var filter = Builders<Book>.Filter.In(x => x.Id, bookIds);
                var books = _bookCollection.Find(filter).ToList();

                foreach (var bookDecrementDTO in bookDecrementQuantityDTOs)
                {
                    var book = books.FirstOrDefault(x => x.Id == bookDecrementDTO.BookId);

                    if (book != null)
                    {
                        book.Quantity -= bookDecrementDTO.Quantity;
                        var update = Builders<Book>.Update.Set(x => x.Quantity, book.Quantity);
                        _bookCollection.UpdateOne(x => x.Id == book.Id, update);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in RemoveProductCount: {ex.Message}");
            }
        }



    }
}
