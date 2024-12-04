using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Comment;
using api.Model;

namespace api.Mappers
{
    public static  class CommentMapper
    {
        public static CommentDTO ToCommentDTO(this Comment commentModel)
        {
            return new CommentDTO
            {
                ID = commentModel.ID,
                Title = commentModel.Title,
                Content = commentModel.Content,
                CreatedOn = commentModel.CreatedOn,
                StockId = commentModel.StockId
            };
        }
        public static Comment ToCommentFromDTO(this CreateCommentDTO commentModel, int stockId)
        {
            return new Comment
            { 
                Title = commentModel.Title,
                Content = commentModel.Content,
                StockId = stockId
            };
        }

        public static Comment ToCommentFromDTOUpdate(this UpdateCommentDTO commentModel)
        {
            return new Comment
            { 
                Title = commentModel.Title,
                Content = commentModel.Content
            };
        }
    }
}