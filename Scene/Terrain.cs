using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteHealthCare.Scene
{
    internal class Terrain
    {
        private struct TerrainSine
        {
            private double _offset;
            private double _xLength;
            private double _yLength;
            private double _height;

            /// <summary>
            /// The sine wave of the terrain, created to make a random wave terrain
            /// </summary>
            /// <param name="offset">An offset in the wave's starting point in the sine</param>
            /// <param name="length">The distance between the average points of the wave</param>
            /// <param name="angle">The direction of the wave (from right going anticlockwise in radians)</param>
            /// <param name="height">The wave's amplitude (maximum height difference with average)</param>
            public TerrainSine(double offset, double length, double angle, double height)
            {
                _offset = offset;
                _xLength = length * Math.Cos(angle);
                _yLength = length * Math.Sin(angle);
                _height = height;
            }

            /// <summary>
            /// Calculates the assigned sine wave at the given coordinates
            /// </summary>
            /// <param name="x">X coordinate</param>
            /// <param name="y">Y coordindate</param>
            /// <returns>The result of the wave</returns>
            public double Calculate(int x, int y)
            {
                return _height * (1 + Math.Sin(_offset + (x / _xLength + y / _yLength)));
            }
        }

        public double[,] TerrainHeights { get; private set; }

        public Terrain()
        {
            Generate();
        }

        /// <summary>
        /// Generates a random wavy terrain in a sine wave shape
        /// </summary>
        private void Generate()
        {
            Random random = new Random();
            List<TerrainSine> terrainSines = new List<TerrainSine>();
            for (int i = 0; i < random.Next(3, 6); i++)
            {
                terrainSines.Add(new TerrainSine(random.NextDouble() * Math.PI, 16 + random.NextDouble() * 48,
                    random.NextDouble() * Math.PI, random.NextDouble() * random.NextDouble()));
            }

            TerrainHeights = new double[256, 256];
            for (int i = 0; i < 256; i++)
            {
                for (int j = 0; j < 256; j++)
                {
                    TerrainHeights[j, i] = terrainSines.Sum(x => x.Calculate(i, j));
                }
            }
        }
    }
}