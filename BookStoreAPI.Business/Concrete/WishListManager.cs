using AutoMapper;
using BookStoreAPI.Business.Abstract;
using BookStoreAPI.Core.Utilities.Result.Abstract;
using BookStoreAPI.Core.Utilities.Result.Concrete.ErrorResult;
using BookStoreAPI.Core.Utilities.Result.Concrete.SuccessResult;
using BookStoreAPI.DataAccess.Settings;
using BookStoreAPI.Entities.Concrete;
using BookStoreAPI.Entities.Dtos.WishListsDto;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace BookStoreAPI.Business.Concrete
{
    public class WishListManager : IWishListService
    {
        private readonly IMongoCollection<WishList> _wishlistCollection;
        private readonly IMapper _mapper;

        public WishListManager(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _wishlistCollection = database.GetCollection<WishList>(databaseSettings.WishListCollectionName);
            _mapper = mapper;
        }

        public IResult AddWishList(string userId, WishListAddItemDto wishListAddItemDTO)
        {
            try
            {
                if (string.IsNullOrEmpty(userId) || wishListAddItemDTO == null)
                    return new ErrorResult("User ID or WishList cannot be null or empty.");

                var existingWishList = _wishlistCollection.Find(x => x.UserId == userId && x.BookId == wishListAddItemDTO.BookId).FirstOrDefault();
                if (existingWishList != null)
                    return new ErrorResult("WishList item already exists for the user and book.");

                var map = _mapper.Map<WishList>(wishListAddItemDTO);
                map.UserId = userId;
                map.BookId = wishListAddItemDTO.BookId;
                

                _wishlistCollection.InsertOne(map);

                return new SuccessResult("WishList Added!");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while adding the wishlist: {ex.Message}");
            }
        }

        public IDataResult<List<WishListItemDto>> GetUserWishList(string userId)
        {
            try
            {
                var userWishList = _wishlistCollection.Find(x => x.UserId == userId).ToList();

                var wishListItems = _mapper.Map<List<WishListItemDto>>(userWishList);
                return new SuccessDataResult<List<WishListItemDto>>(wishListItems, "User's WishList retrieved successfully");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<WishListItemDto>>($"An error occurred while getting the user's wishlist: {ex.Message}");
            }
        }

        public IResult RemoveWishList(string userId, string bookId)
        {
            try
            {
                var filter = Builders<WishList>.Filter.Eq(x => x.UserId, userId) & Builders<WishList>.Filter.Eq(x => x.BookId, bookId);
                var deleteResult = _wishlistCollection.DeleteOne(filter);

                if (deleteResult.DeletedCount > 0)
                {
                    return new SuccessResult("WishList item removed successfully.");
                }
                else
                {
                    return new ErrorResult("WishList item not found for the user and book.");
                }
            }
            catch (Exception ex)
            {
                return new ErrorResult($"An error occurred while removing from the wishlist: {ex.Message}");
            }
        }



    }
}
