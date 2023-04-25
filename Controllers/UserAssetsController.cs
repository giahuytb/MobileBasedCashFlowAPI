﻿
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Security.Claims;

using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using Microsoft.AspNetCore.Authorization;
using MobileBasedCashFlowAPI.Common;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAssetsController : ControllerBase
    {
        private readonly UserAssetRepository _userAssetRepository;

        public UserAssetsController(UserAssetRepository inventoryService)
        {
            _userAssetRepository = inventoryService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetALl()
        {
            var result = await _userAssetRepository.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }


        [Authorize(Roles = "Player, Admin")]
        [HttpGet("my-asset")]
        public async Task<ActionResult<UserAsset>> GetById()
        {
            // get the current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            string users = HttpContext.User.FindFirstValue("roleName");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _userAssetRepository.GetAsync(Int32.Parse(userId));
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this user inventory ");

        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        public async Task<ActionResult> BuyItem(int itemId)
        {
            // get the id of current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _userAssetRepository.CreateAsync(itemId, Int32.Parse(userId));
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);

        }

        [HttpPut]
        public async Task<ActionResult> UpdateLastUsed(int itemId)
        {
            // get the id of current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _userAssetRepository.UpdateLastUsedAsync(itemId, Int32.Parse(userId));
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
