using ShortTools.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SDL2.SDL;
using static ShortTools.General.Prints;

namespace ShortTools.SDL
{
    public sealed partial class Renderer : IDisposable
    {
        private Action<SDL_Keycode, bool> Handle = EmptyHandle;

        private static void EmptyHandle(SDL_Keycode a, bool b) { }




        private void HandleInputs()
        {
            while (SDL_PollEvent(out SDL_Event e) == 1)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT: // ensures that quitting works and runs cleanup code
                        Dispose();
                        break;


                    case SDL_EventType.SDL_KEYDOWN: // when pressed, it calls the handler when the input is down.
                        Handle(e.key.keysym.sym, true);
                        break;

                    case SDL_EventType.SDL_KEYUP: // when pressed, it calls the handler when the input is released.
                        Handle(e.key.keysym.sym, false);
                        break;
                }
            }
        }
    }
}
