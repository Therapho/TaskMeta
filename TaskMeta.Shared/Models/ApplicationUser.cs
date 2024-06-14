using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using TaskMeta.Shared.Interfaces;

namespace TaskMeta.Shared.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [InverseProperty("User")]
        public virtual ICollection<TaskDefinition> TaskDefinitionUsers { get; set; } = [];

        [InverseProperty("User")]
        public virtual ICollection<Job> JobUsers { get; set; } = [];
        [InverseProperty("User")]
        public virtual ICollection<TaskWeek> UserTaskWeeks { get; set; } = [];

    }

}
