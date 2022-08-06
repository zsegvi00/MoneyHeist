namespace MoneyHeist.Dtos
{
    public class SkillsWithEligibleMembersDto
    {
        public List<SkillForHeistDto> HeistSkills { get; set; }

        public List<MemberWithSkillsDto> Members { get; set; }
    }
}
