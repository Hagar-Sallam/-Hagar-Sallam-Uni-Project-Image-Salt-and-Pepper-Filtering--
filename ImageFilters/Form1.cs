using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZGraphTools;

namespace ImageFilters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[,] ImageMatrix;
        string OpenedFilePath;
        private PictureBox pictureBox2;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                 OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);

            }
        }
 
        private void btnZGraph_Click(object sender, EventArgs e)
        {
            // Make up some data points from the N, N log(N) functions
            int N = 40;
            double[] x_values = new double[N];
            double[] y_values_N = new double[N];
            double[] y_values_NLogN = new double[N];
                        
            for (int i = 0; i < N; i++)
            {
                x_values[i] = i;
                y_values_N[i] = i;
                y_values_NLogN[i] = i * Math.Log(i);
            }

            //Create a graph and add two curves to it
            
             ZGraphForm ZGF = new ZGraphForm("Sample Graph", "N", "f(N)");
            ZGF.add_curve("f(N) = N", x_values, y_values_N, Color.Red);
            ZGF.add_curve("f(N) = N Log(N)", x_values, y_values_NLogN, Color.Blue);
             ZGF.Show();
            


        }

               private void button1_Click(object sender, EventArgs e)
        {
            int window = int.Parse(textBox1.Text);
            int Trim = int.Parse(textBox2.Text);
          
            string Sort = comboBox1.Text;
            int sorting=0;
            if (Sort == "Counting Sort") sorting = 1;
            if (Sort == "Kth Algorithm") sorting = 2;

          
         
            ImageOperations.Filter1(ImageMatrix, window, Trim,sorting);
          

            ImageOperations.DisplayImage(ImageMatrix, pictureBox3);



        }

        private void button2_Click(object sender, EventArgs e)
        {
            int window = int.Parse(textBox3.Text);
            string Sort = comboBox2.Text;
            int sorting = 0;
            if (Sort == "Quick Sort") sorting = 1;
            if (Sort == "Counting Sort") sorting = 2;
         

            ImageOperations.Filter2(ImageMatrix, 3, window,sorting);
            
            ImageOperations.DisplayImage(ImageMatrix, pictureBox3);
 
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            int window = int.Parse(textBox4.Text);

            int cnt = 0;
            byte[,] temp = ImageMatrix;

            int count = 0;
            int i = 3;
            int siz = (window - i) / 2;
            double[] x = new double[siz + 1];
            double[] y_quick = new double[siz + 1];
            double[] y_counting = new double[siz + 1];
            while (count <= siz)
            {
                x[count] = i;
                int time_milli;
                int time_second;
                int begin, end;
                begin = System.Environment.TickCount;
                ImageOperations.Filter2(temp, 3, i, 1);
                end = System.Environment.TickCount;
                time_milli = end - begin;
                //time_second = time_milli / 1000;
                y_quick[count] = time_milli;

                temp = ImageMatrix;
                begin = System.Environment.TickCount;
                ImageOperations.Filter2(temp, 3, i, 2);
                end = System.Environment.TickCount;
                time_milli = end - begin;
                //time_second = time_milli / 1000;
                y_counting[count] = time_milli;
                count++;
                i += 2;
            }
            ZGraphForm ZGF2 = new ZGraphForm("timing Graph of adaptive", "window size", "execution time");
            ZGF2.add_curve("counting sort", x, y_counting, Color.Red);
            ZGF2.add_curve("quick sort", x, y_quick, Color.Blue);
            ZGF2.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int window = int.Parse(textBox4.Text);

            byte[,] temp = ImageMatrix;

            int cnt = 0;
            int i = 3;
            int siz = (window - i) / 2;
            double[] x_alpha = new double[siz + 1];
            double[] y_counting_alpha = new double[siz + 1];
            double[] y_kth_alpha = new double[siz + 1];
            while (cnt <= siz)
            {
                x_alpha[cnt] = i;
                int time_milli;
                int begin, end;
                begin = System.Environment.TickCount;
                ImageOperations.Filter1(temp, i, 1, 1);
                end = System.Environment.TickCount;
                time_milli = end - begin;
                y_counting_alpha[cnt] = time_milli;

                temp = ImageMatrix;
                begin = System.Environment.TickCount;
                ImageOperations.Filter1(temp, i, 1, 2);
                end = System.Environment.TickCount;
                time_milli = end - begin;

                y_kth_alpha[cnt] = time_milli;
                cnt++;
                i += 2;

            }
            ZGraphForm ZGF3 = new ZGraphForm("timing Graph of alpha trim", "window size", "execution time");
            ZGF3.add_curve("counting sort", x_alpha, y_counting_alpha, Color.Red);
            ZGF3.add_curve("kth algorithmm", x_alpha, y_kth_alpha, Color.Blue);
            ZGF3.Show();

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }
    }
}