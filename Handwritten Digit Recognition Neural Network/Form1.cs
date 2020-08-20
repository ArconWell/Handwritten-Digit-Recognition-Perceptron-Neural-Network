using System.Drawing;
using System.Windows.Forms;
using Backend;

namespace Handwritten_Digit_Recognition_Neural_Network
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Bitmap bmp = new Bitmap(@"F:\Рабочий стол\Новый точечный рисунок.bmp");
            pictureBox1.Image = bmp;
            NeuralNetwork.RecognizeDigit(bmp);
        }
    }
}
