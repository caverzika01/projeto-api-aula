using BCrypt.Net;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.EntityFrameworkCore;
using MongoDB.Bson.Serialization.IdGenerators;

namespace _2TDSPK.Database.Models
{
    [Collection("users")]
    public class User
    {
        private User(){}

        public User(string email, string password)
        {
            Email = email;
            SetPassword(password);
        }

        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public ObjectId Id { get; set; }

        [BsonElement("email")]
        public string Email { get; private set; }

        [BsonElement("password")]
        public string Password { get; private set; }

        [BsonElement("blocked")]
        protected bool Blocked { get; set; }

        [BsonElement("status")]
        public bool Status { get; set; }

        private void SetPassword(string password)
        {
            Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        }

        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, Password);
        }

        public void AlterBlockedStatus(bool blocked)
        {
            Blocked = blocked;
        }
    }
}
