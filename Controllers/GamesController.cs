﻿
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.DTO;
using MobileBasedCashFlowAPI.Repository;
using MobileBasedCashFlowAPI.Models;
using System.Collections;
using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameRepository _gameService;

        public GamesController(GameRepository gameService)
        {
            _gameService = gameService;
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet]
        [SwaggerOperation(Summary = "Get all game")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _gameService.GetAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        //[Authorize(Roles = "Player, Admin")]
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get game by game id")]
        public async Task<ActionResult<Game>> GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameService.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not find this game");

        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPost]
        [SwaggerOperation(Summary = "Create new game")]
        public async Task<ActionResult> PostGame(GameRequest game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _gameService.CreateAsync(Int32.Parse(userId), game);

            return Ok(result);
        }

        //[Authorize(Roles = "Admin, Moderator")]
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing game")]
        public async Task<ActionResult> UpdateGame(int id, GameRequest game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _gameService.UpdateAsync(id, Int32.Parse(userId), game);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);

        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete an existing game")]
        public async Task<ActionResult> DeleteGame(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameService.DeleteAsync(id);
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }

}
