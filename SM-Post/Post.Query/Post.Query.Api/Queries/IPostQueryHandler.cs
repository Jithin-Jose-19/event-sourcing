using Post.Query.Domain.Entities;

namespace Post.Query.Api.Queries
{
    public interface IPostQueryHandler
    {
        Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query);

        Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query);

        Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query);

        Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query);

        Task<List<PostEntity>> HandleAsync(FindPostsByAuthorQuery query);
    }
}
