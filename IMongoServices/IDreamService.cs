﻿
using MobileBasedCashFlowAPI.MongoDTO;
using MobileBasedCashFlowAPI.MongoModels;
using System.Collections;

namespace MobileBasedCashFlowAPI.IMongoServices
{
    public interface IDreamService
    {
        public Task<IEnumerable> GetAsync();
        public Task<Object?> GetAsync(int pageIndex, int pageSize);
        public Task<Dream?> GetAsync(string id);
        public Task<string> CreateAsync(DreamRequest request);
        public Task<string> UpdateAsync(string id, DreamRequest request);
        public Task<string> RemoveAsync(string id);
    }
}
