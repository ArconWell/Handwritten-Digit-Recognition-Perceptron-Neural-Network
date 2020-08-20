using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Backend
{
    public static class NeuralNetwork
    {
        public static int RecognizeDigit(Bitmap bmp)
        {
            int result = new int();
            int hiddenLayersCount = 2;
            int hiddenLayerNeuronsCount = 16;
            int outputLayerNeuronsCount = 10;
            int[] weightsCount;
            int hiddenLayersBiasesCount = hiddenLayerNeuronsCount;
            int outputLayerBiasesCount = outputLayerNeuronsCount;

            float[] inputLayer = InitializeInputLayer(bmp);

            SetEveryLayerWeightsCount(inputLayer.Length, hiddenLayersCount, hiddenLayerNeuronsCount, outputLayerNeuronsCount,
                out weightsCount);

            float[,] hiddenLayers = new float[hiddenLayersCount, hiddenLayerNeuronsCount];

            return result;
        }
      
        //This is much faster than Bitmap's method GetPixel()
        private static byte[] GetBitmapRGBByteArray(Bitmap bmp)
        {
            //Lock the bitmap's bits
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);

            //Get the address of the first line
            IntPtr ptr = bmpData.Scan0;

            //Declare an array to hold the bytes of the bitmap
            int bytes = Math.Abs(bmpData.Stride) * bmp.Height;
            byte[] rgbValues = new byte[bytes];

            // Copy the RGB values into the array
            Marshal.Copy(ptr, rgbValues, 0, bytes);

            //Unlock the bits
            bmp.UnlockBits(bmpData);

            //The first value is for the first pixel's red value, the second is for the first pixel's green value,
            //the third is for the first pixel's blue value, the fourth is for the second pixel's red value etc 
            return rgbValues;
        }

        private static float[] InitializeInputLayer(Bitmap bmp)
        {
            float[] inputLayer = new float[bmp.Width * bmp.Height];

            byte[] rgbValues = GetBitmapRGBByteArray(bmp);
            int count = 0;
            for (int i = 0; i < rgbValues.Length; i += 3)
            {
                inputLayer[count] = (float)Math.Round((float)((rgbValues[i] + rgbValues[i + 1] + rgbValues[i + 2]) / 3) / 255, 2);
                count++;
            }

            return inputLayer;
        }

        private static void SetEveryLayerWeightsCount(int inputLayerNeuronsCount, int hiddenLayersCount,
           int hiddenLayerNeuronsCount, int outputLayerNeuronsCount, out int[] weightsCount)
        {
            weightsCount = new int[hiddenLayersCount + 1];
            weightsCount[0] = inputLayerNeuronsCount * hiddenLayerNeuronsCount;//between input and first hidden layers
            for (int i = 1; i < hiddenLayersCount; i++)
            {
                weightsCount[i] = hiddenLayerNeuronsCount * hiddenLayerNeuronsCount;
            }
            weightsCount[hiddenLayersCount] = hiddenLayerNeuronsCount * outputLayerNeuronsCount;//between last hidden and output layers
        }

        private static float Sigmoid(float x)
        {
            return (float)(1 / (1 + Math.Exp(-x)));
        }
    }
}
