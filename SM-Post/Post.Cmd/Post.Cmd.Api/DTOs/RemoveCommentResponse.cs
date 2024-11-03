using Post.Common.DTOs;

namespace Post.Cmd.Api.DTOs
{
    public class RemoveCommentResponse: BaseResponse
    {
        public Guid Id { get; set; }
    }
}
