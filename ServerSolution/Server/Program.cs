namespace Server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Server server = new(1337);
            server.Run();
        }
    }
}