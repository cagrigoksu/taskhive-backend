namespace TaskHive.Models.User
{
    public class UserProfileModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; } 
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Department { get; set; }
        public required string Role { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }
        public int DeleteUser { get; set; }
    }
}