using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext dbContext;
        private readonly IStockRepository stockRepo;
        public StockController(ApplicationDBContext _dbContext, IStockRepository _stockRepo)
        {
            stockRepo = _stockRepo;
            dbContext = _dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stocks = await stockRepo.GetAllAsync(query);

            var stockDTO = stocks.Select(s=> s.ToStockDTO());

            return Ok(stocks);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByID([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stock = await stockRepo.GetByIdAsync(id);

            if(stock == null){
                return NotFound();
            }

            return Ok(stock.ToStockDTO());
        }

        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDTO stockDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = stockDTO.ToStockFromCreateDTO();
            await stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetByID), new {id = stockModel.Id}, stockModel.ToStockDTO());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDTO updateDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await stockRepo.UpdateAsync(id,updateDTO);

            if (stockModel == null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDTO());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var stockModel = await stockRepo.DeleteAsync(id);
            if(stockModel == null)
            {
                return NotFound();
            }
            
            return NoContent();
        }

    }
}