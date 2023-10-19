using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Short_Polling_Design_Pattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        private static readonly IDictionary<Guid, int> Jobs = new Dictionary<Guid, int>();


        [HttpPost("submit")]
        public IActionResult SubmitJob()
        {
            var jobId = Guid.NewGuid();
            Jobs[jobId] = 0;
            //Jobs.Add(jobId, 0);
            _ = UpdateJobAsync(jobId, 0);

            return Ok(jobId);
        }

        [HttpGet("checkjob")]
        public IActionResult CheckJob(Guid jobId)
        {
            // Check if the job exists
            if (Jobs.ContainsKey(jobId))
            {
                return Ok(Jobs[jobId]);
            }
            return NotFound();
        }


        private async Task UpdateJobAsync(Guid jobId, int progress)
        {
            while (true)
            {
                if (progress == 100)
                    return;

                // Simulate progress update
                progress += 10;
                Jobs[jobId] = progress;

                // Introduce an asynchronous delay without blocking the thread
                await Task.Delay(2000);
            }
        }
    }
}
