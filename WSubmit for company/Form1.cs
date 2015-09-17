using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace WSubmit_for_company
{
    public partial class Form1 : Form
    {

        ManualResetEvent _shutdownEvent = new ManualResetEvent(false);
        ManualResetEvent _pauseEvent = new ManualResetEvent(true);
        //Thread _thread = null; 

        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String strfilename = openFileDialog.FileName;
                //MessageBox.Show(strfilename);
                file_read(strfilename);

            }
        }

        public void file_read(String filePath)
        {
            StreamReader sr = new StreamReader(filePath);
             
            string delim = " ,."; //maybe some more delimiters like ?! and so on
            string[] fields = null;
            string line = null;
            int ch = 0;
            
            int Symbol = 0, alpNumaric = 0, AlNu = 0, spases = 0, counter = 0;

             while(!sr.EndOfStream)
             {
                 _pauseEvent.WaitOne(Timeout.Infinite);
                 if (_shutdownEvent.WaitOne(0))
                 {
                     break;
                 }
                
                 line = sr.ReadLine();//each time you read a line you should split it into the words
                 line.Trim();
                 fields = line.Split(delim.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                 counter+=fields.Length; //and just add how many of them there is

                 ch += line.Count(char.IsLetter);
                 spases += line.Count(Char.IsWhiteSpace);
                 AlNu += line.Count(Char.IsNumber);
                 Symbol += line.Count(Char.IsSymbol);
                 alpNumaric += line.Count(char.IsLetterOrDigit);
               }

            // print value in labale
             label2.Text = counter.ToString();
             label4.Text = ch.ToString();
             label6.Text = spases.ToString();
             label8.Text = AlNu.ToString();
             label10.Text = Symbol.ToString();
             label12.Text = alpNumaric.ToString();
        }

        private void Pause_Click(object sender, EventArgs e)
        {
            _pauseEvent.Reset();
        }

        private void Resume_Click(object sender, EventArgs e)
        {
            _pauseEvent.Set();
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            // Signal the shutdown event
            _shutdownEvent.Set();

            // Make sure to resume any paused threads
            _pauseEvent.Set();

           
        }
    }

}
