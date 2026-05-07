using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageFilters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //(hide everything except Open)
            label1.Visible = false;
            label2.Visible = false;
            radioButton1.Visible = false;
            radioButton2.Visible = false;

            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
            button4.Visible = false;
            button5.Visible = false;
        }

        byte[,] ImageMatrix;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
                // Hide open button
                btnOpen.Visible = false;

                // Show filter options
                label1.Visible = true;
                radioButton1.Visible = true;
                radioButton2.Visible = true;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                label2.Visible = true;
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;

                button4.Visible = false;
                button5.Visible = false;
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                label2.Visible = true;
                button4.Visible = true;
                button5.Visible = true;

                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 1. Make sure an image has been opened first
            if (ImageMatrix == null)
            {
                MessageBox.Show("Please open an image first!");
                return;
            }

            // 2. Window Size (Set to 3 for now since there is no input box in the UI yet)
            int windowSize = 7;

            // 3. Start the timer (Required for your project graph)
            int startTime = System.Environment.TickCount;

            // 4. Run the Kth Filter algorithm
            byte[,] filteredImage = ImageOperations.KthFilter(ImageMatrix, windowSize);

            // 5. Stop the timer and calculate the total time
            int executionTime = System.Environment.TickCount - startTime;

            // 6. Display the result
            // Note: Change 'pictureBox2' to 'pictureBox1' if you want it to overwrite the original image, 
            // or keep it as 'pictureBox2' if the blank square on the right side of your UI is named pictureBox2.
            ImageOperations.DisplayImage(filteredImage, pictureBox2);

            // 7. Show a popup with the execution time
            MessageBox.Show("Kth Filter Applied!\nExecution Time: " + executionTime + " ms");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Restarts the entire application
            Application.Restart();
        }
    }
}