
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IServices;
using System.Collections;
using System.Security.Claims;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendShipsController : ControllerBase
    {
        private readonly IFriendShipService _friendShipService;

        public FriendShipsController(IFriendShipService friendShipService)
        {
            _friendShipService = friendShipService;
        }

        [HttpGet("friend-ship")]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            try
            {
                var result = await _friendShipService.GetAllFriendShip();
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("List is empty");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("friend-ship-status")]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<IEnumerable>> GetAllFriendShipStatus()
        {
            try
            {
                var result = await _friendShipService.GetAllFriendShipStatus();
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("List is empty");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("friend-ship/{name}")]
        public async Task<ActionResult<IEnumerable>> GetAll(string name)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _friendShipService.SearchFriend(userId, name);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("List is empty");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("list-friend/{statusCode}")]
        //[Authorize(Roles = "Player, Admin")]
        public async Task<ActionResult<IEnumerable>> GetFriendList(string statusCode)
        {
            try
            {
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _friendShipService.GetFriendList(userId, statusCode);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound("List is empty");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost("add-friend/{friendId}")]
        public async Task<ActionResult> AddFriend(string friendId)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _friendShipService.AddFriend(userId, friendId);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete-friend/{friendId}&{statusCode}")]
        public async Task<ActionResult> UpdateFriendShipStatus(string friendId, string statusCode)
        {
            try
            {
                // get the current user logging in system
                string userId = HttpContext.User.FindFirstValue("Id");
                if (userId == null)
                {
                    return BadRequest("User id not Found, please login");
                }
                var result = await _friendShipService.UpdateFriendShipStatus(userId, friendId, statusCode);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
