
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using hello.restaurant.api.Exceptions;

//model
using hello.restaurant.api.Models;
using hello.restaurant.api.Repositories;

namespace hello.restaurant.api.Services
{
     public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> ListAsync();
        
        Task<Restaurant> GetAsync(string id);
        //Task<Restaurant> CreateAsync(Restaurant blog);
        //Task<Restaurant> UpdateAsync(Restaurant blog);
        //Task<Restaurant> DeleteAsync(string id);
    }

    public class RestaurantService : IRestaurantService
    {
        //private readonly IClanService _clanService;

        public RestaurantService(IRestaurantRepository RestaurantRepository) //, IClanService clanService)
        {
            _RestaurantRepository = RestaurantRepository ?? throw new ArgumentNullException(nameof(RestaurantRepository));
            //_clanService = clanService ?? throw new ArgumentNullException(nameof(clanService));
        }

        public async Task<Restaurant> CreateAsync(Restaurant Restaurant)
        {
            //await EnforceClanExistenceAsync(Restaurant.Clan.Name);
            var createdRestaurant = await _RestaurantRepository.CreateAsync(Restaurant);
            return createdRestaurant;
        }


        public async Task<Restaurant> UpdateAsync(Restaurant Restaurant)
        {
           // await EnforceClanExistenceAsync(Restaurant.Clan.Name);
            //await EnforceRestaurantExistenceAsync(Restaurant.Clan.Name, Restaurant.Key);
            var updatedRestaurant = await _RestaurantRepository.UpdateAsync(Restaurant);
            return updatedRestaurant;
        }

        public async Task<Restaurant> DeleteAsync(string id)
        {
           // await EnforceRestaurantExistenceAsync(clanName, RestaurantKey);
            var deletedRestaurant = await _RestaurantRepository.DeleteAsync(id);
            return deletedRestaurant;
        }

        public Task<IEnumerable<Restaurant>> ListAsync()
        {
            return _RestaurantRepository.ListAsync();
        }

        public async Task<Restaurant> GetAsync(string RestaurantKey)
        {
            var Restaurant = await EnforceRestaurantExistenceAsync(RestaurantKey);

            return Restaurant;
        }

        private async Task<Restaurant> EnforceRestaurantExistenceAsync(string RestaurantKey)
        {
            var remoteRestaurant = await _RestaurantRepository.GetAsync(RestaurantKey);

            if (remoteRestaurant == null)
            {
                throw new RestaurantNotFoundException(RestaurantKey);
            }
            return remoteRestaurant;
        } 
    }
}