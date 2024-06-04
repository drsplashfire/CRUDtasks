using System.Text.Json.Serialization;

namespace CRUDtasks.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public string Address { get; set; }

        [JsonIgnore]
        public ICollection<Person> Persons { get; set; } = new HashSet<Person>();


    }
}
