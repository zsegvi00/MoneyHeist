using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyHeist.DbContexts;
using MoneyHeist.Dtos;
using MoneyHeist.Entities;
using MoneyHeist.Services;

namespace MoneyHeist.Controllers
{
    [Route("heist")]
    [ApiController]
    public class HeistController : ControllerBase
    {
        private readonly MoneyHeistContext _context;
        private readonly IMoneyHeistRepository _repository;

        public HeistController(MoneyHeistContext context, IMoneyHeistRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        [HttpPost]
        public ActionResult AddHeist(HeistWithSkillsDto heist)
        {
            if (_context.Heists.Any(h => h.Name == heist.Name))
                return BadRequest($"Heist named {heist.Name} already exists");
            var startTime = DateTime.Parse(heist.StartTime, null, System.Globalization.DateTimeStyles.RoundtripKind);
            var endTime = DateTime.Parse(heist.EndTime, null, System.Globalization.DateTimeStyles.RoundtripKind);

            if (endTime < startTime)
                return BadRequest("End time should be after start time");

            if (endTime < DateTime.UtcNow)
                return BadRequest("End time can't be in the past");



            var skillCount = heist.Skills.GroupBy(s => s.Name.ToLower()).Where(s => s.Count() > 1);
            foreach (var skill in skillCount)
            {
                var test = skill;
                var testing = skill;
                var levelCount = skill.GroupBy(s => s.Level).Where(s => s.Count() > 1);
                if (levelCount.Count() > 0)
                    return BadRequest("2 skills with same name and level added");
            }

            int heistId = _repository.AddHeistWithSkills(heist);

            return CreatedAtRoute("GetHeist", new { HeistId = heistId }, null);

        }

        [HttpGet("{heistid}", Name = "GetHeist")]
        
        public ActionResult GetHeist(int heistId)
        {
            if (_context.Heists.FirstOrDefault(h => h.Id == heistId) == null)
                return NotFound();

            return Ok(_repository.GetHeistById(heistId));
        }

        [HttpGet("{heistid}/eligible_members")]
        public ActionResult GetEligibleMembers(int heistId)
        {
           if(_context.Heists.FirstOrDefault(h => h.Id == heistId)==null)
                return NotFound();
           var response = _repository.GetHeistSkillsWithEligbleMembers(heistId);
           
           return Ok(response);
        }

        [HttpPut("{heistid}/members")]
        public ActionResult AddMembersToHeist(int heistId, HeistMemberDto memberNames)
        {
            Heist heist = _context.Heists.FirstOrDefault(h => h.Id == heistId);
            if (heist == null)
                return NotFound();
            if (heist.Status != Heist.HeistStatus.PLANNING)
                return StatusCode(405);
            
            var eligibleMembers = _repository.GetEligibleMembersForAHeist(heistId);

            var areEligible = _repository.NamesAreEligible(eligibleMembers, memberNames.Members);
            if (areEligible)
            {
                var membersToAdd = _repository.FindMembersSubset(eligibleMembers, memberNames.Members);
                _repository.AddMemberstoHeist(membersToAdd, heistId);
                 HttpContext.Response.Headers.Add("Content-location", $"/heist/{heistId}/members");
                return NoContent();
            }

            return BadRequest("One or more members not eligible");
            
        }

        [HttpPut("{heistid}/start")]
        public ActionResult StartHeist (int heistId)
        {
            Heist heist = _context.Heists.FirstOrDefault(h => h.Id == heistId);
            if (heist == null)
                return NotFound();
            if (heist.Status != Heist.HeistStatus.READY)
                return StatusCode(405);
            heist.Status = Heist.HeistStatus.IN_PROGRESS;
            //heist.StartTime = DateTime.Now;
            _context.SaveChanges();
            HttpContext.Response.Headers.Add("Content-location", $"/heist/{heistId}/status");
            return Ok();


        }

        [HttpGet("{heistid}/members")]
        public ActionResult GetHeistMembers(int heistId)
        {
            Heist heist = _context.Heists.FirstOrDefault(h => h.Id == heistId);
            if (heist == null)
                return NotFound();
            if (heist.Status == Heist.HeistStatus.PLANNING)
                return StatusCode(405);
           
            
            return Ok(_repository.GetHeistMembers(heistId));


        }

        [HttpGet("{heistid}/skills")]
        public ActionResult GetSkillsForAHeist(int heistId)
        {
            Heist heist = _context.Heists.FirstOrDefault(h => h.Id == heistId);
            if (heist == null)
                return NotFound();



            return Ok(_repository.GetHeistSkills(heistId));


        }

        [HttpGet("{heistid}/status")]
        public ActionResult GetHeistStatus(int heistId)
        {
            Heist heist = _context.Heists.FirstOrDefault(h => h.Id == heistId);
            if (heist == null)
                return NotFound();



            return Ok(new { Status = heist.Status.ToString() });


        }

        [HttpGet("{heistid}/outcome")]
        public ActionResult GetHeistOutcome(int heistId)
        {
            Heist heist = _context.Heists.FirstOrDefault(h => h.Id == heistId);
            if (heist == null)
                return NotFound();
            if (heist.Status != Heist.HeistStatus.FINISHED)
                return StatusCode(405);


            if (heist.Outcome == Heist.HeistOutcome.NOT_SET)
            {
            var skillCount = _repository.GetRequiredSkillsCount(heistId);
                _repository.DetermineHeistOutcome(skillCount, heistId);
            return Ok( new {Outcome = heist.Outcome.ToString()});

            }

            else
                return Ok(new { Outcome = heist.Outcome.ToString() });



        }







    }
}
