using System.ComponentModel.DataAnnotations;

namespace MoneyHeist.Dtos
{
    public class SkillForHeistDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [RegularExpression(@"[*]{1,10}",
          ErrorMessage = "Level is 1 to 10 stars (*)")]
        
        public string Level { get; set; } = "*";

        [Required]
        public int Members { get; set; }
    }
}
