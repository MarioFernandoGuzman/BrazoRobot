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

namespace Proyecto_BrazoRobot
{
    public partial class Form1 : Form
    {
        private SerialPort arduinoPort;
        
        private int servo3LastPosition = 0;
       
        public Form1()
        {
            InitializeComponent();
            arduinoPort = new SerialPort("COM7", 9600); // Ajusta el nombre del puerto y la velocidad
            arduinoPort.Open();
            
            
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
        }

        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            SendServoPositions();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendServoPositionss(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SendServoPositionss(90);
        }
    }
}
