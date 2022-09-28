namespace WinFormsApp2

{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       
        int i = 0;

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox5.Text = "hallo Niels";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (i % 2 == 0)
            {
                textBox4.Text = "Guus";
                i++;
            }
            else
            {
                textBox4.Text = "Niet Niels";
                i++;
            }
        }
    }
}