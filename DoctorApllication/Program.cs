namespace DoctorApllication
{
    internal static class Program
    {
        public static DoctorLogin doctor;

        public static void showDoctorLogin() 
        { 
            doctor.Show();
        }
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
             doctor = new DoctorLogin();

            DoctorClient doctorClient = new DoctorClient("127.0.0.1", 1337);
            doctorClient.Connect();

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            Application.Run(doctor);

            for (; ; );
        }
    }
}