using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository commentRepo;
        private readonly IStockRepository stockRepo;

        public CommentController(ICommentRepository _commentRepo, IStockRepository _stockRepo)
        { 
            commentRepo = _commentRepo;
            stockRepo = _stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await commentRepo.GetAllAsync();

            var commentDto = comment.Select(x => x.ToCommentDTO());

            return Ok(commentDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await commentRepo.GetByIDAsync(id);

            if(comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDTO());
        }

        [HttpPost("{id:int}")]
        public async Task<IActionResult> Create([FromRoute] int id, CreateCommentDTO commentDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!await stockRepo.StockExists(id))
            {
                return BadRequest("Stock does not exist!");
            }
            var commentModel = commentDTO.ToCommentFromDTO(id);
            await commentRepo.CreateCommentAsync(commentModel);
            return CreatedAtAction(nameof(GetById), new { id = commentModel.ID }, commentModel.ToCommentDTO());
            
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDTO updateDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var comment = await commentRepo.UpdateCommentAsync(id, updateDTO.ToCommentFromDTOUpdate());

            if(comment == null){
                return NotFound("Comment not found");
            }

            return Ok(comment.ToCommentDTO());

        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var commentModel = await commentRepo.DeleteCommentAsync(id);

            if(commentModel == null){
                return NotFound("Comment not found");
            }

            return Ok(commentModel);
        }
    }
}