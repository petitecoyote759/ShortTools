using SDL2;
using ShortTools.General;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using static SDL2.SDL;





namespace ShortTools.SDL
{
    /// <summary>
    /// DO NOT USE!!! this adds a single extention for the <see cref="int">int</see> type that checks if the value is 0, it is not intended for public use.
    /// </summary>
    [Obsolete("Not intended for public use.", false, UrlFormat = WarningCodes.URL, DiagnosticId = WarningCodes.NotForPublicUse)]
    public static class ZeroExtention
    {
        /// <summary>
        /// Checks if the value is != 0, makes some code more readable
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool Failed(this int value) => value != 0;
    }










#pragma warning disable CA1720, CA1815, CS1591, CA1051
    /// <summary>
    /// Struct to contain the SDL window pointer so it is more clear what the pointer is refering to.
    /// </summary>
    public struct SDLWindow
    {
        public IntPtr pointer;
    }
    /// <summary>
    /// Struct to contain the SDL window pointer so it is more clear what the pointer is refering to.
    /// </summary>
    public struct SDLRenderer
    {
        public IntPtr pointer;
    }
#pragma warning restore CA1720, CA1815, CS1591, CA1051






    /// <summary>
    /// Flags for the ShortTools.SDL.Renderer
    /// </summary>
    [Flags]
#pragma warning disable CA1711
    public enum RendererFlags : int
#pragma warning restore CA1711
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,

        /// <summary>
        /// Automatically calls RenderClear before the render action is called, and then RenderDraw is called after the render action.
        /// </summary>
        AutoRenderDraw = 1,


        /// <summary>
        /// Prints the internal ShortTools debug logs to the console.
        /// </summary>
        PrintDebugLogs = 2,


        /// <summary>
        /// Adds the key inputs to the debugger logs to allow you to see what buttons are working and what their names are.
        /// </summary>
        DebugKeyInputs = 4,
    }












    /// <summary>
    /// Renderer that will automatically create an SDL window and allow you to make a render 
    /// function that will be excecuted in a seperate thread.
    /// When creating this object, you should either do:
    /// <code> using Renderer renderer = new Renderer(); </code>
    /// or after finishing using it, call 
    /// <code> renderer.Dispose(); </code>
    /// as it contains unmanaged objects. 
    /// </summary>
    public sealed partial class Renderer : IDisposable
    {
        int screenwidth = -1;
        int screenheight = -1;


        /// <summary>
        /// Struct containing the SDL window pointer.
        /// </summary>
        private SDLWindow window;
        /// <summary>
        /// Struct containing the SDL renderer pointer.
        /// </summary>
        private SDLRenderer SDLrenderer;

        Thread? thread = null;
        bool running = true;
        /// <summary>
        /// Bool stating if the renderer is currently running.
        /// </summary>
        public bool Running { get => running; }
        bool setup = false;
        bool disposed = false;
        readonly RendererFlags flags = RendererFlags.None;
#pragma warning disable CA2213 // it is disposed, you are just silly.
        private Debugger? debugger = null;
#pragma warning restore CA2213


        private readonly Dictionary<string, IntPtr> images = new Dictionary<string, IntPtr>();



        private readonly Action<SDLRenderer, SDLWindow>? Render = null;
        private static void EmptyAction() { }
        private static void SleepAction(SDLRenderer SDLrenderer, SDLWindow window) { Thread.Sleep(50); }


        /// <inheritdoc cref="Renderer"/>
        public Renderer(Action<SDLRenderer, SDLWindow>? Render = null, Action<SDL_Keycode, bool>? Handle = null, RendererFlags flag = RendererFlags.None)
        {
            this.Render = Render ?? SleepAction;
            this.Handle = Handle ?? EmptyHandle;
            flags = flag;

            try
            {
                Setup();
            }
            catch (ShortException e)
            {
                debugger?.AddLog($"{e.Message} Error code = {e.ErrorCode}", WarningLevel.CriticalError);
                debugger?.Dispose();
            }
        }











        bool sleeping = false;
        int sleepTime = -1;
        /// <summary>
        /// Pauses the renderer thread, restart it with Resume or pass in a time for it to sleep for.
        /// </summary>
        /// <param name="sleepTime">The time it will sleep for, if it is set to -1, it will wait until resume is called.</param>
        public void Pause(int sleepTime = -1)
        {
            this.sleepTime = sleepTime;
            debugger?.AddLog("Thread paused.");
            sleeping = true;
        }
        /// <summary>
        /// Resumes the thread that runs the renderer after Pause has been called.
        /// </summary>
        public void Resume()
        {
            sleeping = false;
            thread?.Interrupt();
        }



        /// <summary>
        /// Starts the renderer, only use this once when you want the renderer to start up for the first time. For stopping and starting the renderer see Pause() and Resume().
        /// </summary>
        /// <exception cref="SDLError">thrown when SDL errors.</exception>
        public void Start()
        {
            if (disposed) 
            {
                debugger?.AddLog($"Attempted to start a disposed renderer", WarningLevel.Error);
                return;
            }


            thread = new Thread(new ThreadStart(() => Controller(this, Render ?? SleepAction, SDLrenderer, window)));
            thread.Start();

            debugger?.AddLog("Renderer started", WarningLevel.Info);
        }



        static void Controller(
            Renderer renderer, Action<SDLRenderer, SDLWindow> Render, 
            SDLRenderer SDLrenderer, SDLWindow window)
        {

            Action Actions = EmptyAction;


#pragma warning disable CA1806 // i am ignoring HResult as it is too intensive to check
            if (renderer.flags.HasFlag(RendererFlags.AutoRenderDraw)) { Actions += () => SDL_RenderClear(SDLrenderer.pointer); }
#pragma warning restore CA1806

            Actions += () => Render(SDLrenderer, window);

            if (renderer.flags.HasFlag(RendererFlags.AutoRenderDraw)) { Actions += () => SDL_RenderPresent(SDLrenderer.pointer); }




            renderer.debugger?.AddLog("Actions created", WarningLevel.Info);





            while (renderer.running)
            {
                renderer.HandleInputs();
                Actions();

                if (renderer.sleeping)
                {
                    try
                    {
                        Thread.Sleep(renderer.sleepTime);
                    }
                    catch (ThreadInterruptedException) { renderer.debugger?.AddLog("Thread restarted.", WarningLevel.Info); }
                    catch (ThreadAbortException) { renderer.debugger?.AddLog("Thread aborted, it cannot be restarted.", WarningLevel.Info); }
                    finally
                    {
                        renderer.debugger?.AddLog("Thread restarted.", WarningLevel.Info);
                        renderer.sleeping = false;
                    }
                }
            }



            renderer.debugger?.AddLog("Main loop stopped", WarningLevel.Info);
        }






        /// <summary>
        /// Wrapper for SDL_RenderClear to deal with errors
        /// </summary>
        /// <param name="renderer"></param>
        public void RenderClear(IntPtr renderer)
        {
            if (SDL_RenderClear(renderer).Failed())
            {
                debugger?.AddLog($"Error with clearing on the renderer, {SDL_GetError()}");
            }
        }





        





        SDL_Rect srcrect = new SDL_Rect() { x = 0, y = 0, w = 32, h = 32 };
        /// <summary>
        /// Edits the source rectangle used in the Draw function. If you only want to edit some of the values, E.G. w, you can do this
        /// <code>
        /// renderer.EditSrcRect(w: width, h: height);
        /// </code>
        /// </summary>
        /// <param name="x">The x position of the top left corner in pixels.</param>
        /// <param name="y">The y position of the top left corner in pixels.</param>
        /// <param name="w">The width in pixels.</param>
        /// <param name="h">The height in pixels.</param>
        public void EditSrcRect(int x = 0, int y = 0, int w = 32, int h = 32)
        {
            srcrect.x = x; srcrect.y = y; srcrect.h = h; srcrect.w = w;
        }


        /// <summary>
        /// Draws the image at the given x and y values (represtenting the top left of the image), at the angle given and flipped if told to.
        /// </summary>
        /// <param name="x">The x position of the top left corner in pixels.</param>
        /// <param name="y">The y position of the top left corner in pixels.</param>
        /// <param name="h">The height in pixels.</param>
        /// <param name="w">The width in pixels.</param>
        /// <param name="image">The path of the image to be drawn.</param>
        /// <param name="angle">The angle of the image to be drawn in degrees.</param>
        /// <param name="flip">An enum value represting if the image should be flipped horizontally or vertically.</param>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void Draw(int x, int y, int w, int h, string image, double angle = 0d, SDL_RendererFlip flip = SDL_RendererFlip.SDL_FLIP_NONE)
        {
            DrawImage(x, y, w, h, images[image], angle, flip);
        }
        /// <inheritdoc cref="Draw(int, int, int, int, string, double, SDL_RendererFlip)"/>
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
        public void Draw(int x, int y, int w, int h, IntPtr image, double angle = 0d, SDL_RendererFlip flip = SDL_RendererFlip.SDL_FLIP_NONE)
        {
            DrawImage(x, y, w, h, image, angle, flip);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private void DrawImage(int x, int y, int w, int h, IntPtr image, double angle, SDL_RendererFlip flip)
        {
            SDL_Rect dstrect = new SDL_Rect() { x = x, y = y, h = h, w = w };

            if (SDL_RenderCopyEx(SDLrenderer.pointer, image, ref srcrect, ref dstrect, angle, IntPtr.Zero, flip).Failed()) 
            { 
                throw new ShortException(ErrorCode.SDLError, "Error during SDL render copy."); 
            }
        }















        private void Setup()
        {
            if (setup) { return; }

            setup = true;





            if (this.flags.HasFlag(RendererFlags.DebugKeyInputs))
            {
                Handle += (SDL_Keycode code, bool down) =>
                {
                    if (down) { debugger?.AddLog(code.ToString(), WarningLevel.Debug); }
                };
            }





            if (this.flags.HasFlag(RendererFlags.PrintDebugLogs))
            {
                debugger = new Debugger("ShortTools Renderer", DebuggerFlag.ShortDefault);
            }
            else
            {
                debugger = new Debugger("ShortTools Renderer", DebuggerFlag.ShortDefault ^ DebuggerFlag.PrintLogs);
            }







            if (SDL_Init(SDL_INIT_EVERYTHING | SDL_INIT_SENSOR).Failed()) // returns 0 on success
            {
                throw new SDLError("SDL couldnt initialise, reason = " + SDL_GetError());
            }

            SDL_image.IMG_InitFlags flags = SDL_image.IMG_InitFlags.IMG_INIT_PNG;
            if ((int)flags != SDL_image.IMG_Init(flags)) // used to allow png loading
            {
                SDL_Quit();
                throw new SDLError("SDL couldnt initialise, reason = " + SDL_GetError());
            }



            // used to get the display width and height of the main monitor, so i can use that for renderering.
            if (SDL_GetCurrentDisplayMode(0, out SDL_DisplayMode displayMode).Failed())
            {

                // Fallback: Use SDL_GetDesktopDisplayMode if SDL_GetCurrentDisplayMode fails
                if (SDL_GetDesktopDisplayMode(0, out displayMode).Failed())
                {
                    SDL_Quit();
                    throw new SDLError("SDL couldnt initialise either display mode, reason = " + SDL_GetError());
                }
            }
            screenwidth = displayMode.w;
            screenheight = displayMode.h;



            window = new SDLWindow()
            {
                pointer = SDL_CreateWindow(
                "Window",
                SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, // Flags used to centre the window
                screenwidth, screenheight,
                SDL_WindowFlags.SDL_WINDOW_BORDERLESS)
            }; // Flags used to make the window borderless


            if (window.pointer == IntPtr.Zero)
            {
                SDL_Quit();
                throw new SDLError("SDL window couldnt initialise, reason = " + SDL_GetError());
            }

            SDLrenderer = new SDLRenderer()
            {
                pointer = SDL_CreateRenderer(window.pointer,
                                                    -1,
                                                    SDL_RendererFlags.SDL_RENDERER_ACCELERATED | // These flags make the screen capped at 60 FPS.
                                                    SDL_RendererFlags.SDL_RENDERER_PRESENTVSYNC)
            }
            ;

            if (SDLrenderer.pointer == IntPtr.Zero)
            {
                SDL_Quit();
                throw new SDLError("SDL renderer couldnt initialise, reason = " + SDL_GetError());
            }


            if (SDL_SetRenderDrawColor(SDLrenderer.pointer, 30, 0, 0, 255).Failed()) // Default draw colour is a deep red, so you can see if something has gone wrong. (returns 0 on success)
            {
                SDL_Quit();
                throw new SDLError("SDL draw colour couldnt initialise, reason = " + SDL_GetError());
            }


            if (!LoadImages() || images.Count == 0)
            {
                debugger.AddLog("Loading with no images", WarningLevel.Warning);
            }
        }








        /// <summary>
        /// 
        /// </summary>
        /// <returns>False on res not existing.</returns>
        bool LoadImages()
        {
            if (!Directory.Exists("res")) { return false; }

            Queue<string> toVisitFiles = new Queue<string>();

            toVisitFiles.Enqueue("res");

            while (toVisitFiles.Count != 0)
            {
                string file = toVisitFiles.Dequeue();

                foreach (string directory in Directory.GetDirectories(file))
                {
                    toVisitFiles.Enqueue(directory);
                }
                foreach (string filename in Directory.GetFiles(file))
                {
                    if (filename.Length <= 4) { continue; }

                    string filetype = filename.Substring(filename.Length - 4, 4);
                    if (filetype != ".png" && filetype != ".bmp") { continue; }

                    if (images.ContainsKey(filename)) { continue; }

                    images.Add(filename, PrivLoadImage(filename));
                }
            }

            return true;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        IntPtr PrivLoadImage(string path)
        {
            string fileType = path[^3..];

            if (fileType == "bmp")
            {
                IntPtr image = SDL_LoadBMP(path);
                if (image == IntPtr.Zero) { debugger?.AddLog($"Image {path} was not loaded correctly, reason = {SDL_GetError()}", WarningLevel.Error); }
                return image;
            }
            else if (fileType == "png")
            {
                IntPtr image = SDL_image.IMG_LoadTexture(SDLrenderer.pointer, path);
                if (image == IntPtr.Zero) { debugger?.AddLog($"Image {path} was not loaded correctly, reason = {SDL_GetError()}", WarningLevel.Error); }
                return image;
            }
            debugger?.AddLog($"Image {path} was not loaded correctly, please contact the package author with the file (and its path) that did not load.", WarningLevel.Error);
            return IntPtr.Zero;
        }

        /// <summary>
        /// Loads an image from the given path into the images dictionary and returns the pointer. If you do not want it to load into the image dictionary,
        /// call <see cref="LoadImageNoDict(string)">LoadImageNoDict</see>.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key">The key of the image in the images dictionary.</param>
        /// <returns></returns>
        public IntPtr LoadImage([NotNull] string path, string? key = null)
        {
            IntPtr image = PrivLoadImage(path);
            images.Add(key ?? path, image);
            return image;
        }
        /// <summary>
        /// Loads an image from the given path and returns the pointer to the image. To load the image into the image dictionary, call <see cref="LoadImage(string, string?)">LoadImage</see>.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public IntPtr LoadImageNoDict([NotNull] string path)
        {
            return PrivLoadImage(path);
        }









        /// <summary>
        /// If the operation fails, adds a log to the debugger. To not do this and improve speed, call <see cref="SetRenderColourFast(byte, byte, byte, byte)">SetRenderColourFast</see>.
        /// </summary>
        /// <param name="r">red value.</param>
        /// <param name="g">green value.</param>
        /// <param name="b">blue value.</param>
        /// <param name="a">alpha value.</param>
        /// <returns>True on success</returns>
        public bool SetRenderColour(byte r = 30, byte g = 0, byte b = 0, byte a = 255)
        {
            bool failed = SDL_SetRenderDrawColor(SDLrenderer.pointer, r, g, b, a).Failed();

            if (!failed) { return true; }

            debugger?.AddLog($"SDL error - {SDL_GetError()}", WarningLevel.Error);
            return false;
        }
        /// <summary>
        /// Same as <see cref="SetRenderColour(byte, byte, byte, byte)">SetRenderColour</see> but faster due to no error checking, use this if you need to squeeze out a little more performance.
        /// </summary>
        /// <param name="r">red value.</param>
        /// <param name="g">green value.</param>
        /// <param name="b">blue value.</param>
        /// <param name="a">alpha value.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void SetRenderColourFast(byte r = 30, byte g = 0, byte b = 0, byte a = 255)
        {
#pragma warning disable CA1806 // They know what they are getting into.
            SDL_SetRenderDrawColor(SDLrenderer.pointer, r, g, b, a);
#pragma warning restore CA1806
        }








        /// <summary>
        /// Disposes of the renderer, deleting the images and destroying the render thread.
        /// </summary>
        public void Dispose()
        {
            if (disposed) { return; }
            disposed = true;


            running = false;

            thread?.Join();

            debugger?.Dispose();

            // Deload images

            SDL_Quit();
        }
#pragma warning disable CA1063
        void IDisposable.Dispose()
        {
            GC.SuppressFinalize(this);

            Dispose();
        }
#pragma warning restore CA1063
    }



    

    /// <summary>
    /// An exception for when an issue with SDL occurs in a short tools based program.
    /// </summary>
    public class SDLError : ShortException
    {

        /// <inheritdoc cref="SDLError"/>
        /// <param name="message">The message to be displayed.</param>
        public SDLError(string message) : base(ShortTools.General.ErrorCode.SDLError, message) 
        { }

        /// <inheritdoc cref="SDLError"/>
        public SDLError() : base(ShortTools.General.ErrorCode.SDLError, "SDL caused a fatal error.")
        { }

        /// <inheritdoc cref="SDLError"/>
        /// <param name="message"><inheritdoc cref="SDLError(string)"/></param>
        /// <param name="innerException"></param>
        public SDLError(string message, Exception innerException) : base(ShortTools.General.ErrorCode.SDLError, message, innerException)
        { }
    }
}
