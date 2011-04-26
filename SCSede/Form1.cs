using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SCSede
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stream stream = File.Open("Stages.bin", FileMode.Create);
            BinaryFormatter binary = new BinaryFormatter();

            binary.Serialize(stream, stageManager);
            stream.Close();
        }
    }
}
