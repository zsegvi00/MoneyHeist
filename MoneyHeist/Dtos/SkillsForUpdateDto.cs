namespace MoneyHeist.Dtos
{
    public class SkillForUpdateDto
    {
        public List<SkillDto> skills { get; set; } = new List<SkillDto>();

        public string MainSkill { get; set; } = string.Empty;
    }
}
