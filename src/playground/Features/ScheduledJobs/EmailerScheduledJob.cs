using EPiServer.PlugIn;
using EPiServer.Scheduler;
using playground.Features.Email;

namespace playground.Features.ScheduledJobs
{
    [ScheduledPlugIn(
       DisplayName = "[Playground] - Emailer",
       SortIndex = 20000)]
    public class EmailerScheduledJob : ScheduledJobBase
    {
        private readonly IEmailService _emailService;

        public EmailerScheduledJob(IEmailService emailService)
        {
            _emailService = emailService;
        }

        public override string Execute()
        {
            _emailService.SendEmailAsync("anything@paulmcgann83-personal-nxfj.imitate.email", "Hello World", "Sending an email using code");

            _emailService.SendEmailFluentAsync("anything@paulmcgann83-personal-nxfj.imitate.email", "Hello World");

            return "Emails Sent!";
        }
    }
}
