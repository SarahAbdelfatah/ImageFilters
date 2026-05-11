using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace ImageFilters
{
    public class ImageOperations
    {
        public static byte[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;

            byte[,] Buffer = new byte[Height, Width];

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x] = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x] = (byte)((int)(p[0] + p[1] + p[2]) / 3);
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }
        public static int GetHeight(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }
        public static int GetWidth(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }
        public static void DisplayImage(byte[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);

            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

            unsafe
            {
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[0] = p[1] = p[2] = ImageMatrix[i, j];
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }

        public static byte SelectKth(byte[] arr, int left, int right, int k)
        {
            if (left == right) return arr[left];

            int pivotIndex = Partition(arr, left, right);

            if (k == pivotIndex)
                return arr[k];
            else if (k < pivotIndex)
                return SelectKth(arr, left, pivotIndex - 1, k);
            else
                return SelectKth(arr, pivotIndex + 1, right, k);
        }

        private static int Partition(byte[] arr, int left, int right)
        {
            byte pivotValue = arr[right];
            int i = left;
            for (int j = left; j < right; j++)
            {
                if (arr[j] < pivotValue)
                {
                    // Swap arr[i] and arr[j]
                    byte temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                    i++;
                }
            }
            // Swap pivot into final place
            arr[right] = arr[i];
            arr[i] = pivotValue;
            return i;
        }

        public static byte[,] KthFilter(byte[,] ImageMatrix, int windowSize)
        {
            int width = GetWidth(ImageMatrix);
            int height = GetHeight(ImageMatrix);
            byte[,] resultImage = new byte[height, width];

            int edge = windowSize / 2;
            int kIndex = (windowSize * windowSize) / 2; // The index of the median

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte[] window = new byte[windowSize * windowSize];
                    int count = 0;

                    // Fill the window with neighboring pixels
                    for (int wy = -edge; wy <= edge; wy++)
                    {
                        for (int wx = -edge; wx <= edge; wx++)
                        {
                            int neighborY = y + wy;
                            int neighborX = x + wx;

                            // Boundary Check: If outside, use the closest edge pixel
                            if (neighborY < 0) neighborY = 0;
                            if (neighborY >= height) neighborY = height - 1;
                            if (neighborX < 0) neighborX = 0;
                            if (neighborX >= width) neighborX = width - 1;

                            window[count++] = ImageMatrix[neighborY, neighborX];
                        }
                    }

                    // Find the Kth smallest element (the median)
                    resultImage[y, x] = SelectKth(window, 0, window.Length - 1, kIndex);
                }
            }
            return resultImage;
        }

        // ===== Midpoint Filter (Non-Efficient) =====
        // Collects neighbors into an array, then finds max & min with separate loops
        public static byte[,] ApplyMidpointNonEfficient(byte[,] ImageMatrix, int windowSize)
        {
            int height = GetHeight(ImageMatrix);
            int width = GetWidth(ImageMatrix);
            byte[,] newimage = new byte[height, width];
            int edge = windowSize / 2;
            int totalNeighbors = windowSize * windowSize;

            for (int i = edge; i < height - edge; i++)
            {
                for (int j = edge; j < width - edge; j++)
                {
                    byte[] neighbor = new byte[totalNeighbors];
                    int size = 0;
                    for (int k = -edge; k <= edge; k++)
                    {
                        for (int l = -edge; l <= edge; l++)
                        {
                            neighbor[size++] = ImageMatrix[i + k, j + l];
                        }
                    }

                    int max = neighbor[0];
                    for (int x = 0; x < totalNeighbors; x++)
                    {
                        if (max < neighbor[x])
                            max = neighbor[x];
                    }

                    int min = neighbor[0];
                    for (int x = 0; x < totalNeighbors; x++)
                    {
                        if (min > neighbor[x])
                            min = neighbor[x];
                    }

                    newimage[i, j] = (byte)((max + min) / 2);
                }
            }
            return newimage;
        }

        // ===== Midpoint Filter (Efficient) =====
        // Tracks max & min on the fly in a single pass — no extra array needed
        public static byte[,] ApplyMidpointEfficient(byte[,] ImageMatrix, int windowSize)
        {
            int height = GetHeight(ImageMatrix);
            int width = GetWidth(ImageMatrix);
            byte[,] newimage = new byte[height, width];
            int edge = windowSize / 2;

            for (int i = edge; i < height - edge; i++)
            {
                for (int j = edge; j < width - edge; j++)
                {
                    int max = 0;
                    int min = 255;
                    for (int k = -edge; k <= edge; k++)
                    {
                        for (int l = -edge; l <= edge; l++)
                        {
                            int current = ImageMatrix[i + k, j + l];
                            if (current > max)
                                max = current;
                            if (current < min)
                                min = current;
                        }
                    }
                    newimage[i, j] = (byte)((max + min) / 2);
                }
            }
            return newimage;
        }

        public static byte[,] MedianQuickSortFilter(byte[,] ImageMatrix, int windowSize)
        {
            int width = GetWidth(ImageMatrix);
            int height = GetHeight(ImageMatrix);

            byte[,] resultImage = new byte[height, width];

            int edge = windowSize / 2;
            int windowLength = windowSize * windowSize;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte[] window = new byte[windowLength];

                    int count = 0;

                    // Collect window elements
                    for (int wy = -edge; wy <= edge; wy++)
                    {
                        for (int wx = -edge; wx <= edge; wx++)
                        {
                            int neighborY = y + wy;
                            int neighborX = x + wx;

                            // Boundary check
                            if (neighborY < 0)
                                neighborY = 0;

                            if (neighborY >= height)
                                neighborY = height - 1;

                            if (neighborX < 0)
                                neighborX = 0;

                            if (neighborX >= width)
                                neighborX = width - 1;

                            window[count++] = ImageMatrix[neighborY, neighborX];
                        }
                    }

                    // Sort the window using Quick Sort
                    QuickSort(window, 0, window.Length - 1);

                    // Get the median
                    resultImage[y, x] = window[windowLength / 2];
                }
            }

            return resultImage;
        }

        public static void QuickSort(byte[] arr, int left, int right)
        {
            int i = left;
            int j = right;


            byte pivot = arr[(left + right) / 2];

            while (i <= j)
            {

                while (arr[i] < pivot)
                    i++;

                while (arr[j] > pivot)
                    j--;

                if (i <= j)
                {
                    byte temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;

                    i++;
                    j--;
                }
            }

            if (left < j)
                QuickSort(arr, left, j);
            if (i < right)
                QuickSort(arr, i, right);
        }

        int Partition(int[] arr, int low, int high)
        {
            int pivot = arr[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (arr[j] < pivot)
                {
                    i++;
                    // swap
                    int temp = arr[i];
                    arr[i] = arr[j];
                    arr[j] = temp;
                }
            }

            // swap pivot
            int temp2 = arr[i + 1];
            arr[i + 1] = arr[high];
            arr[high] = temp2;

            return i + 1;
        }


    }
}
