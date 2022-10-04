using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RemoteHealthCare.GUI
{
    public partial class ClientScreen : Form
    {
        public ClientScreen()
        {
            InitializeComponent();
        }

        private void txtSpeed_TextChanged(object sender, EventArgs e)
        {

        }

        public void setTxtSpeed(decimal s)
        {
            Invoke(new Action(() => txtSpeed.Text = s.ToString("f2")));
        }

        private void txtElapsedTime_TextChanged(object sender, EventArgs e)
        {

        }
        public void setTxtElapsedTime(decimal s)
        {
            Invoke(new Action(() => txtElapsedTime.Text = s.ToString("F2")));
        }

        private void txtDistanceTravelled_TextChanged(object sender, EventArgs e)
        {

        }

        public void setTxtDistanceTravelled(decimal s)
        { 
            Invoke(new Action(() => txtDistanceTravelled.Text = s.ToString("F0")));    
        }

        private void txtHeartRate_TextChanged(object sender, EventArgs e)
        {

        }
        
        public void setTxtHeartRate(decimal s)
        {
            Invoke(new Action(() => txtHeartRate.Text = s.ToString("F0")));
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
