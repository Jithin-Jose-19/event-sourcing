﻿using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries
{
    public class PostQueryHandler(IPostRepository postRepository) : IPostQueryHandler
    {
        private readonly IPostRepository _postRepository = postRepository;

        public async Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query)
        {
            return await _postRepository.ListAllAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
        {
            var post = await _postRepository.GetByIdAsync(query.Id);

            return [post];
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query)
        {
            return await _postRepository.ListWithCommentsAsync();
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query)
        {
            return await _postRepository.ListWithLikesAsync(query.NumberOfLikes);
        }

        public async Task<List<PostEntity>> HandleAsync(FindPostsByAuthorQuery query)
        {
            return await _postRepository.ListByAuthorAsync(query.Author);
        }
    }
}
