
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using hello.restaurant.api.APIs.Services;
using hello.restaurant.api.Exceptions;
using hello.restaurant.api.Extensions;

//model
using hello.restaurant.api.Models;
//using hello.restaurant.api.Repositories;

namespace hello.restaurant.api.Services
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> ListAsync(string keyword);

    }

    public class RestaurantService : IRestaurantService
    {
        private readonly IGoogleService _googleService;
        private readonly IMapper _autoMapper;

        public RestaurantService(IGoogleService googleService, IMapper autoMapper)
        {
            _googleService = googleService ?? throw new ArgumentNullException(nameof(googleService));
            _autoMapper = autoMapper ?? throw new ArgumentNullException(nameof(autoMapper));
        }


        //
        // Summary:
        //      list a restaurant by keyword
        //
        // Returns:
        //      list of restaurant 
        //
        // Params:
        //      keyword: keyword from UI e.g. Bang Sue
        //
        public async Task<IEnumerable<Restaurant>> ListAsync(string keyword)
        {
            var places = await _googleService.ListPlaceByTextSeachAsync(keyword, PlaceType.RESTAURANT.GetDescription());

            //mapping from  apis model to restaurant entity
            var entities = _autoMapper.Map<List<Restaurant>>(places);

            return entities;

        }

    }
}