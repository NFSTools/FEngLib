namespace FEngLib.Data
{
    public class FEKeyNode
    {
        public int Time { get; set; }

        public uint[] Val { get; set; }

        public FEKeyNode()
        {
            Val = new uint[4];
        }
    }
}