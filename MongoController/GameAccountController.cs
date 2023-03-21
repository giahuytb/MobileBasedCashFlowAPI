﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobileBasedCashFlowAPI.IMongoServices;
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.MongoController
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameAccountsController : ControllerBase
    {
        private readonly IGameAccountService _gameAccountService;

        public GameAccountsController(IGameAccountService gameAccountService)
        {
            _gameAccountService = gameAccountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable>> GetALl()
        {
            try
            {
                var result = await _gameAccountService.GetAsync();
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

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<GameAccount>> GetGameAccountById(string id)
        {
            var result = await _gameAccountService.GetAsync(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("Can not found this game account");
        }

        [HttpPost]
        public async Task<ActionResult> CreateGameAccount(AccountRequest request)
        {
            try
            {
                var result = await _gameAccountService.CreateAsync(request);
                if (result == "success")
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult<List<GameAccount>>> UpdateGameAccount(string id, AccountRequest request)
        {
            try
            {
                var result = await _gameAccountService.GetAsync(id);
                if (result is null)
                {
                    return NotFound(result);
                }
                await _gameAccountService.UpdateAsync(id, request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult<List<GameAccount>>> DeleteGameAccount(string id)
        {
            try
            {
                var result = await _gameAccountService.GetAsync(id);
                if (result is null)
                {
                    return NotFound(result);
                }
                await _gameAccountService.RemoveAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}