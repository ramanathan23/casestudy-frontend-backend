using System;

namespace EAuction.Core.Common.Exceptions
{
    public class EAuctionDomainException : Exception
    {
        public EAuctionDomainException(string message) : base(message)
        {
        }
    }
}