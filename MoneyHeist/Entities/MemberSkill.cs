using System.ComponentModel.DataAnnotations;

namespace MoneyHeist.Entities
{
    public class MemberSkill
    {

        public Skill Skill { get; set; }
        public Member Member { get; set; }
        
        [RegularExpression(@"[*]{1,10}",
         ErrorMessage = "Level is 1 to 10 stars (*)")]
        public string Level { get; set; }
    }
}
