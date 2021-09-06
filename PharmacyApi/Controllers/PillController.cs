using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PharmacyApi.Models;
using PharmacyApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PillController: ControllerBase
    {
        private readonly IRepository<Pill> _pillRepository;

        public PillController(IRepository<Pill> pillRepository)
        {
            _pillRepository = pillRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User,Manager")]
        public ActionResult<IEnumerable<Pill>> GetPills()
        {
            var claims = HttpContext.User.Claims;
            var role = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);

            return _pillRepository.GetAll().ToList();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<Pill>> AddPill(Pill pill)
        {
            return await _pillRepository.AddAsync(pill);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Pill>> DeletePill(Pill pill)
        {
            return await _pillRepository.DeleteAsync(pill);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<Pill>> UpdatePill(Pill pill)
        {
            return await _pillRepository.UpdateAsync(pill);
        }
    }
}
