using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyHeist.Entities
{

    [Index(nameof(Email), IsUnique = true)]
    public class Member
    {
        public enum StatusType
        {
            AVAILABLE,
            EXPIRED,
            INCARCERATED,
            RETIRED
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[F|M]{1,1}$",
         ErrorMessage = "Sex is either F or M")]
        public string Sex { get; set; } = string.Empty;
        
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public ICollection<Skill> Skills { get; set; } = new List<Skill>();
        public ICollection<MemberSkill> MemberSkills { get; set; } = new List<MemberSkill>();

        public ICollection<Heist> Heists { get; set; } = new List<Heist>();


        public string? MainSkill { get; set; }

        [Required]
        public StatusType Status { get; set; }


    }
}
