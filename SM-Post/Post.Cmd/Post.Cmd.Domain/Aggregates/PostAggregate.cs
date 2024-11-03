using CQRS.Core.Domain;
using CQRS.Core.Messages;
using Post.Common.Events;
using System.Security.Cryptography;

namespace Post.Cmd.Domain.Aggregates
{
    public class PostAggregate : AggregateRoot
    {
        private bool _active;

        private string _author;

        private readonly Dictionary<Guid, Tuple<string, string>> _comments = new();

        public bool Active
        {
            get { return _active; }
            set { _active = value; }
        }

        public PostAggregate()
        {

        }

        public PostAggregate(Guid id, string author, string message)
        {
            RaiseEvent(new PostCreatedEvent()
            {
                Id = id,
                Author = author,
                Message = message,
                DatePosted = DateTime.Now,
            });
        }

        public void Apply(PostCreatedEvent @event)
        {
            _id = @event.Id;
            _active = true;
            _author = @event.Author;
        }

        public void EditMessage(string message)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit the mesage of an invalid post!");
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new InvalidOperationException($"The value of {nameof(message)} cannot be null or empty.Pls provide a valid {nameof(message)}");
            }

            RaiseEvent(new MessageUpdatedEvent()
            {
                Id = _id,
                Message = message,
            });
        }

        public void Apply(MessageUpdatedEvent @event)
        {
            _id = @event.Id;
        }

        public void LikePost()
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot like an inactive post");
            }

            RaiseEvent(new PostLikedEvent()
            {
                Id = _id,
            });
        }

        public void Apply(PostLikedEvent @event)
        {
            _id = @event.Id;
        }

        public void AddComment(string comment, string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot add comment to an inactive post");
            }


            if (string.IsNullOrWhiteSpace(comment))
            {
                throw new InvalidOperationException($"The value of {nameof(comment)} cannot be null or empty.Pls provide a valid {nameof(comment)}");
            }

            RaiseEvent(new CommentAddedEvent()
            {
                Id = _id,
                CommentId = Guid.NewGuid(),
                Comment = comment,
                Username = userName
            });
        }

        public void Apply(CommentAddedEvent @event)
        {
            _id = @event.Id;
            _comments.Add(@event.CommentId, new Tuple<string, string>(@event.Comment, @event.Username));
        }

        public void EditComment(Guid commentId, string comment, string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot edit comment of an inactive post");
            }

            if (!_comments[commentId].Item2.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to edit a comment that was made by another user");
            }

            RaiseEvent(new CommentUpdatedEvent()
            {
                Id = _id,
                CommentId = commentId,
                CommentText = comment,
                Username = userName,
                EditDate = DateTime.Now
            });
        }

        public void Apply(CommentUpdatedEvent @event)
        {
            _id = @event.Id;
            _comments[@event.CommentId] = new Tuple<string, string>(@event.CommentText, @event.Username);
        }

        public void RemoveComment(Guid commentId, string userName) 
        {
            if (!_active)
            {
                throw new InvalidOperationException("You cannot remove comment of an inactive post");
            }

            if (!_comments[commentId].Item2.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to remove a comment that was made by another user");
            }

            RaiseEvent(new CommentRemovedEvent()
            {
                Id = _id,
                CommentId = commentId
            });
        }

        public void Apply(CommentRemovedEvent @event)
        {
            _id = @event.Id;
            _comments.Remove(@event.CommentId);
        }

        public void DeletePost(string userName)
        {
            if (!_active)
            {
                throw new InvalidOperationException("Post has already been removed");
            }

            if(!_author.Equals(userName, StringComparison.CurrentCultureIgnoreCase))
            {
                throw new InvalidOperationException("You are not allowed to delete a post made by somebody else!");
            }

            RaiseEvent(new PostRemovedEvent()
            {
                Id = _id
            });
        }

        public void Apply(PostRemovedEvent @event)
        {
            _id = @event.Id;
            _active = false;
        }
    }
}
