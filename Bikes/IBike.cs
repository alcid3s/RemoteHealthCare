namespace RemoteHealthCare.Bikes
{
    public delegate void OnUpdate();
    internal interface IBike
    {
        bool IsRunning { get; set; }
        OnUpdate OnUpdate { get; set; }
        decimal ElapsedTime { get; }
        int DistanceTravelled { get; }
        decimal Speed { get; }
        int HeartRate { get; }

        void Init();
        void Stop();

        void SetResistance(byte resistance);
    }
}