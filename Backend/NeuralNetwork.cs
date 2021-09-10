using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Backend
{
    public class NeuralNetwork
    {
        private int outputLayerNeuronsCount;//10
        private int hiddenLayersCount;//2
        private int[] hiddenLayersNeuronsCount;//1 - 16; 2 - 16
        private int outputLayerBiasesCount;
        private int[] hiddenLayersBiasesCount;

        /// <summary>
        /// Initialize new copy of class NeuralNetwork
        /// </summary>
        /// <param name="outputLayerNeuronsCount">Count of neurons in outout layer</param>
        /// <param name="hiddenLayersNeuronsCount">Array consisted of count of neurons in each hidden layer</param>
        public NeuralNetwork(int outputLayerNeuronsCount, params int[] hiddenLayersNeuronsCount)
        {
            this.outputLayerNeuronsCount = outputLayerNeuronsCount;
            hiddenLayersCount = hiddenLayersNeuronsCount.Length;
            this.hiddenLayersNeuronsCount = hiddenLayersNeuronsCount;
            hiddenLayersBiasesCount = this.hiddenLayersNeuronsCount;
            outputLayerBiasesCount = outputLayerNeuronsCount;
        }

        public int RecognizeDigit(Bitmap bmp)
        {
            int result = new int();

            float[] inputLayer = InitializeInputLayer(bmp);

            // FIXME инициализировать веса случайными значениями нужно только при первой тренировке, а здесь должна быть функция
            // считывания сохраненных значений весов из файла
            float[][,] weights = InitializeLayersWeightsByRandomValue(inputLayer.Length, hiddenLayersNeuronsCount, hiddenLayersCount,
                outputLayerNeuronsCount);

            float[][] hiddenLayers = new float[hiddenLayersCount][];//new float[hiddenLayersCount, hiddenLayersNeuronsCount];
            for (int i = 0; i < hiddenLayersNeuronsCount; i++)
            {
                hiddenLayers[0, i] =
            }

            return result;
        }

        //This is much faster than Bitmap's method GetPixel()
        private byte[] GetBitmapRGBByteArray(Bitmap bmp)
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

        private float[] InitializeInputLayer(Bitmap bmp)
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

        private float[][,] InitializeLayersWeightsByRandomValue(int inputLayerNeuronsCount, int[] hiddenLayersNeuronsCount, int hiddenLayersCount,
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

            float[][,] weights = new float[hiddenLayersCount + 1][,];
            weights[0] = InitializeLayerWeightsByRandomValues(inputLayerNeuronsCount, hiddenLayersNeuronsCount[0]);
            for (int i = 1; i < hiddenLayersCount; i++)
            {
                weights[i] = InitializeLayerWeightsByRandomValues(hiddenLayersNeuronsCount[i - 1], hiddenLayersNeuronsCount[i]);
            }
            weights[hiddenLayersCount] = InitializeLayerWeightsByRandomValues(hiddenLayersNeuronsCount[hiddenLayersCount-1], outputLayerNeuronsCount);
            return weights;
        }

        private float Sigmoid(float x)
        {
            return (float)(1 / (1 + Math.Exp(-x)));
        }
    }
}
