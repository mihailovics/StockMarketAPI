using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Stock;
using api.Model;

namespace api.Mappers
{
    public static class StockMappers
    {
        public static StockDTO ToStockDTO(this Stock stockModel){
            return new StockDTO{
                Id = stockModel.Id,
                Symbol = stockModel.Symbol,
                Company = stockModel.Company,
                Purchase = stockModel.Purchase,
                Dividend = stockModel.Dividend,
                Industry = stockModel.Industry,
                MarketCap = stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c => c.ToCommentDTO()).ToList()
            };
        }
        public static Stock ToStockFromCreateDTO(this CreateStockRequestDTO stockDTO)
        {
            return new Stock
            {
                Symbol = stockDTO.Symbol,
                Company = stockDTO.Company,
                Purchase = stockDTO.Purchase,
                Dividend = stockDTO.Dividend,
                Industry = stockDTO.Industry,
                MarketCap = stockDTO.MarketCap
            };
        }
    }
}