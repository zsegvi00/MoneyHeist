using Microsoft.EntityFrameworkCore;
using MoneyHeist.DbContexts;

namespace MoneyHeist.Services
{
    public class BackgroundUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        
        private const int Delay = 1000 * 6 ;

        public BackgroundUpdateService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
           _serviceProvider = serviceProvider;
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                using (var scope =_serviceProvider.CreateScope())
                {
                await Task.Delay(Delay, stoppingToken);
                var _context = scope.ServiceProvider.GetRequiredService<MoneyHeistContext>();
                await UpdateStatus(_context); 
                }
            }
        }

        public Task UpdateStatus(MoneyHeistContext _context)
        {
            _context.Heists.Where(h => h.StartTime < DateTime.Now && h.Status == Entities.Heist.HeistStatus.READY).ToList().ForEach(h => h.Status = Entities.Heist.HeistStatus.IN_PROGRESS);
           
            var HeistsToStart = _context.Heists.Where(h => h.StartTime < DateTime.Now).ToList();
            var HeistsToEnd = _context.Heists.Where(h => h.EndTime < DateTime.Now && h.Status == Entities.Heist.HeistStatus.IN_PROGRESS).ToList();
            
            
            
            if (HeistsToStart.Any())
            { 
                foreach (var heist in HeistsToStart)
                {

                    Console.WriteLine($"{heist.Name} {heist.StartTime} {heist.Status.ToString()} {DateTime.Now}");
                }
            }

            if (HeistsToEnd.Any())
            {
                foreach (var heist in HeistsToEnd)
                {
                    var heistWithSkills = _context.Heists.Include(h => h.HeistSkills).First(h => h.Name.ToLower() == heist.Name.ToLower());
                    var heistMembers = _context.Members.Include(m => m.Heists).Include(m => m.MemberSkills).ThenInclude(ms => ms.Skill).Where(m => m.Heists.Any(h => h.Name == heist.Name));
                    foreach (var heistMember in heistMembers)
                    {
                        foreach(var memberSkill in heistMember.MemberSkills)
                        {
                            if(heistWithSkills.HeistSkills.Any(hs => hs.Name.ToLower() == memberSkill.Skill.Name.ToLower()))
                            {
                                var seconds = (heistWithSkills.EndTime - heistWithSkills.StartTime).TotalSeconds;
                                int levelGain = (int) seconds / Int32.Parse(_configuration["LevelUpTime"]);
                                string levelStars = string.Concat(Enumerable.Repeat("*", levelGain));
                                memberSkill.Level += levelStars;
                                if (memberSkill.Level.Length > 10)
                                    memberSkill.Level = memberSkill.Level.Substring(0, 10);
                            }
                        }
                    }
                    
                    Console.WriteLine($"Finished heists {heist.Name} {heist.StartTime} {heist.EndTime} {heist.Status.ToString()}");
                    heist.Status = Entities.Heist.HeistStatus.FINISHED;

                }
            }
           
            _context.SaveChanges();
            
            return Task.CompletedTask;
        }
    }
}
