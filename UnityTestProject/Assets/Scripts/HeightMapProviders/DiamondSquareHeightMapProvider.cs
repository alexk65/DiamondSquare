using System;
using Random = UnityEngine.Random;

namespace Assets.Scripts.HeightMapProviders
{
    public class DiamondSquareHeightMapProvider : IHeightMapProvider
    {
        private float[,] heightMap;
        private int mapSideSize;
        private float roughness;

        public float[,] Generate(int power, float roughness)
        {
            mapSideSize = (int)Math.Pow(2, power);
            this.roughness = roughness;
            heightMap = new float[mapSideSize, mapSideSize];
            GenerateRandomAngles();
            DiamondSquare(mapSideSize);

            return heightMap;
        }

        private void DiamondSquare(int stepSize)
        {
            var halfSide = stepSize / 2;
            if (halfSide < 1)
            {
                return;
            }

            for (var y = halfSide; y < mapSideSize; y += stepSize)
            {
                for (var x = halfSide; x < mapSideSize; x += stepSize)
                {
                    Square(x, y, halfSide);
                }
            }

            for (var y = 0; y < mapSideSize; y += halfSide)
            {
                for (var x = (y + halfSide) % stepSize; x < mapSideSize; x += stepSize)
                {
                    Diamond(x, y, halfSide);
                }
            }

            DiamondSquare(stepSize / 2);
        }

        private void Square(int x, int y, int halfSide)
        {
            var a = GetHeight(x - halfSide, y - halfSide);
            var b = GetHeight(x + halfSide, y - halfSide);
            var c = GetHeight(x + halfSide, y + halfSide);
            var d = GetHeight(x - halfSide, y + halfSide);

            heightMap[x, y] = GenerateHeight(a, b, c, d, halfSide);
        }

        private void Diamond(int x, int y, int halfSide)
        {
            var a = GetHeight(x, y - halfSide);
            var b = GetHeight(x + halfSide, y);
            var c = GetHeight(x, y + halfSide);
            var d = GetHeight(x - halfSide, y);

            heightMap[x, y] = GenerateHeight(a, b, c, d, halfSide);
        }

        private void GenerateRandomAngles()
        {
            const float min = 0.3f;
            const float max = 0.6f;

            heightMap[0, 0] = Random.Range(min, max);
            heightMap[0, mapSideSize - 1] = Random.Range(min, max);
            heightMap[mapSideSize - 1, mapSideSize - 1] = Random.Range(min, max);
            heightMap[mapSideSize - 1, 0] = Random.Range(min, max);
        }

        private float GenerateHeight(float a, float b, float c, float d, int halfSide)
        {
            return (a + b + c + d) / 4 + Random.Range(-halfSide * 2 * roughness / mapSideSize, halfSide * 2 * roughness / mapSideSize);
        }

        private float GetHeight(int x, int y)
        {
            if (x < 0 || y < 0 || x > mapSideSize - 1 || y > mapSideSize - 1)
            {
                return 0;
            }

            return heightMap[x, y];
        }
    }
}
