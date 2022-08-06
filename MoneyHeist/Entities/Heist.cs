using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyHeist.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Heist
    {

        public enum HeistStatus
        {
            PLANNING,
            READY,
            IN_PROGRESS,
            FINISHED
        }

        public enum HeistOutcome
        {
            NOT_SET,
            SUCCEEDED,
            FAILED
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        
        public string Location { get; set; } = string.Empty;

        
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public ICollection<HeistSkill> HeistSkills { get; set; } = new List<HeistSkill>();


        public ICollection<Member> Members { get; set; } = new List<Member>();

        public HeistStatus Status { get; set; } = HeistStatus.PLANNING;
        public HeistOutcome Outcome { get; set; } = HeistOutcome.NOT_SET;





    }
}
