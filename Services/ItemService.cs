
using Microsoft.EntityFrameworkCore;
using System.Collections;

using MobileBasedCashFlowAPI.Common;
using MobileBasedCashFlowAPI.IServices;
using MobileBasedCashFlowAPI.Models;
using MobileBasedCashFlowAPI.DTO;

namespace MobileBasedCashFlowAPI.Services
{
    public class ItemService : IItemService
    {
        public const string SUCCESS = "success";
        private readonly MobileBasedCashFlowGameContext _context;

        public ItemService(MobileBasedCashFlowGameContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable> GetAsync()
        {
            var item = await (from i in _context.Items
                              select new
                              {
                                  i.ItemId,
                                  i.ItemName,
                                  i.ItemImageUrl,
                                  i.ItemPrice,
                                  i.Description,
                                  i.IsInShop,
                                  i.CreateBy,
                              }).ToListAsync();
            return item;
        }

        public async Task<Object?> GetAsync(string id)
        {
            var item = await _context.Items.Select(i => new
            {
                i.ItemId,
                i.ItemName,
                i.ItemImageUrl,
                i.ItemPrice,
                i.Description,
                i.IsInShop,
                i.CreateBy,
            }).Where(i => i.ItemId == id).FirstOrDefaultAsync();
            return item;
        }

        public async Task<string> CreateAsync(string userId, ItemRequest item)
        {

            try
            {
                var checkName = await _context.Items
                                .Where(d => d.ItemName == item.ItemName)
                                .Select(d => new { itemName = d.ItemName }).FirstOrDefaultAsync();
                if (checkName != null)
                {
                    return "This Item name is existed";
                }
                if (!ValidateInput.isNumber(item.ItemPrice.ToString()) || item.ItemPrice <= 0)
                {
                    return "Price must be mumber and bigger than 0";
                }

                var item1 = new Item()
                {
                    ItemId = Guid.NewGuid().ToString(),
                    ItemName = item.ItemName,
                    ItemImageUrl = item.ItemImageUrl,
                    ItemPrice = item.ItemPrice,
                    Description = item.Description,
                    IsInShop = item.IsInShop,
                    CreateAt = DateTime.Now,
                    CreateBy = userId,
                };

                _context.Items.Add(item1);
                await _context.SaveChangesAsync();
                return SUCCESS;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }

        }

        public async Task<string> UpdateAsync(string id, string userId, ItemRequest item)
        {
            var oldItem = await _context.Items.FirstOrDefaultAsync(i => id == i.ItemId);
            if (oldItem != null)
            {
                try
                {
                    var checkName = await _context.Items
                                .Where(d => d.ItemName == item.ItemName)
                                .Select(d => new { itemName = d.ItemName }).FirstOrDefaultAsync();
                    if (checkName != null)
                    {
                        return "This Item name is existed";
                    }
                    if (!ValidateInput.isNumber(item.ItemPrice.ToString()) || item.ItemPrice <= 0)
                    {
                        return "Price must be mumber and bigger than 0";
                    }

                    oldItem.ItemName = item.ItemName;
                    oldItem.ItemImageUrl = item.ItemImageUrl;
                    oldItem.ItemPrice = item.ItemPrice;
                    oldItem.Description = item.Description;
                    oldItem.IsInShop = item.IsInShop;
                    oldItem.CreateAt = oldItem.CreateAt;
                    oldItem.CreateBy = oldItem.CreateBy;
                    oldItem.UpdateAt = DateTime.Now;
                    oldItem.UpdateBy = userId;

                    await _context.SaveChangesAsync();
                    return SUCCESS;
                }
                catch (Exception ex)
                {
                    return ex.ToString();
                }
            }
            return "Can not find this item";
        }

        public async Task<string> DeleteAsync(string itemId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item == null)
            {
                return "Can not find this item";
            }
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return SUCCESS;
        }

    }
}
