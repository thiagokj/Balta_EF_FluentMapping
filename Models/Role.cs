namespace Balta_EntityFramework.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }

        public List<User> Users { get; set; }
    }
}