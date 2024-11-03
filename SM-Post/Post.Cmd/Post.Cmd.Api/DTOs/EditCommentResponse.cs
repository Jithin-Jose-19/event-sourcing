using Post.Common.DTOs;

namespace Post.Cmd.Api.DTOs
{
    public class EditCommentResponse: BaseResponse
    {
        public Guid Id { get; set; }
    }
}
