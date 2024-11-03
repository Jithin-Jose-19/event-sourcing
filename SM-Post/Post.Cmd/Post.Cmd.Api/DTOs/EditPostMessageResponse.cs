using Post.Common.DTOs;

namespace Post.Cmd.Api.DTOs
{
    public class EditPostMessageResponse : BaseResponse
    {
        public Guid Id {  get; set; }
    }
}
