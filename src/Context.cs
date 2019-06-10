/**
 * Copyright (c) 2019 Emilian Roman
 * 
 * This software is provided 'as-is', without any express or implied
 * warranty. In no event will the authors be held liable for any damages
 * arising from the use of this software.
 * 
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 * 
 * 1. The origin of this software must not be misrepresented; you must not
 *    claim that you wrote the original software. If you use this software
 *    in a product, an acknowledgment in the product documentation would be
 *    appreciated but is not required.
 * 2. Altered source versions must be plainly marked as such, and must not be
 *    misrepresented as being the original software.
 * 3. This notice may not be removed or altered from any source distribution.
 */

using System.IO;
using static System.Environment;
using static System.IO.File;
using static HXE.Paths;
using static HXE.Paths.HCE;

namespace SPV3
{
  /// <summary>
  ///   Module for determining the scenario of the loader environment.
  /// </summary>
  public class Context
  {
    /// <summary>
    ///   Possible environment states.
    /// </summary>
    public enum Type
    {
      Load,    /* hce executable exists in the current directory  */
      Install, /* hce executable doesn't exist, but manifest does */
      Invalid  /* neither the hce executable nor manifest exist   */
    }

    /// <summary>
    ///   Determines the environment context.
    /// </summary>
    /// <returns>
    ///   <see cref="Type" />
    /// </returns>
    public static Type Infer()
    {
      if (Exists(Path.Combine(CurrentDirectory, Executable)))
        return Type.Load;

      if (Exists(Path.Combine(CurrentDirectory, "data", Manifest)))
        return Type.Install;

      return Type.Invalid;
    }
  }
}