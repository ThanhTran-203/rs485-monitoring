using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
namespace ComPort1
{
    public partial class Form1 : Form
    {
        string dataOut;
        string dataIn;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            cbComPort.Items.AddRange(ports);
            btConnect.Enabled = true;
            btExit.Enabled = false;
            btStop.Enabled = false;
            cbAlwaysUpdate.Enabled = true;
            cbAddToOldData.Enabled = true;
        }

        private void btConnect_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = cbComPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(cbBaudRate.Text);
                serialPort1.DataBits = Convert.ToInt32(cbDataBits.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), cbParity.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBit.Text);

                serialPort1.Open();
                progressBar1.Value = 100;
                btConnect.Enabled = false;
                btExit.Enabled = true;
                btStop.Enabled = true;
                lbStatus.Text = "ON";
            }

            catch (Exception err)
            {
                MessageBox.Show(err.Message,"Error", MessageBoxButtons.OK, MessageBoxIcon.Error); ;   
                 
            }
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Close();
                progressBar1.Value = 0;
            }
            btConnect.Enabled = true;
            btExit.Enabled = false;
            btStop.Enabled = false;
            lbStatus.Text = "OFF";

        }

        private void btSend_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                dataOut = tSendData.Text; 
                serialPort1.WriteLine(dataOut);

            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
            this.Hide();
        }

        private void btDelete_Click(object sender, EventArgs e)
        {
            if(tBoxDataOut.Text != "")
            {
                tBoxDataOut.Text = "";
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            dataIn = serialPort1.ReadExisting();
            this.Invoke(new EventHandler(ShowData));
        }
        private void ShowData(object sender, EventArgs e)
        {
            if(cbAlwaysUpdate.Checked)
            {
                tReceivedData.Text = dataIn;
            }
            else if(cbAddToOldData.Checked)
            {
                tReceivedData.Text += dataIn;
            }
        }

        private void cbAlwaysUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if(cbAlwaysUpdate.Checked)
            {
                cbAlwaysUpdate.Checked = true;
                cbAddToOldData.Checked = false;
            }
        }

        private void cbAddToOldData_CheckedChanged(object sender, EventArgs e)
        {
            if(cbAddToOldData.Checked)
            {
                cbAddToOldData.Checked = true;
                cbAlwaysUpdate.Checked = false; 
            }
        }
    }
}
