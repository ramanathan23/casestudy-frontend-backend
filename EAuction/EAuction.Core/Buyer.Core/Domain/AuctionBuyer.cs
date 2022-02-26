﻿using EAuction.Persistence.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EAuction.Buyer.Core.Domain
{
    public class AuctionBuyer : IEntity<string>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pin { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
}