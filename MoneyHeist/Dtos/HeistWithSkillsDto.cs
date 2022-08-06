using System.ComponentModel.DataAnnotations;

namespace MoneyHeist.Dtos
{
    public class HeistWithSkillsDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string StartTime { get; set; }

        [Required]
        public string EndTime { get; set; }

        [Required]
        public List<SkillForHeistDto> Skills { get; set; }

        public string Status { get; set; } = "PLANNING";

        
    }
}
