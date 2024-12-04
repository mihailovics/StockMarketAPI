using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext dbContext;
        public CommentRepository(ApplicationDBContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<Comment> CreateCommentAsync(Comment commentModel)
        {
            await dbContext.Comment.AddAsync(commentModel);
            await dbContext.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment> DeleteCommentAsync(int id)
        {
           var commentModel = await dbContext.Comment.FirstOrDefaultAsync(i => i.ID == id);

           if(commentModel == null){
            return null;
           }

           dbContext.Comment.Remove(commentModel);
           await dbContext.SaveChangesAsync();

           return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await dbContext.Comment.ToListAsync();
        }

        public async Task<Comment?> GetByIDAsync(int id)
        {
            return await dbContext.Comment.FindAsync(id);
        }

        public async Task<Comment?> UpdateCommentAsync(int id, Comment commentModel)
        {
            var existingComment = await dbContext.Comment.FindAsync(id);

            if(existingComment == null){
                return null;
            }

            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;

            await dbContext.SaveChangesAsync();

            return existingComment;

        }
    }
}