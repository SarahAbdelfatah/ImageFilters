using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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

            groupBox1.Visible = false;
            groupBox2.Visible = false;
            groupBox3.Visible = false;
            groupBox4.Visible = false; // Add window size group hide

            radioButton1.Visible = false;
            radioButton2.Visible = false;
            radioButton6.Visible = false;

            // Default window size 3x3 checked
            radioButton3.Checked = true;
        }

        byte[,] ImageMatrix;

        /// <summary>
        /// Returns the window size based on which radio button is selected in groupBox4.
        /// radioButton3 = 3, radioButton4 = 5, radioButton5 = 7
        /// </summary>
        private int GetSelectedWindowSize()
        {
            if (radioButton3.Checked) return 3;
            if (radioButton4.Checked) return 5;
            if (radioButton5.Checked) return 7;
            // Default fallback
            return 3;
        }

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

                // Show window size group and filter options
                groupBox1.Visible = true;
                groupBox4.Visible = true; // Show window size selection
                radioButton1.Visible = true;
                radioButton2.Visible = true;
                radioButton6.Visible = true;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                groupBox3.Visible = false;
                groupBox2.Visible = true;
                groupBox2.BringToFront();
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                groupBox2.Visible = false;
                groupBox3.Visible = true;
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton6.Checked)
            {
                groupBox2.Visible = false;
                groupBox3.Visible = false;

                if (ImageMatrix == null)
                {
                    MessageBox.Show("Please open an image first!");
                    return;
                }

                int windowSize = GetSelectedWindowSize();
                int startTime = System.Environment.TickCount;

                byte[,] filteredImage = ImageOperations.ApplyMedianFilter_SlidingWindow(ImageMatrix, windowSize);

                int executionTime = System.Environment.TickCount - startTime;

                ImageOperations.DisplayImage(filteredImage, pictureBox2);

                MessageBox.Show("Heuristic Method Applied!\nWindow Size: " + windowSize + "x" + windowSize + "\nExecution Time: " + executionTime + " ms");
                
                // Uncheck the radio button to allow running it again
                radioButton6.Checked = false;
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

            // 2. Get window size from the selected radio button
            int windowSize = GetSelectedWindowSize();

            // 3. Start the timer (Required for your project graph)
            int startTime = System.Environment.TickCount;

            // 4. Run the Kth Filter algorithm
            byte[,] filteredImage = ImageOperations.KthFilter(ImageMatrix, windowSize);

            // 5. Stop the timer and calculate the total time
            int executionTime = System.Environment.TickCount - startTime;

            // 6. Display the result
            ImageOperations.DisplayImage(filteredImage, pictureBox2);

            // 7. Show a popup with the execution time
            MessageBox.Show("Kth Filter Applied!\nWindow Size: " + windowSize + "x" + windowSize + "\nExecution Time: " + executionTime + " ms");
        }


        private void button4_Click(object sender, EventArgs e)
        {
            // Non-Efficient Midpoint Filter
            if (ImageMatrix == null)
            {
                MessageBox.Show("Please open an image first!");
                return;
            }

            int windowSize = GetSelectedWindowSize();

            int startTime = System.Environment.TickCount;

            byte[,] filteredImage = ImageOperations.ApplyMidpointNonEfficient(ImageMatrix, windowSize);

            int executionTime = System.Environment.TickCount - startTime;

            ImageOperations.DisplayImage(filteredImage, pictureBox2);

            MessageBox.Show("Midpoint Filter (Non-Efficient) Applied!\nWindow Size: " + windowSize + "x" + windowSize + "\nExecution Time: " + executionTime + " ms");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // Efficient Midpoint Filter
            if (ImageMatrix == null)
            {
                MessageBox.Show("Please open an image first!");
                return;
            }

            int windowSize = GetSelectedWindowSize();

            int startTime = System.Environment.TickCount;

            byte[,] filteredImage = ImageOperations.ApplyMidpointEfficient(ImageMatrix, windowSize);

            int executionTime = System.Environment.TickCount - startTime;

            ImageOperations.DisplayImage(filteredImage, pictureBox2);

            MessageBox.Show("Midpoint Filter (Efficient) Applied!\nWindow Size: " + windowSize + "x" + windowSize + "\nExecution Time: " + executionTime + " ms");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Restarts the entire application
            Application.Restart();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (ImageMatrix == null)
            {
                MessageBox.Show("Please open an image first!");
                return;
            }

            int windowSize = GetSelectedWindowSize();
            int startTime = System.Environment.TickCount;

            byte[,] filteredImage = ImageOperations.ApplyMedianFilter_CountingSort(ImageMatrix, windowSize);

            int executionTime = System.Environment.TickCount - startTime;

            ImageOperations.DisplayImage(filteredImage, pictureBox2);

            MessageBox.Show("Median Filter (Counting Sort) Applied!\nWindow Size: " + windowSize + "x" + windowSize + "\nExecution Time: " + executionTime + " ms");
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            if (ImageMatrix == null)
            {
                MessageBox.Show("Please open an image first!");
                return;
            }

            int windowSize = GetSelectedWindowSize();
            int startTime = System.Environment.TickCount;

            byte[,] filteredImage = ImageOperations.MedianQuickSortFilter(ImageMatrix, windowSize);

            int executionTime = System.Environment.TickCount - startTime;

            ImageOperations.DisplayImage(filteredImage, pictureBox2);

            MessageBox.Show("Median Filter (Quick Sort) Applied!\nWindow Size: " + windowSize + "x" + windowSize + "\nExecution Time: " + executionTime + " ms");
        }







        private void analyze_Click(object sender, EventArgs e)
        {
            if (ImageMatrix == null)
            {
                MessageBox.Show("Please open an image first!");
                return;
            }
            double[] window_sizes = { 3, 5, 7 };
            //to store time to put on y-axis
            double[] quick_y = new double[3];
            double[] counting_y = new double[3];
            double[] kth_y = new double[3];
            double[] MidpointEfficient_y= new double[3];
            double[] NonEfficientMidPoint_y = new double[3];

            // calculate time taken for each filter at each window 
            Stopwatch sw = new Stopwatch();
            for(int i = 0; i < window_sizes.Length; i++)
            {
                int currentSize = (int)window_sizes[i];
                //time of quick
                sw.Restart();
                ImageFilters.ImageOperations.MedianQuickSortFilter(ImageMatrix,currentSize);
                sw.Stop();
                quick_y[i] = sw.ElapsedMilliseconds;

                //time of counting
                // sw.Restart();
                //ImageFilters.ImageOperations.MedianCountingFilter(ImageMatrix, currentSize);
                //sw.Stop();
                //counting_y[i] = sw.ElapsedMilliseconds;

                //time of kth
                sw.Restart();
                ImageFilters.ImageOperations.KthFilter(ImageMatrix,currentSize);
                sw.Stop();
                kth_y[i] = sw.ElapsedMilliseconds;

                //time of Midpoint Efficient
                sw.Restart();
                ImageFilters.ImageOperations.ApplyMidpointEfficient(ImageMatrix, currentSize);
                sw.Stop();
                MidpointEfficient_y[i] = sw.ElapsedMilliseconds;

                //time of Midpoint Efficient
                sw.Restart();
                ImageFilters.ImageOperations.ApplyMidpointNonEfficient(ImageMatrix, currentSize);
                sw.Stop();
                NonEfficientMidPoint_y[i] = sw.ElapsedMilliseconds;
            }

            Form2 graphForm = new Form2();
            graphForm.DrawAllGraphs(window_sizes, quick_y, kth_y, MidpointEfficient_y, NonEfficientMidPoint_y);
            graphForm.Show();
        }
    }
}










        private void analyze_Click(object sender, EventArgs e)
        {
            if (ImageMatrix == null)
            {
                MessageBox.Show("Please open an image first!");
                return;
            }
            double[] window_sizes = { 3, 5, 7 };
            //to store time to put on y-axis
            double[] quick_y = new double[3];
            double[] counting_y = new double[3];
            double[] kth_y = new double[3];
            double[] heuristic_y = new double[3];
            double[] MidpointEfficient_y= new double[3];
            double[] NonEfficientMidPoint_y = new double[3];

            // calculate time taken for each filter at each window 
            Stopwatch sw = new Stopwatch();
            for(int i = 0; i < window_sizes.Length; i++)
            {
                int currentSize = (int)window_sizes[i];
                //time of quick
                sw.Reset();
                sw.Start();
                ImageFilters.ImageOperations.MedianQuickSortFilter(ImageMatrix,currentSize);
                sw.Stop();
                quick_y[i] = sw.ElapsedMilliseconds;

                //time of counting
                sw.Reset();
                sw.Start();
                ImageFilters.ImageOperations.ApplyMedianFilter_CountingSort(ImageMatrix, currentSize);
                sw.Stop();
                counting_y[i] = sw.ElapsedMilliseconds;

                //time of heuristic (sliding window)
                sw.Reset();
                sw.Start();
                ImageFilters.ImageOperations.ApplyMedianFilter_SlidingWindow(ImageMatrix, currentSize);
                sw.Stop();
                heuristic_y[i] = sw.ElapsedMilliseconds;

                //time of kth
                sw.Reset();
                sw.Start();
                ImageFilters.ImageOperations.KthFilter(ImageMatrix,currentSize);
                sw.Stop();
                kth_y[i] = sw.ElapsedMilliseconds;

                //time of Midpoint Efficient
                sw.Reset();
                sw.Start();
                ImageFilters.ImageOperations.ApplyMidpointEfficient(ImageMatrix, currentSize);
                sw.Stop();
                MidpointEfficient_y[i] = sw.ElapsedMilliseconds;

                //time of Midpoint Efficient
                sw.Reset();
                sw.Start();
                ImageFilters.ImageOperations.ApplyMidpointNonEfficient(ImageMatrix, currentSize);
                sw.Stop();
                NonEfficientMidPoint_y[i] = sw.ElapsedMilliseconds;
            }

            Form2 graphForm = new Form2();
            graphForm.DrawAllGraphs(window_sizes, quick_y, kth_y, counting_y, heuristic_y, MidpointEfficient_y, NonEfficientMidPoint_y);
            graphForm.Show();
        }
    }
}



