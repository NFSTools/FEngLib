using System.Collections.Generic;

namespace FEngLib.Messaging;

public interface IHaveMessageResponses
{
    List<MessageResponse> MessageResponses { get; }
}