using Post.Common.DTOs;

namespace Post.Cmd.Api.DTOs
{
    public class DeletePostResponse : BaseResponse
    {
        public Guid Id { get; set; }
    }
}
