using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMeta.Shared.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [InverseProperty("User")]
        public virtual ICollection<TaskDefinition> TaskDefinitionUsers { get; set; } = new List<TaskDefinition>();
    
    }

}
