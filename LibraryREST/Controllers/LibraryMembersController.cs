using Microsoft.AspNetCore.Mvc;
using Library.Infrastructure.Models;
using Library.Infrastructure.Services;
using LibraryREST.Models;

namespace LibraryREST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryMembersController : ControllerBase
    {
        private readonly ICrudServiceAsync<LibraryMemberModel> _memberService;

        public LibraryMembersController(ICrudServiceAsync<LibraryMemberModel> memberService)
        {
            _memberService = memberService;
        }

        // GET: api/LibraryMembers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibraryMemberApiModel>>> GetAll()
        {
            var members = await _memberService.ReadAllAsync();
            var result = members.Select(m => new LibraryMemberApiModel
            {
                Id = m.Id,
                FullName = m.FullName,
                Email = m.Email
            });

            return Ok(result);
        }

        // GET: api/LibraryMembers/1
        [HttpGet("{id:int}")]
        public async Task<ActionResult<LibraryMemberApiModel>> Get(int id)
        {
            var member = await _memberService.ReadAsync(id);
            if (member == null)
                return NotFound();

            var dto = new LibraryMemberApiModel
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email
            };

            return Ok(dto);
        }

        // POST: api/LibraryMembers
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LibraryMemberCreateModel model)
        {
            var newMember = new LibraryMemberModel
            {
                FullName = model.FullName,
                Email = model.Email,
                RegisteredAt = DateTime.UtcNow
            };

            var created = await _memberService.CreateAsync(newMember);
            if (!created) return BadRequest("Could not create member.");

            await _memberService.SaveAsync();

            return CreatedAtAction(nameof(Get), new { id = newMember.Id }, newMember);
        }

        // PUT: api/LibraryMembers/1
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] LibraryMemberUpdateModel model)
        {
            var member = await _memberService.ReadAsync(id);
            if (member == null)
                return NotFound();

            member.FullName = model.FullName;
            member.Email = model.Email;

            var updated = await _memberService.UpdateAsync(member);
            if (!updated) return BadRequest("Update failed.");

            await _memberService.SaveAsync();

            return NoContent();
        }

        // DELETE: api/LibraryMembers/1
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var member = await _memberService.ReadAsync(id);
            if (member == null)
                return NotFound();

            var deleted = await _memberService.RemoveAsync(member);
            if (!deleted) return BadRequest("Delete failed.");

            await _memberService.SaveAsync();

            return NoContent();
        }
    }
}
