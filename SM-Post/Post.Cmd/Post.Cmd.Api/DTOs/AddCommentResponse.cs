using Post.Common.DTOs;

namespace Post.Cmd.Api.DTOs
{
    public class AddCommentResponse: BaseResponse
    {
        public Guid Id { get; set; }
    }
}
