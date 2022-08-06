using MoneyHeist.Dtos;
using MoneyHeist.Entities;
using static MoneyHeist.Entities.Heist;

namespace MoneyHeist.Mappings
{
    public class Map
    {
        public static Member MapMemberDtoToMember(MemberWithSkillsDto member)
        {
            Member memberToSave = new Member();
            memberToSave.Name = member.Name;
            memberToSave.Status = (Member.StatusType) Enum.Parse(typeof(Member.StatusType), member.Status);
            memberToSave.Email = member.Email;
            memberToSave.Sex = member.Sex;
            memberToSave.MainSkill = member.MainSkill;
            

            return memberToSave;



    }

        public static Heist MapHeistDtoToHeist(HeistWithSkillsDto heist)
        {
            Heist heistToSave = new Heist();
            heistToSave.Name = heist.Name;
            heistToSave.Location = heist.Location;
            heistToSave.StartTime = DateTime.Parse(heist.StartTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
            heistToSave.EndTime = DateTime.Parse(heist.EndTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
            Enum.TryParse<HeistStatus>(heist.Status, true, out var enumStatus);
            heistToSave.Status = enumStatus;


            return heistToSave;
        }

        public static MemberWithSkillsDto MapMemberToMemberWithSkillsDto(Member member )
        {
            MemberWithSkillsDto memberToAdd = new MemberWithSkillsDto();
            memberToAdd.Name = member.Name;
            memberToAdd.Status = member.Status.ToString();
            memberToAdd.Email = member.Email;
            memberToAdd.Sex = member.Sex;
            memberToAdd.MainSkill = member.MainSkill;
            foreach (var memberSkill in member.MemberSkills)
            {
                memberToAdd.Skills.Add(new SkillDto
                {
                    Level = memberSkill.Level,
                    Name = memberSkill.Skill.Name
                });
            }

            return memberToAdd;
        }

        public static SkillForUpdateDto MapMemberToSkillsOnly(MemberWithSkillsDto member)
        {
            SkillForUpdateDto skillForUpdate = new SkillForUpdateDto();
            skillForUpdate.MainSkill = member.MainSkill;
            skillForUpdate.skills = member.Skills.ToList();
            return skillForUpdate;

        }

        public static HeistWithSkillsDto MapHeistToHeistDto(Heist heist)
        {
            HeistWithSkillsDto heistToShow = new HeistWithSkillsDto
            {
                Name = heist.Name,
                Location = heist.Location,
                StartTime = heist.StartTime.ToString("O", System.Globalization.CultureInfo.InvariantCulture),
                EndTime = heist.EndTime.ToString("O", System.Globalization.CultureInfo.InvariantCulture),
                Status = heist.Status.ToString()

            };

            heistToShow.Skills = new List<SkillForHeistDto>();
            foreach (var skill in heist.HeistSkills)
            {
                heistToShow.Skills.Add(new SkillForHeistDto
                {
                    Name=skill.Name,
                    Level = skill.Level,
                    Members = skill.Members
                });
            }



            return heistToShow;
        }
    }
}
