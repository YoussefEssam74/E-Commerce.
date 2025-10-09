using AutoMapper;
using DomainLayer.Contracts;
using DomainLayer.Exceptions;
using DomainLayer.Models;
using ServiceAbstraction;
using Shared.DataTransferObjects.BasketModuleDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BasketService(IBasketRepository _basketRepository,IMapper _mapper) : IBasketService
    {
        public async Task<BasketDTo> CreateOrUpdateBasketAsync(BasketDTo basket)
        {
            var CustomerBasket = _mapper.Map<BasketDTo, CustomerBasket>( basket);
            var CreateOrUpdatedBasket = await _basketRepository.CreateOrUpdateBasketAsync( CustomerBasket);
            if (CreateOrUpdatedBasket is not null)
                return await GetBasketAsync( basket.Id);
            else
                throw new Exception(message: "Can Not Update Or Create Basket Now Try Again Later");
        }


        public async Task<BasketDTo> GetBasketAsync(string Key)
        {
            var Basket = await _basketRepository.GetBasketAsync(Key);
            if (Basket is not null)
                return _mapper.Map<CustomerBasket, BasketDTo>(Basket);
            else
                throw new BasketNotFoundException(Key);
        }
        public Task<bool> DeleteBasketAsync(string Key) => _basketRepository.DeleteBasketAsync(Key);
    }
}
