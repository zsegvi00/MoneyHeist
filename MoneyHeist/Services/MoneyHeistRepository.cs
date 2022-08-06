using Microsoft.EntityFrameworkCore;
using MoneyHeist.DbContexts;
using MoneyHeist.Dtos;
using MoneyHeist.Entities;
using MoneyHeist.Mappings;

namespace MoneyHeist.Services
{
    public class MoneyHeistRepository : IMoneyHeistRepository
    {

        private readonly MoneyHeistContext _context;
        private readonly IEmailService _mailService;

        public MoneyHeistRepository(MoneyHeistContext context, IEmailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }


        public MemberWithSkillsDto GetMemberWithSkillsById (int memberId)
        {
            Member member = _context.Members.Include(m => m.MemberSkills).ThenInclude(ms => ms.Skill).FirstOrDefault(m => m.Id == memberId);
            return Map.MapMemberToMemberWithSkillsDto(member);
        }
        public void AddOneSkillToMember(Member member, SkillDto skillToAdd)
        {
            var existingSkill = _context.Skills.FirstOrDefault(s => s.Name.ToLower() == skillToAdd.Name.ToLower());
            if (existingSkill == null)
            {
                Skill newSkill = new Skill
                {
                    Name = skillToAdd.Name.ToLower(),
                };

                member.MemberSkills.Add(new MemberSkill
                {
                    Level = skillToAdd.Level,
                    Skill = newSkill,
                    Member = member
                });
            }
            else
            {
                member.MemberSkills.Add(new MemberSkill
                {
                    Level = skillToAdd.Level,
                    Skill = existingSkill,
                    Member = member
                });
            }

        }
        public int AddMemberWithSkills(MemberWithSkillsDto memberWithSkills)
        {
            Member member = Map.MapMemberDtoToMember(memberWithSkills);
            member.MemberSkills = new List<MemberSkill>();
            foreach (var skill in memberWithSkills.Skills)
            {
                AddOneSkillToMember(member, skill);
            }
            _context.Add(member);
            _mailService.Send("zvonimirsegvic@gmail.com", $"{member.Name} has been added", $"{member.Name} has been added");
            _context.SaveChanges();
            return member.Id;





        }


        public int AddHeistWithSkills(HeistWithSkillsDto heistWithSkills)
        {
            Heist heistToSave = Map.MapHeistDtoToHeist(heistWithSkills);
            heistToSave.HeistSkills = new List<HeistSkill>();
            foreach (var skill in heistWithSkills.Skills)
            {
                HeistSkill skillToAdd = new HeistSkill
                {
                    Members = skill.Members,
                    Level = skill.Level,
                    Name = skill.Name
                };
                heistToSave.HeistSkills.Add(skillToAdd);
            }
            _context.Add(heistToSave);
            _context.SaveChanges();
            return heistToSave.Id;

        }





        public string UpdateMemberSkills(int memberId, SkillForUpdateDto skillsForUpdate)
        {
            var memberToUpdate = _context.Members
                .Include(m => m.MemberSkills)
                .ThenInclude(m => m.Skill)
                .Where(m => m.Id == memberId)
                .FirstOrDefault();

            foreach (var skill in skillsForUpdate.skills)
            {
                var existingSkill = memberToUpdate.MemberSkills.FirstOrDefault(ms => ms.Skill.Name == skill.Name);
                if (existingSkill != null)
                {
                    existingSkill.Level = skill.Level;

                }
                else
                {
                    AddOneSkillToMember(memberToUpdate, skill);
                }
            }

            if (memberToUpdate.MemberSkills.Any(s => s.Skill.Name == skillsForUpdate.MainSkill))
            {
                memberToUpdate.MainSkill = skillsForUpdate.MainSkill;
                _context.SaveChanges();
                return "OK";

            }
            else
                return "MainSkill doesn't exist in Skill array";



        }



        public void RemoveOneSkillFromMember(Member member, SkillDto skillToRemove)
        {
            throw new NotImplementedException();
        }


        //Ista metoda, samo dodat sve u listu da nisu unique za uspjesnost pljacke
        public SkillsWithEligibleMembersDto GetHeistSkillsWithEligbleMembers(int heistId)
        {
            Heist heist = _context.Heists.Include(h => h.HeistSkills).FirstOrDefault(h => h.Id == heistId);



            List<Member> eligbleMembers = new List<Member>();

            SkillsWithEligibleMembersDto eligibleSkills = new SkillsWithEligibleMembersDto();
            eligibleSkills.HeistSkills = new List<SkillForHeistDto>();
            eligibleSkills.Members = new List<MemberWithSkillsDto>();

            List<Heist> overlappingHeists = FindOverlappingHeists(heist);

            foreach (var skill in heist.HeistSkills)
            {
                eligibleSkills.HeistSkills.Add(new SkillForHeistDto
                {
                    Name = skill.Name,
                    Level = skill.Level,
                    Members = skill.Members

                });


                var memberWithSkills = _context.Members.Include(m => m.MemberSkills).ThenInclude(ms => ms.Skill).Where(m => m.Skills.Any(s => s.Name.ToLower() == skill.Name.ToLower())).ToList();
                
                foreach (var member in memberWithSkills)
                {
                    var HeistsWithSameMember = overlappingHeists.Where(h => h.Members.Any(m => m.Name == member.Name)).ToList();
                    
                    foreach (var memberSkill in member.MemberSkills)
                    {
                        if ( memberSkill.Level.Count() >= skill.Level.Count() && 
                             memberSkill.Skill.Name.ToLower() == skill.Name.ToLower() && 
                            (member.Status == Member.StatusType.AVAILABLE || member.Status == Member.StatusType.RETIRED) && 
                            (!eligibleSkills.Members.Any(m => m.Email.ToLower() == member.Email.ToLower())) && 
                             HeistsWithSameMember.Count() == 0 )
                        
                        {
                            eligibleSkills.Members.Add(Map.MapMemberToMemberWithSkillsDto(member));
                        }
                    }
                }



            }

            return eligibleSkills;

        }

        public List<Member> GetEligibleMembersForAHeist(int heistId)
        {
            Heist heist = _context.Heists.Include(h => h.HeistSkills).FirstOrDefault(h => h.Id == heistId);

            List<Heist> overlappingHeists = FindOverlappingHeists(heist);

            List<Member> eligibleMembers = new List<Member>();
             
            foreach (var skill in heist.HeistSkills)
            {
                var memberWithSkills = _context.Members.Include(m => m.MemberSkills).ThenInclude(ms => ms.Skill).Where(m => m.Skills.Any(s => s.Name.ToLower() == skill.Name.ToLower())).ToList();
                foreach (var member in memberWithSkills)
                {
                    var HeistsWithSameMember = overlappingHeists.Where(h => h.Members.Any(m => m.Name == member.Name)).ToList();

                    foreach (var memberSkill in member.MemberSkills)
                    {
                        if (memberSkill.Level.Count() >= skill.Level.Count() &&
                             memberSkill.Skill.Name.ToLower() == skill.Name.ToLower() &&
                            (member.Status == Member.StatusType.AVAILABLE || member.Status == Member.StatusType.RETIRED) &&
                            (!eligibleMembers.Any(m => m.Email.ToLower() == member.Email.ToLower())) &&
                             HeistsWithSameMember.Count() == 0)

                        {
                            eligibleMembers.Add(member);
                        }
                    }
                }
            }

            return eligibleMembers;
        }

        public bool NamesAreEligible(List<Member> eligibleMembers, List<string> names)
        {
            var eligibleMembersNames = eligibleMembers.Select(m => m.Name.ToLower()).ToList();
            var newList = names.Select(m=> m.ToLower()).ToList();

            
            //Intersection
            var common = newList.FindAll(x => eligibleMembersNames.Contains(x));


            if (common.Count() == names.Count())
                return true;
            return false;
        }

        public List<Member> FindMembersSubset(List<Member> eligibleMembers, List<string> names)
        {
             List<Member> membersToReturn = new List<Member>();
             foreach (var name in names)
            {
                membersToReturn.Add(eligibleMembers.FirstOrDefault(m => m.Name.ToLower() == name.ToLower()));
            }
             return membersToReturn;

        }

        public void AddMemberstoHeist(List<Member> Members, int heistId)
        {
            Heist heist = _context.Heists.FirstOrDefault(h => h.Id == heistId);
            if(heist != null)
            {
                foreach (var member in Members)
                {
                    heist.Members.Add(member);
                }
            heist.Status = Heist.HeistStatus.READY;
            _context.SaveChanges();
            }

        }

        public List<Heist> FindOverlappingHeists(Heist heist)
        {
            List<Heist> heistsToReturn = _context.Heists.Include(h => h.Members).Where(h =>
                                         (h.StartTime <= heist.StartTime && heist.StartTime <= h.EndTime) ||
                                         (h.StartTime <= heist.EndTime && heist.EndTime <= h.EndTime) ||
                                         (heist.StartTime <= h.StartTime && heist.EndTime >= h.EndTime)).ToList();

            heistsToReturn.Remove(heist);

            return heistsToReturn;

        }

        public HeistWithSkillsDto GetHeistById (int heistId)
        {
            Heist heist = _context.Heists.Include(h => h.HeistSkills).SingleOrDefault(h => h.Id == heistId);
            return Map.MapHeistToHeistDto(heist);
        }

        public List<MemberNameSkillsDto> GetHeistMembers(int heistId)
        {
            var heist =_context.Heists.Include(h => h.Members).ThenInclude(m => m.MemberSkills).ThenInclude(ms => ms.Skill).FirstOrDefault(h => h.Id == heistId);
            HeistMembersSkillsDto heistMembers = new HeistMembersSkillsDto();
            heistMembers.Members = new List<MemberNameSkillsDto>();
            foreach (var member in heist.Members)
            {
                MemberNameSkillsDto memberToAdd = new MemberNameSkillsDto();
                memberToAdd.Skills = new List<SkillDto>();
                memberToAdd.Name = member.Name;
                foreach (var memberSkill in member.MemberSkills)
                {
                    memberToAdd.Skills.Add(new SkillDto {
                        Name = memberSkill.Skill.Name,
                        Level = memberSkill.Level
                    });
                }
                heistMembers.Members.Add(memberToAdd);
            }

            return heistMembers.Members;
        }

        public List<SkillForHeistDto> GetHeistSkills(int heistId)
        {
            var heist = _context.Heists.Include(h => h.HeistSkills).FirstOrDefault(h => h.Id == heistId);
            
            List<SkillForHeistDto> listToReturn = new List<SkillForHeistDto>();
            foreach (var skill in heist.HeistSkills)
            {
                listToReturn.Add(new SkillForHeistDto
                {
                    Name = skill.Name,
                    Level = skill.Level,
                    Members = skill.Members
                });
            }
            return listToReturn;
        }

        public int GetRequiredSkillsCount (int heistId)
        {
            int counter = 0;
            Heist heist = _context.Heists.Include(h => h.HeistSkills).Include(h => h.Members).ThenInclude(m => m.MemberSkills).ThenInclude(ms=> ms.Skill).FirstOrDefault(h => h.Id == heistId);

            foreach (var skill in heist.HeistSkills)
            {
                int particularSkillCounter = 0;
                var memberWithSkills = heist.Members;
                foreach (var member in memberWithSkills)
                {
                    

                    foreach (var memberSkill in member.MemberSkills)
                    {
                        if (memberSkill.Level.Count() >= skill.Level.Count() &&
                             memberSkill.Skill.Name.ToLower() == skill.Name.ToLower() && particularSkillCounter < skill.Members )

                        {
                            particularSkillCounter++;
                        }
                    }
                }
                //Linija koja pokazuje brojac za svaki pojedini skill
                //Console.WriteLine($"{skill.Name} {particularSkillCounter} / {skill.Members}");
                counter += particularSkillCounter;
            }
            return counter;
        }

        public string DetermineHeistOutcome(int skillCount, int heistId)
        {
            var heist = _context.Heists.Include(h => h.Members).Include(h => h.HeistSkills).FirstOrDefault(h => h.Id == heistId);
            int requiredSkillCount = heist.HeistSkills.Sum(hs => hs.Members);
            double outcomeDecider = (double) skillCount / requiredSkillCount;
            var memberCount = heist.Members.Count();
            
            if (outcomeDecider < 0.5)
            {
                heist.Outcome = Heist.HeistOutcome.FAILED;
                foreach (var member in heist.Members)
                {
                    Random rnd = new Random();
                    if (rnd.Next(2) == 0)
                    {
                        member.Status = Member.StatusType.EXPIRED;

                    }
                    else
                    {
                        member.Status = Member.StatusType.INCARCERATED;
                    }

                }

            }
            else if (outcomeDecider >= 0.5 && outcomeDecider < 0.75)
            {
                //Moze biti success ili fail, zato Random Number Generator
                Random rnd = new Random();
                if (rnd.Next(2) == 0)
                {
                    heist.Outcome = Heist.HeistOutcome.FAILED;

                    var members = heist.Members.Take(2 * memberCount / 3);
                    foreach (var member in members)
                    {
                        if (rnd.Next(2) == 0)
                        {
                            member.Status = Member.StatusType.INCARCERATED;
                        }
                        else
                        {
                            member.Status = Member.StatusType.EXPIRED;
                        }
                    }
                }
                else
                {
                    heist.Outcome = Heist.HeistOutcome.SUCCEEDED;

                    var members = heist.Members.Take(1 * memberCount / 3);
                    foreach (var member in members)
                    {
                        if (rnd.Next(2) == 0)
                        {
                            member.Status = Member.StatusType.INCARCERATED;
                        }
                        else
                        {
                            member.Status = Member.StatusType.EXPIRED;
                        }
                    }

                }

            }
            else if (outcomeDecider >= 0.75 && outcomeDecider < 1.0)
            {
                heist.Outcome = Heist.HeistOutcome.SUCCEEDED;

                var members = heist.Members.Take(1 * memberCount / 3);
                foreach (var member in members)
                {
                    
                    member.Status = Member.StatusType.INCARCERATED;
                            
                }

            }
            else
            {
                heist.Outcome = Heist.HeistOutcome.SUCCEEDED;
            }

            _context.SaveChanges();
            return heist.Outcome.ToString();

        }


    }
}
