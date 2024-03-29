﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Collections;

using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Dto;



namespace MobileBasedCashFlowAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ISendMailRepository _sendMailService;
        private readonly MobileBasedCashFlowGameContext _context;

        public UserRepository(MobileBasedCashFlowGameContext context,
                           IConfiguration configuration,
                           ISendMailRepository sendMailService)
        {
            _configuration = configuration;
            _sendMailService = sendMailService;
            _context = context;
        }

        public async Task<object> Authenticate(LoginRequest request)
        {
            var user = await _context.UserAccounts.SingleOrDefaultAsync(x => x.UserName == request.UserName);
            if (user == null)
            {
                return Constant.NotFound;
            }
            bool isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

            if (!isValidPassword)
            {
                return Constant.WrongPassword;
            }
            // find role name by id
            var role = await _context.UserRoles
                                .Where(r => r.RoleId == user.RoleId)
                                .Select(r => new { roleName = r.RoleName }).FirstOrDefaultAsync();
            if (role == null)
            {
                return "Can not found this role";
            }
            else if (user.Address == null)
            {
                user.Address = "";
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Role , role.roleName),
                new Claim("Id" , user.UserId.ToString()),
                new Claim("Status" , user.Status.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: creds);
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);

            // get last character is used by player
            var userAsset = await (from usAs in _context.UserAssets
                                   join Ast in _context.Assets on usAs.AssetId equals Ast.AssetId
                                   where Ast.AssetId == Ast.AssetId
                                      && usAs.UserId == user.UserId
                                      && usAs.LastUsed == (from NestedUsAs in _context.UserAssets
                                                           join Ast in _context.Assets on NestedUsAs.AssetId equals Ast.AssetId
                                                           where Ast.AssetTypeId == 2 && NestedUsAs.UserId == user.UserId
                                                           orderby NestedUsAs.LastUsed descending
                                                           select NestedUsAs.LastUsed
                                                            ).SingleOrDefault()
                                   select new
                                   {
                                       Ast.ImageUrl,
                                   }).AsNoTracking().SingleOrDefaultAsync();
            return new
            {
                user = new
                {
                    user.UserId,
                    user.NickName,
                    user.Email,
                    user.Address,
                    user.Coin,
                    user.Point,
                    user.ImageUrl,
                    user.Phone,
                    user.Gender,
                    role.roleName,
                    user.LastJobSelected,
                    lastCharacterSelected = userAsset != null ? userAsset.ImageUrl : null,
                },
                token = stringToken,
            };

        }

        public async Task<string> Register(RegisterRequest request)
        {
            var user = new UserAccount()
            {
                UserName = request.UserName,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Gender = "Nam",
                Email = request.Email,
                Coin = 100,
                Point = 0,
                CreateAt = DateTime.Now,
                RoleId = null,
                Status = true,
            };

            var checkUser = await _context.UserAccounts.FirstOrDefaultAsync(u => u.UserName == request.UserName);
            var checkMail = await _context.UserAccounts.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (checkUser != null)
            {
                return "This username existed";
            }
            else if (checkMail != null)
            {
                return "This email has used already";
            }
            else
            {
                // Find role "Player" in database
                var roleId = await (from role in _context.UserRoles
                                    where role.RoleName == "Player"
                                    select new { roleId = role.RoleId }).AsNoTracking().FirstOrDefaultAsync();
                // Find gameId in database that match version == "ver_1"
                var gameServerId = await (from game in _context.GameServers
                                          where game.GameVersion == "Ver_1"
                                          select new { gameServerId = game.GameServerId }).AsNoTracking().FirstOrDefaultAsync();

                if (roleId != null && gameServerId != null)
                {
                    // Add role to user
                    user.GameServerId = gameServerId.gameServerId;
                    user.RoleId = roleId.roleId;
                    await _context.UserAccounts.AddAsync(user);
                    await _context.SaveChangesAsync();

                    int userId = user.UserId;

                    var asset = await _context.Assets.Where(i => i.AssetName == "Default").FirstOrDefaultAsync();
                    if (asset == null)
                    {
                        return "Can not found this asset";
                    }
                    // check if this asset has been purchased by the user 
                    var check = await _context.UserAssets.FirstOrDefaultAsync(i => i.UserId == userId && i.AssetId == asset.AssetId);
                    if (check != null)
                    {
                        return "You already bought this asset";
                    }
                    if (user.Coin < asset.AssetPrice)
                    {
                        return "You don't have enough coin to buy this asset";
                    }
                    var invent = new UserAsset()
                    {
                        AssetId = asset.AssetId,
                        UserId = userId,
                        CreateAt = DateTime.Now,
                        LastUsed = DateTime.Now,
                    };

                    user.Coin = user.Coin - asset.AssetPrice;
                    await _context.UserAssets.AddAsync(invent);
                    await _context.SaveChangesAsync();

                    return Constant.Success;
                }
                return Constant.Failed;
            }
        }


        public async Task<IEnumerable> GetAllAsync()
        {

            var users = await (from user in _context.UserAccounts
                               join role in _context.UserRoles on user.RoleId equals role.RoleId
                               where user.RoleId != 1
                               select new
                               {
                                   user.UserId,
                                   user.UserName,
                                   user.NickName,
                                   user.Gender,
                                   user.Email,
                                   user.Coin,
                                   user.Point,
                                   role.RoleName,
                               }).AsNoTracking().ToListAsync();
            return users;
        }


        public async Task<string> EditProfile(int userId, EditProfileRequest request)
        {
            var oldProfile = await _context.UserAccounts.FirstOrDefaultAsync(i => i.UserId == userId);
            if (oldProfile != null)
            {
                var checkNickName = await _context.UserAccounts.
                                Where(u => u.NickName == request.NickName && u.NickName != oldProfile.NickName)
                                .FirstOrDefaultAsync();
                if (checkNickName != null)
                {
                    return "This name is already use by other user, please use other name";
                }
                oldProfile.NickName = request.NickName;
                oldProfile.Gender = request.Gender;
                oldProfile.Phone = request.Phone;
                oldProfile.Email = request.Email;
                oldProfile.ImageUrl = request.ImageUrl;
                oldProfile.UpdateAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return Constant.Success;

            }
            return Constant.Failed;
        }

        public async Task<object?> ViewProfile(int userId)
        {
            var user = await (from u in _context.UserAccounts
                              where u.UserId == userId
                              select new
                              {
                                  u.NickName,
                                  u.Gender,
                                  u.Phone,
                                  u.Email,
                                  u.ImageUrl,
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<string> UpdateCoin(int userId, EditRequest request)
        {
            var oldProfile = await _context.UserAccounts.FirstOrDefaultAsync(i => i.UserId == userId);
            if (oldProfile != null)
            {
                oldProfile.Coin = request.Coin;
                oldProfile.Point = request.Point;
                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return Constant.Failed;
        }

        public async Task<object?> FindUserById(int userId)
        {
            var user = await (from u in _context.UserAccounts
                              where u.UserId == userId
                              select new
                              {
                                  u.NickName,
                                  u.Gender,
                                  u.ImageUrl,
                              }).FirstOrDefaultAsync();
            return user;
        }

        public async Task<object?> GetUserAsset(int userId)
        {
            var inventory = await (from userAsset in _context.UserAssets
                                   join user in _context.UserAccounts on userAsset.UserId equals user.UserId
                                   join asset in _context.Assets on userAsset.AssetId equals asset.AssetId
                                   join assetType in _context.AssetTypes on asset.AssetTypeId equals assetType.AssetTypeId
                                   where user.UserId == userId
                                   select new
                                   {
                                       asset.AssetId,
                                       user.UserName,
                                       asset.AssetName,
                                       assetType.AssetTypeName,
                                       userAsset.CreateAt,
                                       userAsset.LastUsed,
                                   }).AsNoTracking().ToListAsync();
            return inventory;
        }

        public async Task<string> BuyAsset(int assetId, int userId)
        {
            var asset = await _context.Assets.Where(i => i.AssetId == assetId).FirstOrDefaultAsync();
            if (asset == null)
            {
                return "Can not found this asset";
            }
            // check if this asset has been purchased by the user 
            var check = await _context.UserAssets.FirstOrDefaultAsync(i => i.UserId == userId && i.AssetId == assetId);
            if (check != null)
            {
                return "You already bought this asset";
            }
            var user = await _context.UserAccounts.Where(u => u.UserId == userId).FirstOrDefaultAsync();
            if (user == null)
            {
                return "Can not found this user";
            }
            if (user.Coin < asset.AssetPrice)
            {
                return "You don't have enough coin to buy this asset";
            }
            var invent = new UserAsset()
            {
                AssetId = assetId,
                UserId = userId,
                CreateAt = DateTime.Now,
            };

            user.Coin = user.Coin - asset.AssetPrice;
            await _context.UserAssets.AddAsync(invent);
            await _context.SaveChangesAsync();
            return Constant.Success;
        }

        public async Task<string> UpdateLastUsed(int userId, LastUsedRequest request)
        {
            var userAsset = await _context.UserAssets
                            .Where(i => i.UserId == userId && i.AssetId == request.AssetId)
                            .FirstOrDefaultAsync();
            var user = await _context.UserAccounts.FindAsync(userId);
            if (user == null)
            {
                return "Can not found this user";
            }
            if (userAsset == null)
            {
                return "Can not found this asset on your inventory";
            }

            if (userAsset != null)
            {
                userAsset.LastUsed = DateTime.Now;
                user.LastJobSelected = request.LastJobSelected;
                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return Constant.NotFound;
        }

        public async Task<string> DeleteMyAsset(int assetId, int userId)
        {
            var myAsset = await _context.UserAssets.Where(a => a.AssetId == assetId && a.UserId == userId).FirstOrDefaultAsync();
            if (myAsset != null)
            {
                _context.UserAssets.Remove(myAsset);
                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return Constant.NotFound;
        }
    }
}
