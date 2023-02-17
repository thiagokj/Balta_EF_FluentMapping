using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Balta_EntityFramework.Models
{
    public class Category
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public IList<Post> Posts { get; set; }
    }
}