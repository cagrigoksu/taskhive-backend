namespace TaskHive.Models.Project
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CreateUser { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public float Budget { get; set; }
    }

}