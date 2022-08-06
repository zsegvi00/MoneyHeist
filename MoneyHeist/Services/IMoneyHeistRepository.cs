using MoneyHeist.Dtos;
using MoneyHeist.Entities;

namespace MoneyHeist.Services
{
    public interface IMoneyHeistRepository
    {
        public MemberWithSkillsDto GetMemberWithSkillsById(int memberId);

        public void AddOneSkillToMember(Member member, SkillDto skillToAdd);

        public void RemoveOneSkillFromMember(Member member, SkillDto skillToRemove);

        public int AddMemberWithSkills(MemberWithSkillsDto member);

        public int AddHeistWithSkills(HeistWithSkillsDto heist);

        public string UpdateMemberSkills (int memberId, SkillForUpdateDto skillsForUpdate);

        public SkillsWithEligibleMembersDto GetHeistSkillsWithEligbleMembers(int heistId);

        public List<Member> GetEligibleMembersForAHeist(int heistId);
        public bool NamesAreEligible(List<Member> eligibleMembers, List<string> names);

        public List<Member> FindMembersSubset(List<Member> eligibleMembers, List<string> names);

        public void AddMemberstoHeist(List<Member> Members, int heistId);

        public HeistWithSkillsDto GetHeistById(int heistId);

        public List<MemberNameSkillsDto> GetHeistMembers(int heistId);

        public List<SkillForHeistDto> GetHeistSkills(int heistId);

        public int GetRequiredSkillsCount(int heistId);

        public string DetermineHeistOutcome(int skillCount, int heistId);

    }
}
