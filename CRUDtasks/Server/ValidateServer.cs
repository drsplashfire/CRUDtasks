using CRUDtasks.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CRUDtasks.Server
{
    public class ValidateServer
    {
        public bool PersonExists(AppDbContext context, int id)
        {
            return context.Persons.Any(e => e.Id == id);
        }
    }
}
