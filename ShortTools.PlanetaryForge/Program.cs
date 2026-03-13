using ShortTools.Perlin;


namespace ShortTools.PlanetaryForge
{
    public static class MapGenerator
    {
        static float[,] alitudeMap;
        static Tuple<int, int> centre = new Tuple<int, int>(0, 0);



        /// <summary>
        /// Returns a map comprised of tile types, and a seperate jagged array for the altitudes for height map rendering.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static (TileID[][], float[][]) CreateMap(int width, int height)
        {
            Random random = new Random();

            centre = new Tuple<int, int>(width / 2, height / 2);

            float[,] treeMap = Perlin.Perlin.GeneratePerlinMap(width, height, 8, random: random);

            float[,] continentMap = Perlin.Perlin.GeneratePerlinMap(width, height, 64, 4f, random: random);
            float[,] firstMap = Perlin.Perlin.GeneratePerlinMap(width, height, 16, random: random);
            float[,] secondMap = Perlin.Perlin.GeneratePerlinMap(width, height, 8, 0.25f, random: random);

            float[,] perlinMap = Perlin.Perlin.CombineFloatMaps(firstMap, secondMap, 1.25f);
            perlinMap = Perlin.Perlin.CombineFloatMaps(continentMap, perlinMap, 5f);

            perlinMap = ApplyFuncToMap(perlinMap);

            perlinMap = CreateIsland(perlinMap);

            alitudeMap = perlinMap;


            float[][] jaggedAltitudeMap = new float[width][];
            TileID[][] jaggedTileMap = new TileID[width][];
            for (int x = 0; x < width; x++)
            {
                jaggedAltitudeMap[x] = new float[height];
                jaggedTileMap[x] = new TileID[height];
                for (int y = 0; y < height; y++)
                {
                    jaggedAltitudeMap[x][y] = alitudeMap[x, y];
                    jaggedTileMap[x][y] = GetTileID(perlinMap, treeMap, x, y);
                }
            }

            return (jaggedTileMap, jaggedAltitudeMap);
        }



        private static TileID GetTileID(float[,] perlinMap, float[,] treeMap, int x, int y)
        {
            float value = perlinMap[x, y];
            //if (GetTileGradient(x, y, perlinMap) > 0.2f) { return new Tuple<byte, byte, byte>(255, 255, 255); }


            if (
                (value > 0 && GetTileGradient(x, y, perlinMap) > 0.27f) ||
                (value > 0.4f && GetTileGradient(x, y, perlinMap) > 0.15f))
            {
                return TileID.Cliff;
            }

            if (value < 0f) // bright shore thingie
            {
                //return new Tuple<byte, byte, byte>(255, 255, 255);
                return TileID.Water;
            }
            else if (value < 0.1f)
            {
                return TileID.Sand;
            }
            else
            { // 0.1 to 1, thats 0.9 which is basically 0 - 1 : grass
                if (treeMap[x, y] > 0.1f)
                {
                    return TileID.Forest;
                }
                else
                {
                    return TileID.Grass;
                }
            }
        }










        private static float[,] ApplyFuncToMap(float[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            float[,] outMap = new float[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    outMap[x, y] = MathF.Tanh(4 * map[x, y]);
                }
            }

            return outMap;
        }


        private static float GetTileGradient(int x, int y, float[,] PerlinMap)
        {
            if (x == 0 || y == 0 || x == PerlinMap.GetLength(0) - 1 || y == PerlinMap.GetLength(1) - 1) { return 0f; }

            float mainTile = PerlinMap[x, y];

            float dn = MathF.Abs(PerlinMap[x, y - 1] - mainTile);
            float de = MathF.Abs(PerlinMap[x + 1, y] - mainTile);
            float ds = MathF.Abs(PerlinMap[x, y + 1] - mainTile);
            float dw = MathF.Abs(PerlinMap[x - 1, y] - mainTile);

            return (dn + de + ds + dw);
        }



        const float pathWidth = 50f;
        const float pathHeight = 1.8f;
        const float borderWidth = 50f;
        const float borderHeight = 3f;
        const float islandWidth = 40f;
        private static float[,] CreateIsland(float[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            float[,] outMap = new float[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float altitude = GetIslandHeight(width / 2, height / 2, x, y, islandWidth);


                    // <<Paths>> //

                    float pathAlt = 0;

                    if (x > (width / 2) - pathWidth && x < (width / 2) + pathWidth)
                    {
                        pathAlt += pathHeight - MathF.Abs((pathHeight / pathWidth) * (x - (width / 2)));
                    }
                    if (y > (height / 2) - pathWidth && y < (height / 2) + pathWidth)
                    {
                        pathAlt += pathHeight - MathF.Abs((pathHeight / pathWidth) * (y - (height / 2)));
                    }


                    // <<Border>> //

                    // <<X>> //
                    float borderAlt = -1;

                    if (x < borderWidth * 2)
                    {
                        borderAlt = MathF.Max(borderAlt, borderHeight * (borderWidth - x) / borderWidth);
                    }
                    if (y < borderWidth * 2)
                    {
                        borderAlt = MathF.Max(borderAlt, borderHeight * (borderWidth - y) / borderWidth);
                    }
                    if (x > width - (borderWidth * 2))
                    {
                        borderAlt = MathF.Max(borderAlt, borderHeight * (x - width + borderWidth) / borderWidth);
                    }
                    if (y > height - (borderWidth * 2))
                    {
                        borderAlt = MathF.Max(borderAlt, borderHeight * (y - height + borderWidth) / borderWidth);
                    }






                    altitude = MathF.Max(MathF.Max(altitude, pathAlt), borderAlt);

                    if (altitude > 0)
                    {
                        outMap[x, y] = float.Clamp(map[x, y] + altitude, -1, 1);
                    }
                    else
                    {
                        outMap[x, y] = map[x, y];
                    }
                }
            }

            return outMap;
        }

        private static float GetIslandHeight(int cx, int cy, int x, int y, float islandWidth = 20f)
        {
            return MathF.Tanh(4 * (MathF.Exp(-((MathF.Pow(x - cx, 2)) + (MathF.Pow(y - cy, 2))) / (islandWidth * islandWidth)) - 0.5f));
        }









        private static void Main()
        {
            Console.WriteLine("Why are you running this?");
        }
    }





    public enum TileID
    {
        Water,
        Grass,
        Sand,
        Forest,
        Cliff
    }
}