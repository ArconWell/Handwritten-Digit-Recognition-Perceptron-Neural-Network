using System;
using System.ComponentModel;
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
            int hiddenLayersBiasesCount = hiddenLayerNeuronsCount;
            int outputLayerBiasesCount = outputLayerNeuronsCount;

            float[] inputLayer = InitializeInputLayer(bmp);

            float[][,] weights = InitializeLayersWeightsByRandomValue(inputLayer.Length, hiddenLayerNeuronsCount, hiddenLayersCount,
                outputLayerNeuronsCount);

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

        private static float Sigmoid(float x)
        {
            return (float)(1 / (1 + Math.Exp(-x)));
        }

        private static float[][,] InitializeLayersWeightsByRandomValue(int inputLayerNeuronsCount, int hiddenLayerNeuronsCount, int hiddenLayersCount, 
            int outputLayerNeuronsCount)
        {
            Random rnd = new Random();

            //Local function
            float[,] InitializeLayerWeightsByRandomValues(int firstLayerNeuronsCount, int secondLayerNeuronsCount)
            {
                float[,] layerWeights = new float[secondLayerNeuronsCount, firstLayerNeuronsCount];
                for (int i = 0; i < secondLayerNeuronsCount; i++)
                {
                    for (int j = 0; j < firstLayerNeuronsCount; j++)
                    {
                        layerWeights[i, j] = (float)rnd.Next(-100, 101) / 100;
                    }
                }
                return layerWeights;
            }

            float[][,] weights = new float[hiddenLayersCount+1][,];
            weights[0] = InitializeLayerWeightsByRandomValues(inputLayerNeuronsCount, hiddenLayerNeuronsCount);
            for (int i = 1; i < hiddenLayersCount; i++)
            {
                weights[i] = InitializeLayerWeightsByRandomValues(hiddenLayerNeuronsCount, hiddenLayerNeuronsCount);
            }
            weights[hiddenLayersCount] = InitializeLayerWeightsByRandomValues(hiddenLayerNeuronsCount, outputLayerNeuronsCount);
            return weights;
        }
    }
}
