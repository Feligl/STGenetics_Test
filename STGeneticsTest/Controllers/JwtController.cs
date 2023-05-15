using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using STGeneticsTest.Utilities;

namespace STGeneticsTest.Controllers
{
    [ApiController]
    [AllowAnonymous]
    [Route("[controller]/[action]")]
    public class JwtController : ControllerBase
    {
        #region Constructor and Properties
        private readonly Jwt _jwt;
        private readonly ILogger<JwtController> _logger;

        public JwtController(ILogger<JwtController> logger, Jwt jwt)
        {
            _logger = logger;
            _jwt = jwt;
        }
        #endregion

        #region Public Methods & Endpoints
        [HttpGet]
        public IActionResult Generate(bool createJwt)
        {
            return Ok(_jwt.GenerateJwt(createJwt));
        }
        #endregion
    }
}