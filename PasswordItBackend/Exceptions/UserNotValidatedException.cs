using System;
using PasswordItBackend.Objects;
namespace PasswordItBackend.Exceptions
{
    /// <summary>
    /// Thrown when trying to access user data before it has been unlocked.
    /// </summary>
    public class UserNotValidatedException : Exception
    {
        // - - Exception data - - 
        public User UnvalidatedUser { get; init; }

        public UserNotValidatedException(User user)
        {
            UnvalidatedUser = user;
        }

        public UserNotValidatedException(string message, User user) : base(message)
        {
            UnvalidatedUser = user;
        }

        public UserNotValidatedException(string message, User user, Exception inner) : base(message, inner)
        {
            UnvalidatedUser = user;
        }
    }
}