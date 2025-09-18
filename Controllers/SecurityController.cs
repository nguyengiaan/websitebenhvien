using Microsoft.AspNetCore.Mvc;

namespace websitebenhvien.Controllers
{
    [Route("api")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ILogger<SecurityController> _logger;

        public SecurityController(ILogger<SecurityController> logger)
        {
            _logger = logger;
        }

        [HttpPost("csp-violation")]
        public IActionResult ReportCspViolation([FromBody] CspViolationReport report)
        {
            try
            {
                _logger.LogWarning("CSP Violation reported: {@Report}", report);
                
                // Store violation in database or monitoring system
                // This helps identify security issues and adjust CSP policy
                
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing CSP violation report");
                return StatusCode(500);
            }
        }

        [HttpGet("security-check")]
        public IActionResult SecurityCheck()
        {
            var securityInfo = new
            {
                IsHttps = Request.IsHttps,
                UserAgent = Request.Headers.UserAgent.ToString(),
                SecureHeaders = new
                {
                    HasHSTS = Response.Headers.ContainsKey("Strict-Transport-Security"),
                    HasCSP = Response.Headers.ContainsKey("Content-Security-Policy"),
                    HasXFrameOptions = Response.Headers.ContainsKey("X-Frame-Options")
                },
                Timestamp = DateTime.UtcNow
            };

            return Ok(securityInfo);
        }
    }

    public class CspViolationReport
    {
        public string? Directive { get; set; }
        public string? BlockedUri { get; set; }
        public string? Timestamp { get; set; }
        public string? UserAgent { get; set; }
        public string? DocumentUri { get; set; }
    }
}
