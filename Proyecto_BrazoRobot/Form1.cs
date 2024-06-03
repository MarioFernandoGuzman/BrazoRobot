using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO.Ports;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;

namespace Proyecto_BrazoRobot
{
    public partial class Form1 : Form
    {
        private SerialPort arduinoPort;
        
        private int servo3LastPosition = 0;
       
        public Form1()
        {
            InitializeComponent();
            arduinoPort = new SerialPort("COM3", 9600); // Ajusta el nombre del puerto y la velocidad
            arduinoPort.Open();
            trackBar1.Scroll += new EventHandler(TrackBar_Scroll);
            trackBar2.Scroll += new EventHandler(TrackBar_Scroll);
            trackBar3.Scroll += new EventHandler(TrackBar_Scroll);

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            
        }
        private void SendServoPositions()
        {

            int servo1Position = trackBar3.Value;
            int servo2Position = trackBar2.Value;
            int servo3Position = servo3LastPosition; // Usa la última posición enviada para el tercer servo
            var message = new { servo1Position = servo1Position, servo2Position = servo2Position, servo3Position = servo3Position };
            string jsonString = JsonConvert.SerializeObject(message);
            arduinoPort.WriteLine(jsonString);
        }
       // Variable para almacenar la última posición del tercer servo

        private void SendServoPositionss(int position)
        {
            servo3LastPosition = position; // Actualiza la última posición
            SendServoPositions(); // Envía las posiciones de todos los servos
        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            SendServoPositions();
            lbl_Ang1.Text = trackBar2.Value.ToString();
            if(trackBar2.Value > 0)
            {
                lbl_Ang1.Text = (trackBar2.Value - 90).ToString();
            }
            else
            {
                lbl_Ang1.Text = (trackBar2.Value + 90).ToString();
                if (lbl_Ang1.Text == "90")
                    lbl_Ang1.Text = "-90";
            }
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            SendServoPositions();
            lbl_Ang2.Text = trackBar3.Value.ToString();
            if (trackBar3.Value > 0)
            {
                lbl_Ang2.Text = (trackBar3.Value - 90).ToString();
            }
            else
            {
                lbl_Ang2.Text = (trackBar3.Value + 90).ToString();
                if (lbl_Ang2.Text == "90")
                    lbl_Ang2.Text = "-90";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendServoPositionss(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendServoPositionss(90);
        }

        private void Calculos_S3()
        {
            int Dx = trackBar1.Value;
            double ang1 = trackBar2.Value;
            double ang2 = trackBar3.Value;

            ang1 = ang1* Math.PI / 180.0;
            ang2 = ang2 * Math.PI / 180.0;

            double ang3 = ang1 + ang2;
            int L1 = 20, L2 = 15;

            double x = Dx + L1 * Math.Cos(ang1) + L2 * Math.Cos(ang3);
            double y = L1 * Math.Sin(ang1) + L2 * Math.Sin(ang3);

            lbl_S3.Text = $"({x:N3}, {y:N3})";


        }

        private void Calculos_S2()
        {
            int Dx = trackBar1.Value;
            int L = 20;
            double ang = trackBar2.Value;

            ang = ang * Math.PI / 180.0;

            double x = Dx + L * Math.Cos(ang);
            double y = L * Math.Sin(trackBar2.Value + trackBar3.Value);

            lbl_S2.Text = $"({x:N3}, {y:N3})";
        }

        private void Calculos_S1()
        {

        }

        private void Calculos_S0()
        {
            lbl_S0.Text = "(0,0)";
        }

        private void TrackBar_Scroll(object sender, EventArgs e)
        {
            // Obtener el TrackBar que generó el evento
            TrackBar trackBar = sender as TrackBar;

            if (trackBar != null)
            {
                Calculos_S0();
                Calculos_S1();
                Calculos_S2();
                Calculos_S3();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lbl_L1.Text = trackBar1.Value.ToString();
            lbl_Ang1.Text = trackBar2.Value.ToString();
            lbl_Ang2.Text = trackBar3.Value.ToString();
            SendServoPositions();

            Calculos_S0();
            Calculos_S1();
            Calculos_S2();
            Calculos_S3();

        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            lbl_L1.Text = trackBar1.Value.ToString();
        }
    }
}
