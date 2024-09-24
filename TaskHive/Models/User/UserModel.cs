namespace TaskHive.Models.User
{
    public class UserModel
    {
        public int Id { get; set;}
        public string Email { get; set;}
        public DateTime LogOnDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteDate { get; set; }
        public int DeleteUser { get; set; }
    }
}