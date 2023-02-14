﻿//using MobieBasedCashFlowAPI.SubModels;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MobieBasedCashFlowAPI.MongoModels
{
    public class JobCardMg
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Children_cost { get; set; } = 0;
        public  List<GameAccountMg> Game_accounts { get; set; } = new List<GameAccountMg>();

    }
}
