using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.HeightMapProviders
{
    class ImageHeightMapProvider : IHeightMapProvider
    {
        private const string ImageFilePath = "HeightMaps/HeightMap.jpg";

        private float[,] heightMap;
        private int mapSideSize;

        public float[,] Generate(int power, float roughness)
        {
            mapSideSize = (int)Math.Pow(2, power);
            heightMap = new float[mapSideSize, mapSideSize];
            GenerateHeightMap();

            return heightMap;
        }

        private void GenerateHeightMap()
        {
            var texture = GetImageTexture();
            if (texture == null)
            {
                return;
            }

            for (int y = 0; y < mapSideSize; y++)
            {
                for (int x = 0; x < mapSideSize; x++)
                {
                    heightMap[x, y] = texture.GetPixel(x, y).grayscale;
                }
            }
        }

        private Texture2D GetImageTexture()
        {
            var filePath = Path.Combine(Application.dataPath, ImageFilePath);
            if (File.Exists(filePath))
            {
                var fileData = File.ReadAllBytes(filePath);
                var texture = new Texture2D(mapSideSize/2, mapSideSize/2);
                texture.LoadImage(fileData);

                return texture;
            }

            return null;
        }
    }
}
