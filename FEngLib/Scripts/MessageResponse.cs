using System.Collections.Generic;

namespace FEngLib.Scripts;

public class MessageResponse
{
    public uint Id { get; set; }
    public List<Response> Responses { get; set; }

    public MessageResponse()
    {
        Responses = new List<Response>();
    }
}