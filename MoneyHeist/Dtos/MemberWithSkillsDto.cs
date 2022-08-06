using System.ComponentModel.DataAnnotations;

namespace MoneyHeist.Dtos
{
    public class MemberWithSkillsDto
    {





        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[F|M]{1,1}$",
          ErrorMessage = "Sex is either F or M")]
        public string Sex { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public ICollection<SkillDto> Skills { get; set; } = new List<SkillDto>();
        

        public string? MainSkill { get; set; }

        
        public string Status { get; set; } = "AVAILABLE" ;

    }
}
