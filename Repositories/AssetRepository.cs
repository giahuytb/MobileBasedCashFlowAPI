﻿
using Microsoft.EntityFrameworkCore;
using System.Collections;

using MobileBasedCashFlowAPI.Utils;
using MobileBasedCashFlowAPI.IRepositories;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.Dto;
using NuGet.ContentModel;

namespace MobileBasedCashFlowAPI.Repositories
{
    public class AssetRepository : IAssetRepository
    {
        private readonly MobileBasedCashFlowGameContext _context;

        public AssetRepository(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAllAsync()
        {
            var asset = await (from a in _context.Assets
                               select new
                               {
                                   a.AssetId,
                                   a.AssetName,
                                   a.ImageUrl,
                                   a.AssetPrice,
                                   a.Description,
                                   a.IsInShop,
                                   a.CreateBy,
                                   a.AssetTypeId,
                               }).AsNoTracking().ToListAsync();
            return asset;
        }

        public async Task<IEnumerable> GetAssetInShop(int userId)
        {
            var asset = await (from a in _context.Assets
                               where a.IsInShop == true && !_context.UserAssets
                                                                  .Where(ua => ua.UserId == userId)
                                                                  .Select(ua => ua.AssetId)
                                                                  .Contains(a.AssetId)
                               select new
                               {
                                   a.AssetId,
                                   a.AssetName,
                                   a.ImageUrl,
                                   a.AssetPrice,
                                   a.Description,
                                   a.IsInShop,
                                   a.CreateBy,
                                   a.AssetTypeId,
                               }).AsNoTracking().ToListAsync();
            return asset;
        }

        public async Task<Object?> GetByIdAsync(int id)
        {
            var asset = await _context.Assets.Select(a => new
            {
                a.AssetId,
                a.AssetName,
                a.ImageUrl,
                a.AssetPrice,
                a.Description,
                a.IsInShop,
                a.CreateBy,
                a.AssetTypeId,
            }).Where(a => a.AssetId == id).AsNoTracking().FirstOrDefaultAsync();
            return asset;
        }

        public async Task<string> CreateAsync(int userId, AssetRequest request)
        {
            var checkName = await _context.Assets
                            .Where(a => a.AssetName == request.AssetName)
                            .AsNoTracking()
                            .FirstOrDefaultAsync();
            if (checkName != null)
            {
                return "This asset name is existed";
            }

            var asset = new Models.Asset()
            {
                AssetName = request.AssetName,
                ImageUrl = request.ImageUrl,
                AssetPrice = request.AssetPrice,
                Description = request.Description,
                IsInShop = request.IsInShop,
                CreateAt = DateTime.Now,
                CreateBy = userId,
                AssetTypeId = request.AssetTypeId,
            };

            await _context.Assets.AddAsync(asset);
            await _context.SaveChangesAsync();
            return Constant.Success;
        }

        public async Task<string> UpdateAsync(int assetId, int userId, AssetRequest request)
        {
            var oldAsset = await _context.Assets.FirstOrDefaultAsync(a => a.AssetId == assetId);
            if (oldAsset != null)
            {
                // check if the new name is already exist in database. (except it's old name)
                var checkName = await _context.Assets
                                        .Where(a => a.AssetName == request.AssetName && a.AssetName != oldAsset.AssetName)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This asset name is existed";
                }
                oldAsset.AssetName = request.AssetName;
                oldAsset.ImageUrl = request.ImageUrl;
                oldAsset.AssetPrice = request.AssetPrice;
                oldAsset.Description = request.Description;
                oldAsset.IsInShop = request.IsInShop;
                oldAsset.CreateAt = oldAsset.CreateAt;
                oldAsset.CreateBy = oldAsset.CreateBy;
                oldAsset.UpdateAt = DateTime.Now;
                oldAsset.UpdateBy = userId;
                oldAsset.AssetTypeId = request.AssetTypeId;

                await _context.SaveChangesAsync();
                return Constant.Success;
            }
            return "Can not find this Asset";
        }

        public async Task<string> DeleteAsync(int AssetId)
        {
            var Asset = await _context.Assets.Where(a => a.AssetId == AssetId).FirstOrDefaultAsync();
            if (Asset == null)
            {
                return "Can not find this Asset";
            }
            _context.Assets.Remove(Asset);
            await _context.SaveChangesAsync();

            return Constant.Success;
        }

    }
}
