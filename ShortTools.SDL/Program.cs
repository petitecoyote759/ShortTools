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
            using Renderer rendy = new Renderer(Draw, Handle, flag: RendererFlags.AutoRenderDraw | RendererFlags.PrintDebugLogs | RendererFlags.DebugKeyInputs);
            renderer = rendy;


            renderer.Start();

            Thread.Sleep(5000);

            Thread.Sleep(5000);
        }




        internal static void Handle(SDL_Keycode key, bool down)
        {

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


















}