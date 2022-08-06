using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoneyHeist.DbContexts;
using MoneyHeist.Dtos;
using MoneyHeist.Entities;
using MoneyHeist.Services;
using MoneyHeist.Mappings;

namespace MoneyHeist.Controllers
{
    [Route("member")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly MoneyHeistContext _context;
        private readonly IMoneyHeistRepository _repository;

        public MemberController(MoneyHeistContext context, IMoneyHeistRepository repository)
        {
            _context = context;
            _repository = repository;
        }





        [HttpGet("{memberid}", Name = "GetMember")]

        public ActionResult GetMember(int memberId)
        {
            
            if(_context.Members.FirstOrDefault(m => m.Id == memberId) == null)
                return NotFound();


            return Ok(_repository.GetMemberWithSkillsById(memberId));
        }

        [HttpGet("{memberid}/skills")]

        public ActionResult GetMemberSkills(int memberId)
        {

            if (_context.Members.FirstOrDefault(m => m.Id == memberId) == null)
                return NotFound();

            
            return Ok(Map.MapMemberToSkillsOnly(_repository.GetMemberWithSkillsById(memberId)));
        }





        [HttpPost]
        public ActionResult AddMember(MemberWithSkillsDto member)
        {
            if (_context.Members.Any(m => m.Email == member.Email))
                return BadRequest($"Member with {member.Email} already exists");
            var skillCount = member.Skills.GroupBy(s => s.Name).Where(s => s.Count() > 1);
            
            if (skillCount.Count() > 0)
                return BadRequest("You added 2 skills with the same name");
            
            if (!member.Skills.Any(s => s.Name == member.MainSkill))
                return BadRequest("Main skill is not in the skill list");

            if (member.Status.ToUpper() != "AVAILABLE" && member.Status.ToUpper() != "EXPIRED" && member.Status.ToUpper() != "INCARCERATED" && member.Status.ToUpper() != "RETIRED")
                member.Status = "AVAILABLE";
            
            int memberId = _repository.AddMemberWithSkills(member);
            return CreatedAtRoute("GetMember", new {MemberId =memberId }, null);
            
        }

        [HttpPut("{memberid}/skills")]

        public ActionResult UpdateMemberSkills(int memberId, SkillForUpdateDto skills)
        {
            var exists = _context.Members.Any(m => m.Id == memberId);
            if (!exists)
                return NotFound();

           string result = _repository.UpdateMemberSkills(memberId, skills);
            if(result == "OK")
                return NoContent();
            
            return BadRequest(result);

        }

        [HttpDelete("{memberid}/skills/{skillname}")]

        public ActionResult DeleteSkill (int memberId, string skillName)
        {
            var exists = _context.Members.Any(m => m.Id == memberId);
            if (!exists)
                return NotFound();
            
            var memberToUpdate = _context.Members
                .Include(m => m.MemberSkills)
                .ThenInclude(m => m.Skill)
                .Where(m => m.Id == memberId)
                .FirstOrDefault();

            MemberSkill skillToRemove = memberToUpdate.MemberSkills.FirstOrDefault(ms => ms.Skill.Name.ToLower() == skillName.ToLower());
            if (skillToRemove == null)
                return NotFound();

            _context.Remove(skillToRemove);
            _context.SaveChanges();
            return Ok();

        }
    }
}
