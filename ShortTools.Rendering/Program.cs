using ShortTools.General;

/*

// <<Plan>> //

User passes in 2 functions, HandleInput and Render







*/

namespace ShortTools.Rendering
{
    internal class GraphicsHandlerTester : GraphicsHandler 
    { 
        public GraphicsHandlerTester(
            int screenWidth = -1, int screenHeight = -1,
            Action? render = null, Action<string, WarningLevel>? logOutput = null,
            params RendererFlag[] flags) : base(screenWidth, screenHeight, render, logOutput, flags) {  }
    }












    internal static class Testing
    {
        static int w = 2;
        static int h = 2;

        static GraphicsHandler renderer;

        private static void Main()
        {
            using (renderer = new GraphicsHandlerTester(
                render: Render,
                flags: [RendererFlag.LogOutput, RendererFlag.WriteLogsToConsole]))
            {
                Console.WriteLine("Testing 123");

                Thread.Sleep(10000);

                Console.WriteLine("Terminating");
            }
        }


        static readonly long start = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        static float offset = 2 * MathF.PI / 3f;
        private static void Render()
        {
            int xTiles = renderer.ScreenWidth / w;
            int yTiles = 1 + renderer.ScreenHeight / h;

            renderer.SetPixel(0, 0, renderer.ScreenWidth, renderer.ScreenHeight, 0, 255, 0);

            for (int x = 0; x < xTiles; x++)
            {
                for (int y = 0; y < yTiles; y++)
                {
                    float timeOffset = ((DateTimeOffset.Now.ToUnixTimeMilliseconds() - start) / 100f);
                    float number = (x + y) * 2 * MathF.PI / (yTiles + xTiles);
                    byte r = (byte)((MathF.Cos(timeOffset + number) + 1) * (255f / 2));
                    byte g = (byte)((MathF.Cos(timeOffset + number + offset) + 1) * (255f / 2));
                    byte b = (byte)((MathF.Cos(timeOffset + number + 2 * offset) + 1) * (255f / 2));

                    renderer.SetPixel(x * w, y * h, w, h, r, 0, 0, 120);
                }
            }
        }
    }
}