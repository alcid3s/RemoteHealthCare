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
            //doctor.setTXTSpeed("15");
            //doctor.setTXTDT("200");
            //doctor.setTXTET("20.23");
            //doctor.setTXTHR("125");
            //doctor.addListItems("Niels");
            //doctor.addListItems("Koen");
            //doctor.addListItems("Coen");
            //doctor.addListItems("Guus");
            //doctor.addListItems("Max");
            //doctor.addListItems("Tygo");
            //doctor.addListItems("Simulation Bike");

            //DoctorClient.clientData.

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            //ApplicationConfiguration.Initialize();
            Application.Run(doctor);

            for (; ; );
        }
    }
}