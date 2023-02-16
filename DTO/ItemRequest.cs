﻿namespace MobieBasedCashFlowAPI.ViewModels
{
    public class itemRequest
    {
        public string ItemName { get; set; } = null!;
        public string ItemImageUrl { get; set; } = null!;
        public double ItemPrice { get; set; }
        public string Description { get; set; } = null!;
        public bool IsInShop { get; set; }
    }
}
