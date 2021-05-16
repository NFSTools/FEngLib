namespace FEngLib.Scripts
{
    public class TrackNode
    {
        public TrackNode()
        {
            Val = default;
        }

        public int Time { get; set; }

        public object Val { get; set; }

        public T GetValue<T>() => (T) Val;
    }
}