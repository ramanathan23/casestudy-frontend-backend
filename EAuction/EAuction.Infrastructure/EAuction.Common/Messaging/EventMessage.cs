namespace EAuction.Infrastructure.Common
{
    public class EventMessage
    {
        public string SessionId { get; set; }
        public string MessageType { get; set; }

        public string Message { get; set; }
    }
}