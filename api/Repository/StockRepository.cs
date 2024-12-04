using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Model;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
   {
        private readonly ApplicationDBContext dbContext;
        
        public StockRepository(ApplicationDBContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<Stock> CreateAsync(Stock stockModel)
        {
           await dbContext.Stock.AddAsync(stockModel);
           await dbContext.SaveChangesAsync();
           return stockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var stockModel = await dbContext.Stock.FirstOrDefaultAsync(x => x.Id == id);
            if(stockModel == null){
                return null;
            }
            dbContext.Stock.Remove(stockModel);
            await dbContext.SaveChangesAsync();
            return stockModel;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
           var stocks = dbContext.Stock.Include(c => c.Comments).AsQueryable();
        
            if(!string.IsNullOrWhiteSpace(query.Company))
            {
                stocks = stocks.Where(s => s.Company.Contains(query.Company));
            }
            if(!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }
            if(!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.isDescending ? stocks.OrderByDescending(s=> s.Symbol) : stocks.OrderBy(s=> s.Symbol);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await dbContext.Stock.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<bool> StockExists(int id)
        {
            return await dbContext.Stock.AnyAsync(s => s.Id == id);
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDTO stockDTO)
        {
            var stockModel = await dbContext.Stock.FirstOrDefaultAsync(x=> x.Id == id);

            if(stockModel == null){
                return null;
            }

            stockModel.Symbol = stockDTO.Symbol;
            stockModel.Company = stockDTO.Company;
            stockModel.Purchase = stockDTO.Purchase;
            stockModel.Dividend = stockDTO.Dividend;
            stockModel.Industry = stockDTO.Industry;
            stockModel.MarketCap = stockDTO.MarketCap;

            await dbContext.SaveChangesAsync();
            
            return stockModel;

        }
    }
}