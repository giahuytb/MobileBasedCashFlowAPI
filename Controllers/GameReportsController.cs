﻿
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.Dto;
using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Models;
using System.Collections;

using System.Security.Claims;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.EntityFrameworkCore;

namespace MobileBasedCashFlowAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameReportsController : ControllerBase
    {
        private readonly IGameReportRepository _gameReportService;
        private readonly MobileBasedCashFlowGameContext _context;

        public GameReportsController(IGameReportRepository gameReportService, MobileBasedCashFlowGameContext context)
        {
            _gameReportService = gameReportService;
            _context = context;
        }

        //[HttpGet("test")]
        //[SwaggerOperation(Summary = "Get all game report")]
        //public async Task<ActionResult<IEnumerable>> Get()
        //{
        //    var result = await _context.GameReports.ToListAsync();
        //    if (result != null)
        //    {
        //        return Ok(result);
        //    }
        //    return NotFound("List is empty");
        //}

        [HttpGet("all")]
        [SwaggerOperation(Summary = "Get all game report")]
        public async Task<ActionResult<IEnumerable>> GetAll()
        {
            var result = await _gameReportService.GetAllAsync();
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpGet("my-report/{id}")]
        [SwaggerOperation(Summary = "Get game report by user id")]
        public async Task<ActionResult<GameReport>> MyReport(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameReportService.MyReport(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("List is empty");
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create new game report")]
        public async Task<ActionResult> PostGameReport(GameReportRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // get the current user logging in system
            string userId = HttpContext.User.FindFirstValue("Id");
            if (userId == null)
            {
                return Unauthorized("User id not Found, please login");
            }
            var result = await _gameReportService.CreateAsync(int.Parse(userId), request);
            return Ok(result);
        }

        [HttpDelete("delete-all")]
        [SwaggerOperation(Summary = "Delete all record in game report table")]
        public async Task<ActionResult> DeleteAllReport()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _gameReportService.DeleteAllRecord();
            if (result.Equals(Constant.Success))
            {
                return Ok(result);
            }
            return BadRequest(result);

        }
    }
}
