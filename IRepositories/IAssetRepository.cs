﻿using MobileBasedCashFlowAPI.Dto;
using System.Collections;

namespace MobileBasedCashFlowAPI.IRepositories
{
    public interface IAssetRepository
    {
        public Task<IEnumerable> GetAllAsync();
        public Task<IEnumerable> GetAssetInShop(int userId);
        public Task<object?> GetByIdAsync(int assetId);
        public Task<string> CreateAsync(int userId, AssetRequest request);
        public Task<string> UpdateAsync(int assetId, int userId, AssetRequest request);
        public Task<string> DeleteAsync(int assetId);
    }
}
