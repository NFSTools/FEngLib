using System.Collections.Generic;

namespace FEngLib.Messaging;

public class MessageResponse
{
    public MessageResponse()
    {
        Responses = new List<ResponseCommand>();
    }

    public MessageResponse(uint id, List<ResponseCommand> responses)
    {
        Id = id;
        Responses = responses;
    }

    public uint Id { get; set; }
    public List<ResponseCommand> Responses { get; set; }
}