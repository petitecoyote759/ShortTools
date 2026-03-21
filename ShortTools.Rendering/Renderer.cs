using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShortTools.General;
using Thread = System.Threading.Thread; // specify i want this over Silk.Net.SDL.Thread


#pragma warning disable CA1805 // Initialised to its default value - This is more clear and looks nicer.
#pragma warning disable IDE0301 // Simplify Collection Initialisation - Looks weird.



namespace ShortTools.Rendering
{
    [Flags]
    public enum RendererFlag
    {
        None = 0,
        LogOutput,
        WriteLogsToConsole
    }


    public abstract partial class GraphicsHandler : IDisposable
    {
        // <<Public Variables>>//

        public int ScreenWidth { get => screenwidth; set => screenwidth = value; }
        public int ScreenHeight { get => screenheight; set => screenheight = value; }


        /// <summary>
        /// Boolean storing if the program is running. If it is false then the program will pause until it is true.
        /// </summary>
        public bool Running => running;






        // <<Private Variables>> //

        protected int screenwidth = -1;
        protected int screenheight = -1;


        protected Thread? thread = null;

        
        private bool running = true;

        
        private long sleepTime; // time in ms until thread should wake up


        private readonly RendererFlag[] flags = Array.Empty<RendererFlag>();


        private readonly Action<string, WarningLevel> logOutput; // Function to output log info
        protected readonly Debugger? debugger = null;


        private bool disposed = false;


        // SDL

        private unsafe Window* window;
        private unsafe Renderer* renderer;

        private readonly Action render;






        // <<Constructors>> //

        /// <summary>
        /// Class for creating a window and drawing simple designs.
        /// </summary>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        /// <param name="render"></param>
        /// <param name="logOutput"></param>
        /// <param name="flags"></param>
        protected GraphicsHandler(
            int screenWidth = -1, int screenHeight = -1,
            Action? render = null, Action<string, WarningLevel>? logOutput = null,
            params RendererFlag[] flags)
        {

            this.render = render ?? (() => { });

            this.flags = flags;

            // Log output defaults to nothing
            this.logOutput = logOutput ?? ((text, priority) => { });
            if (flags.Contains(RendererFlag.LogOutput))
            {
                // overwrites it with debugger addlog if it says to log

                // if it is writing to console, have that flag, else just do all the other ones
                if (flags.Contains(RendererFlag.WriteLogsToConsole)) { this.debugger = new Debugger(DebuggerFlag.ShortDefault); }
                else { this.debugger = new Debugger(DebuggerFlag.ShortDefault ^ DebuggerFlag.PrintLogs); }
                
                this.logOutput = debugger.AddLog;
            }




            try
            {
                Start(screenWidth, screenHeight);
            }
            catch (Exception)
            {
                this.logOutput("Exception occured during startup, shutting down...", WarningLevel.CriticalError);
                throw;
            }
        }



        private ManualResetEventSlim setupDone = new(false);
        private unsafe void Setup(int screenWidth, int screenHeight)
        {
            // Create SDL + Image contexts
            sdl = Sdl.GetApi();

            // SDL Init
            if (sdl.Init(Sdl.InitEverything | Sdl.InitSensor) < 0)
            {
                logOutput("SDL couldn't initialise, reason = " + sdl.GetErrorS(), WarningLevel.CriticalError);
                throw new ShortSDLSetupException("SDL couldn't initialise, reason = " + sdl.GetErrorS());
            }




            if (screenwidth == -1 && screenheight == -1)
            {
                // Get display mode
                DisplayMode displayMode;

                if (sdl.GetCurrentDisplayMode(0, &displayMode) < 0)
                {
                    // Fallback
                    if (sdl.GetDesktopDisplayMode(0, &displayMode) < 0)
                    {
                        sdl.Quit();
                        logOutput(
                            "SDL couldn't initialise either display mode, reason = " +
                            sdl.GetErrorS(), WarningLevel.CriticalError
                        );
                        throw new ShortSDLSetupException("SDL couldn't initialise either display mode, reason = " + sdl.GetErrorS());
                    }
                }
                else
                {
                    screenwidth = displayMode.W;
                    screenheight = displayMode.H;
                }
            }
            else
            {
                screenwidth = screenWidth;
                screenheight = screenHeight;
            }







            // Create window (borderless, centered)
            window = sdl.CreateWindow(
                    "SGL Window",
                    Sdl.WindowposCentered,
                    Sdl.WindowposCentered,
                    screenwidth,
                    screenheight,
                    (uint)WindowFlags.Borderless
                );

            if (window == null)
            {
                sdl.Quit();
                logOutput("SDL window couldn't initialise, reason = " + sdl.GetErrorS(), WarningLevel.CriticalError);
                throw new ShortSDLSetupException("SDL window couldn't initialise, reason = " + sdl.GetErrorS());
            }

            // Create renderer (accelerated + vsync)
            renderer = sdl.CreateRenderer(
                window,
                -1,
                (uint)(RendererFlags.Accelerated)
            );

            if (renderer == null)
            {
                sdl.Quit();
                logOutput("SDL renderer couldn't initialise, reason = " + sdl.GetErrorS(), WarningLevel.CriticalError);
                throw new ShortSDLSetupException("SDL renderer couldn't initialise, reason = " + sdl.GetErrorS());
            }

            // Set default draw color (white)
            if (sdl.SetRenderDrawColor(renderer, 255, 255, 255, 255) < 0)
            {
                sdl.Quit();
                logOutput("SDL draw colour couldn't initialise, reason = " + sdl.GetErrorS(), WarningLevel.CriticalError);
                throw new ShortSDLSetupException("SDL draw colour couldn't initialise, reason = " + sdl.GetErrorS());
            }



            pixelBuffer = new byte[screenwidth * screenheight * 4];

            texture = sdl.CreateTexture(
                renderer,
                (uint)PixelFormatEnum.Rgba8888,
                (int)TextureAccess.Streaming,
                screenwidth,
                screenheight
            );



            setupDone.Set();
        }

















        /// <summary>
        /// Pauses the renderer thread, restart it with Resume or pass in a time for it to sleep for.
        /// </summary>
        /// <param name="sleepTime">The time it will sleep for, if it is set to -1, it will wait until resume is called.</param>
        public void Pause(long sleepTime = -1)
        {
            this.sleepTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() + sleepTime;
            logOutput("Thread paused.", WarningLevel.Info);
            running = false;
        }
        /// <summary>
        /// Resumes the thread that runs the renderer after Pause has been called.
        /// </summary>
        public void Resume()
        {
            logOutput("Thread unpaused.", WarningLevel.Info);
            running = true;
        }



        /// <summary>
        /// Starts the renderer, only use this once when you want the renderer to start up for the first time. For stopping and starting the renderer see Pause() and Resume().
        /// </summary>
        /// <exception cref="SDLError">thrown when SDL errors.</exception>
        public void Start(int screenWidth, int screenHeight)
        {
            if (disposed)
            {
                logOutput($"Attempted to start a disposed renderer", WarningLevel.Warning);
                return;
            }


            thread = new Thread(
                new ThreadStart(
                    (Action)(() => Setup(screenWidth, screenHeight)) + (Action)(() => Controller())
                    ));
            thread.Start();

            logOutput("Renderer started", WarningLevel.Info);

            setupDone.Wait();
        }








        unsafe void Controller()
        {
            while (!disposed)
            {
                Event e;
                while (sdl.PollEvent(&e) == 1)
                {
                    if (e.Type == (uint)EventType.Quit)
                    {
                        disposed = true;
                    }
                }
                if (running)
                {
                    render();


                    fixed (byte* ptr = &pixelBuffer[0])
                    {
                        // Upload CPU buffer to GPU texture
                        _ = sdl.UpdateTexture(texture, null, ptr, screenwidth * 4);

                        // Render texture in one call
                        _ = sdl.RenderCopy(renderer, texture, null, null);
                    }


                    sdl.RenderPresent(renderer);
                }
                else
                {
                    Thread.Sleep(20);
                }
            }
            ThreadDispose();


            logOutput("Renderer Stopped", WarningLevel.Info);
        }







        /// <summary>
        /// Sets a pixel range's colour.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="a"></param>
        public unsafe void SetPixel(int x, int y, int w, int h, byte r, byte g, byte b, byte a = 255)
        {
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    int px = x + i;
                    int py = y + j;

                    if (px < 0 || px >= screenwidth || py < 0 || py >= screenheight)
                    {
                        continue; // ignore any outside
                    }

                    int idx = (py * screenwidth + px) * 4;

                    // Get originals and verge via alphas
                    byte oA = pixelBuffer[idx + 0];
                    byte oB = pixelBuffer[idx + 1];
                    byte oG = pixelBuffer[idx + 2];
                    byte oR = pixelBuffer[idx + 3];

                    float srcA = a / 255f;
                    float invA = 1f - srcA;

                    byte outR = (byte)(r * srcA + oR * invA);
                    byte outG = (byte)(g * srcA + oG * invA);
                    byte outB = (byte)(b * srcA + oB * invA);
                    byte outA = (byte)(a + oA * invA);


                    pixelBuffer[idx + 0] = outA;
                    pixelBuffer[idx + 1] = outB;
                    pixelBuffer[idx + 2] = outG;
                    pixelBuffer[idx + 3] = outR;
                }
            }
        }






        void IDisposable.Dispose()
        {
            disposed = true;
            GC.SuppressFinalize(this);
        }


        unsafe void ThreadDispose()
        {
            sdl.DestroyRenderer(renderer);
            sdl.DestroyWindow(window);
            sdl.Dispose();
        }

    }








    public class ShortSDLSetupException : Exception
    {
        public ShortSDLSetupException() : base("An exception occured during setup.") { }
        public ShortSDLSetupException(string message) : base(message) { }
        public ShortSDLSetupException(string message, Exception innerException) : base(message, innerException) { }
    }
}
