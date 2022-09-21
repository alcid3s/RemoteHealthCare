namespace RemoteHealthCare.Bikes
{

    public delegate void OnUpdate();

    internal interface IBike
    {
        OnUpdate OnUpdate { get; set; }
        decimal ElapsedTime { get; }
        int DistanceTravelled { get; }
        decimal Speed { get; }
        int HeartRate { get; }
    }
}