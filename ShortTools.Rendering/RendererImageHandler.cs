using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 

namespace ShortTools.Rendering
{
    
    public abstract partial class GraphicsHandler
    {
        IntPtr PrivLoadImage(string path)
        {
            string fileType = path[^3..];

            sdl.GameControllerAddMappingsFromRW
            
            if (fileType == "bmp")
            {

                sdl.rw
                var rw = sdl.RWFromFile();
                IntPtr image = sdl.SDL_LoadBMP(path);
                if (image == IntPtr.Zero) { debugger?.AddLog($"Image {path} was not loaded correctly, reason = {SDL_GetError()}", WarningLevel.Error); }
                return image;
            }
            else if (fileType == "png")
            {
                IntPtr image = SDL_image.IMG_LoadTexture(renderer.pointer, path);
                if (image == IntPtr.Zero) { debugger?.AddLog($"Image {path} was not loaded correctly, reason = {SDL_GetError()}", WarningLevel.Error); }
                return image;
            }
            debugger?.AddLog($"Image {path} was not loaded correctly, please contact the package author with the file (and its path) that did not load.", WarningLevel.Error);
            return IntPtr.Zero;
        }
    }
}
