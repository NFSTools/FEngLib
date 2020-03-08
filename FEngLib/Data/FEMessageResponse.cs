using System.Collections.Generic;

namespace FEngLib.Data
{
    public class FEMessageResponse
    {
        public uint Id { get; set; }
        public List<FEResponse> Responses { get; set; }

        public FEMessageResponse()
        {
            Responses = new List<FEResponse>();
        }
    }
}