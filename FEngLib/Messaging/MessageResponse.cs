using System.Collections.Generic;

namespace FEngLib.Messaging;

public class MessageResponse
{
    public MessageResponse()
    {
        Responses = new List<Response>();
    }

    public uint Id { get; set; }
    public List<Response> Responses { get; set; }
}