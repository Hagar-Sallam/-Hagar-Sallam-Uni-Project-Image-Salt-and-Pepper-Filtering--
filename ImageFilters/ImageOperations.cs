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
        /// <summary>
        /// Open an image, convert it to gray scale and load it into 2D array of size (Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of gray values</returns>
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

        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>
        public static int GetHeight(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }

        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }

        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
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
        static void heapifyMax(byte[] arr, int n, int i)
        {
            int largest = i;      // 1 step
            int l = 2 * i + 1;   //1 step
            int r = 2 * i + 2;  //1 step

            if (l < n && arr[l] > arr[largest]) // 5 steps
                largest = l; // 1 step

            if (r < n && arr[r] > arr[largest]) //5 steps
                largest = r; // 1 step

            if (largest != i)// 1 step 
            {
                byte swap = arr[i]; // 2 steps
                arr[i] = arr[largest]; //3 steps
                arr[largest] = swap; //2 steps

                heapifyMax(arr, n, largest);//
            }
        }

        static void buildHeapMax(byte[] arr, int n) //O(N log(N))
        {
            int startIdx = (n / 2) - 1; //3 steps


            for (int i = startIdx; i >= 0; i--) //2*startIdx+2
            {
                heapifyMax(arr, n, i); //O(log n)
            }
        }

        static void heapifyMin(byte[] arr, int n, int i)
        {
            int largest = i;
            int l = 2 * i + 1;
            int r = 2 * i + 2;

            if (l < n && arr[l] < arr[largest])
                largest = l;

            if (r < n && arr[r] < arr[largest])
                largest = r;

            if (largest != i)
            {
                byte swap = arr[i];
                arr[i] = arr[largest];
                arr[largest] = swap;

                heapifyMin(arr, n, largest);
            }
        }

        static void buildHeapMin(byte[] arr, int n)
        {
            int startIdx = (n / 2) - 1;
            for (int i = startIdx; i >= 0; i--)
            {
                heapifyMin(arr, n, i);
            }
        }

        public static Boolean checkBounds(byte[,] ImageMatrix, int i, int j)
        {
            int height = GetHeight(ImageMatrix);
            int width = GetWidth(ImageMatrix);
            return (i >= 0 && i < height && j >= 0 && j < width);
        }
        public static int partition(byte[] Array, int low, int high)
        {
            byte pivot = Array[high];// 2 steps
            byte Temp;
            int i = low;// 1 step
            for (int j = low; j < high; j++) //2*(high-low)+2
            {
                if (Array[j] <= pivot)// 2(high-low) steps
                {
                    Temp = Array[j]; //2(high-low) steps
                    Array[j] = Array[i];//3(high-low) steps
                    Array[i++] = Temp;//3(high-low) steps
                }
            }
            Temp = Array[i];//2 steps
            Array[i] = Array[high]; //3 steps
            Array[high] = Temp; // 2 steps
            return i; // 1 step
        }
        public static byte[] quick_sort(byte[] Array, int low, int high)
        {
            if (low < high)
            {
                int pivot = partition(Array, low, high);
                quick_sort(Array, low, pivot - 1);
                quick_sort(Array, pivot + 1, high);
            }
            return Array;
        }
        public static void CountingSort(byte[] Array, int ArrayLength, int Max)
        {
            {
                byte[] count = new byte[Max + 1];//1 step

                for (int i = 0; i < count.Length; i++)//2*count.Length+2
                {
                    count[i] = 0; //2*count.Length steps
                }
                for (int i = 0; i < ArrayLength; i++) //2*ArrayLength+2
                {
                    count[Array[i]]++; //2*ArrayLength steps 
                }

                for (int i = 0, j = 0; i <= Max; i++)//2*Max+4
                {
                    while (count[i] > 0)//(2*count[i]+1)*(Max+1)
                    {
                        Array[j] = (byte)i;//Max*count[i]*2
                        j++;//Max*count[i] step
                        count[i]--; //2*Max*count[i] steps
                    }
                }
            }
        }
        public static void Filter1(byte[,] ImageMatrix, int ws, int t, int sort)
        {

            int height = GetHeight(ImageMatrix); // 2 steps
            int width = GetWidth(ImageMatrix); // 2 steps

            for (int i = 0; i < height; i++) // 2*height+2 steps
            {
                for (int j = 0; j < width; j++) // 2*width+2 steps
                {

                    Trim_Filter(ImageMatrix, i, j, ws, t, sort);
                }
            }

        }
        public static void Trim_Filter(byte[,] ImageMatrix, int x, int y, int W_Max_Size, int trim, int sort)
        {
            int sz = W_Max_Size * W_Max_Size; // 2 steps
            byte[] CurrentWindow = new byte[sz];
            int index = 0;
            int sum = 0;
            int Max = 0;

            if (W_Max_Size % 2 == 0)
                W_Max_Size++;

            for (int i = x - (W_Max_Size / 2); i <= (W_Max_Size / 2) + x; i++)
            {
                for (int j = y - (W_Max_Size / 2); j <= y + (W_Max_Size / 2); j++)
                {

                    if (checkBounds(ImageMatrix, i, j))
                    {
                        CurrentWindow[index] = ImageMatrix[i, j];
                        if (CurrentWindow[index] > Max)
                            Max = CurrentWindow[index];
                       

                        index++;


                    }

                }
            }
            if (sort == 1) // 1 step
            {
                if (trim * 2 > index) // checked // 2 steps
                {
                    for (int i = 0; i < index; i++) // 2*index+2 steps 
                    {
                        sum += CurrentWindow[i];      // 3*index steps
                    }
                    int Avg = sum / index;   // 2 step
                    ImageMatrix[x, y] = (byte)Avg; // 1 step
                }
                else
                {


                    CountingSort(CurrentWindow, index, Max); // checked  // ANALYZE



                    /*  for (int k = 0; k < trim; k++)  // 2*trim+2 steps 
                      {
                          // OPTIMIZE
                          CurrentWindow = Remove.RemoveAt(CurrentWindow, 0); // checked // 6+2*CurrentWindow.Length
                      }
                      for (int k = 0; k < trim; k++) // 2*trim+2 steps 
                      {
                          // OPTIMIZE
                          if (index - trim - k - 1 >= 0) // NOT CHECKED // 4 steps (NOT CHECKED )
                              CurrentWindow = Remove.RemoveAt(CurrentWindow, index - trim - k - 1); // checked // 6+2*CurrentWindow.Length

                      }*/


                    int newsize = index - (2 * trim); // 3 steps       //checked
                    for (int i = trim; i < index - trim; i++)
                    {
                        sum += CurrentWindow[i]; // 3*newsize steps
                    }
                    int Avg = sum / newsize; // 2 step
                    ImageMatrix[x, y] = (byte)Avg; // 3 step

                }

            }
            if (sort == 2) // 1 step
            {

                if (trim * 2 > index) // checked // 2 steps
                {
                    for (int i = 0; i < index; i++) // 2*index+2 steps
                    {
                        sum += CurrentWindow[i]; // 2*index
                    }
                    int Avg = sum / index; // 2 steps
                    ImageMatrix[x, y] = (byte)Avg; // 3 steps (NOT CHECKED)
                }
                else
                {
                    buildHeapMax(CurrentWindow, index);

                    for (int i = 1; i <= trim; i++) // 2*trim+2 steps
                    {

                        if (index != 0) // 1 step
                        {
                            CurrentWindow[0] = CurrentWindow[index - 1]; // 1 step
                            index--; // 2 steps
                                     // buildHeapMax(CurrentWindow, index);
                            heapifyMax(CurrentWindow, index, 0);

                        }
                    }

                    buildHeapMin(CurrentWindow, index);

                    for (int i = 1; i <= trim; i++) // 2*trim+2
                    {

                        if (index != 0) // 1 step
                        {
                            CurrentWindow[0] = CurrentWindow[index - 1]; // 1 step
                            index--; // 2 steps
                                     //   buildHeapMin(CurrentWindow, index);
                            heapifyMin(CurrentWindow, index, 0);
                        }
                    }

                    int newsize = index;
                    for (int i = 0; i < newsize; i++)
                    {
                        sum += CurrentWindow[i];
                    }

                    int Avg = sum / newsize;
                    ImageMatrix[x, y] = (byte)Avg;
                }
            }
        }


        public static void Filter2(byte[,] ImageMatrix, int w, int ws, int sort)
        {
            int height = GetHeight(ImageMatrix);//2 steps
            int width = GetWidth(ImageMatrix);//2 steps
            for (int i = 0; i < height; i++)//2*height+2
            {
                for (int j = 0; j < width; j++) //(2*width+2)*height
                {

                    AdaptiveFilter(ImageMatrix, i, j, w, ws, sort);
                }
            }

        }


        public static void AdaptiveFilter(byte[,] ImageMatrix, int x, int y, int w, int ws, int sort)
        {
            int sz = w * w; // 1 step
            byte[] CurrentWindow = new byte[sz]; // 1 step
            int index = 0; //1 step
            int Max = 0;//1 step

            for (int i = x - (w / 2); i <= (w / 2) + x; i++)//2*w+2
            {
                for (int j = y - (w / 2); j <= y + (w / 2); j++)//(2*w+2)*w
                {

                    if (checkBounds(ImageMatrix, i, j))//9*W_Max_Size*W_Max_Size steps
                    {
                        CurrentWindow[index] = ImageMatrix[i, j];//3*W_Max_Size*W_Max_Size steps
                        if (CurrentWindow[index] > Max)//2*W_Max_Size*W_Max_Size steps - worst case
                            Max = CurrentWindow[index];//2*W_Max_Size*W_Max_Size steps - worst case
                        

                        index++; //W_Max_Size* W_Max_Size steps - worst case
                    }
                }
            }
            if (sort == 1) // 1 step
                quick_sort(CurrentWindow, 0, index - 1); //
            else if (sort == 2) // 1 step
                CountingSort(CurrentWindow, index, Max); //
            int Zxy = ImageMatrix[x, y], Zmax = CurrentWindow[index - 1], Zmin = CurrentWindow[0], Zmed = CurrentWindow[index / 2];// 1 step for each

            int A1 = Zmed - Zmin;// 1 step
            int A2 = Zmax - Zmed;// 1 step

            if (A1 > 0 && A2 > 0) //3 steps
            {
                int B1 = Zxy - Zmin; // 1 step

                int B2 = Zmax - Zxy;// 1 step

                if (!(B1 > 0 && B2 > 0)) //4 steps
                {
                    ImageMatrix[x, y] = (byte)Zmed; // 2 steps
                }

            }
            else
            {
                w += 2; // 2 steps
                if (w < ws) //1 step for <
                    AdaptiveFilter(ImageMatrix, x, y, w, ws, sort);//
                else
                    ImageMatrix[x, y] = (byte)Zmed;//2 steps

            }

        }

      
    }
}
