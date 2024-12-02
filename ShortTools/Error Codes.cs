using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortTools.General
{
    /// <summary>
    /// Error codes used throughout ShortTools.
    /// </summary>
    public enum ErrorCode : int
    {
        /// <summary>
        /// The program completed successfully
        /// </summary>
        Success = 0,

        //             v - digit for the type of program that caused it. 
        //  Test = 0x00100000



        #region SDL
        /// <summary>
        /// Error caused by the SDL framework.
        /// </summary>
        SDLError = 0x00500001,
        #endregion



        #region Settings

        

        #endregion
    }
}
