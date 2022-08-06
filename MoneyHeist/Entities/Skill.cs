using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyHeist.Entities
{
    [Index(nameof(Name), IsUnique = true)]
    public class Skill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = "";

        public ICollection<Member> Members { get; set; } = new List<Member>();

        public ICollection<MemberSkill> MemberSkills { get; set; } = new List<MemberSkill>();

        

       


    }
}
