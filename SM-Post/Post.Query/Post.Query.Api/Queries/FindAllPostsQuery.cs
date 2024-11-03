using CQRS.Core.Queries;

namespace Post.Query.Api.Queries
{
    public class FindAllPostsQuery : BaseQuery
    {
        public Guid Id { get; set; }
    }
}
