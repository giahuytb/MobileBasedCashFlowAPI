﻿
using MobileBasedCashFlowAPI.Dto;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MobileBasedCashFlowAPI.MongoModels
{
    public class JobCard
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? id { get; set; }
        public string Job_card_name { get; set; } = string.Empty;
        public int Children_cost { get; set; } = 0;
        public string Image_url { get; set; } = string.Empty;
        public bool Status { get; set; }
        public List<GameAccountRequest> Game_accounts { get; set; } = new List<GameAccountRequest>();

    }
}
