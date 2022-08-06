using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyHeist.Entities
{
    public class HeistSkill
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [RegularExpression(@"[*]{1,10}",
          ErrorMessage = "Level is 1 to 10 stars (*)")]

        public string Level { get; set; } = "*";

        [Required]
        public int Members { get; set; }

        public Heist Heist { get; set; }
    }

}
