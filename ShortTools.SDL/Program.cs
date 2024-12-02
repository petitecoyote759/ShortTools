using ShortTools.General;
using static ShortTools.General.Prints;
using SDL2;
using static SDL2.SDL;
using System.Runtime.CompilerServices;

namespace ShortTools.SDL
{
    internal static class GeneralTests
    {
        static Renderer? renderer = null;

        internal static void Main(string[] args)
        {
            using Renderer rendy = new Renderer(Draw, RendererFlags.AutoRenderDraw | RendererFlags.PrintDebugLogs);
            renderer = rendy;


            renderer.Start();

            Thread.Sleep(5000);

            renderer.Pause(2000);

            Thread.Sleep(5000);
        }




        static int xPos = 0;
#pragma warning disable CA1806
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        internal static void Draw(SDLRenderer renderer, SDLWindow window)
        {
            xPos++;

            for (int x = xPos; x < xPos + 100; x++)
            {
                for (int y = 0; y < 100; y++)
                {
                    GeneralTests.renderer?.SetRenderColour(255, 255, 255, 255);
                    SDL_RenderDrawPoint(renderer.pointer, x, y);
                }
            }

            GeneralTests.renderer?.SetRenderColour();
        }
    }
#pragma warning restore CA1806






















#pragma warning disable
    /// <summary>
    /// 
    /// </summary>
    public class ShortException : Exception
    {
        public ShortException(ErrorCode errorCode, string message) : base(message) 
        { ErrorCode = errorCode; }
        public ShortException(ErrorCode errorCode) : base() 
        { ErrorCode = errorCode; }
        public ShortException(ErrorCode errorCode, string message, Exception innerException) : base(message, innerException) 
        { ErrorCode = errorCode; }


        public readonly ErrorCode ErrorCode;
    }
#pragma warning restore
}