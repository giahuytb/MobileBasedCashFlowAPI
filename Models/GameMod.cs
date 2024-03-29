﻿using System;
using System.Collections.Generic;

namespace MobileBasedCashFlowAPI.Models
{
    public partial class GameMod
    {
        public GameMod()
        {
            GameMatches = new HashSet<GameMatch>();
        }

        public int GameModId { get; set; }
        public string ModName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public int CreateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? UpdateBy { get; set; }
        public bool Status { get; set; }
        public int? GameId { get; set; }

        public virtual Game? Game { get; set; }
        public virtual ICollection<GameMatch> GameMatches { get; set; }
    }
}
