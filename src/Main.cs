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

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using HXE;
using SPV3.Annotations;
using static System.IO.File;

namespace SPV3
{
  public partial class Main
  {
    public MainError   Error   { get; set; } = new MainError();   /* catches & shows exceptions   */
    public MainInstall Install { get; set; } = new MainInstall(); /* checks & allows installation */
    public MainLoad    Load    { get; set; } = new MainLoad();    /* checks & allows loading      */
    public MainAssets  Assets  { get; set; } = new MainAssets();  /* permits SPV3 assets update   */

    /// <summary>
    ///   Wrapper for subclass initialisation methods.
    /// </summary>
    public void Initialise()
    {
      Directory.CreateDirectory(Paths.Directory);     /* create data directory */
      Directory.CreateDirectory(HXE.Paths.Directory); /* create hxe directory  */

      /**
       * We determine installation or initiation mode:
       * 
       * -   initiation: The HCE executable exists, thus SPV3 is ready to be loaded.
       * -   installation: The manifest exists, thus SPV3 is ready to be installed.
       *
       * If neither of the above apply in this scenario, then we prohibit loading or installing; instead, we prompt the
       * user to place the loader in the current directory.
       */

      switch (Context.Infer())
      {
        case Context.Type.Load:
          Load.Visibility = Visibility.Visible;
          Assets.Initialise();
          break;
        case Context.Type.Install:
          Install.Visibility = Visibility.Visible;
          break;
        case Context.Type.Invalid:
          Error.Visibility = Visibility.Visible;
          Error.Content    = "Please ensure this loader is in the appropriate SPV3 folder.";
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    ///   Wrapper for the load routine with UI support.
    /// </summary>
    public void Invoke()
    {
      try
      {
        Load.Invoke();
      }
      catch (Exception e)
      {
        Exception(e, "Loading error");
      }
    }

    /// <summary>
    ///   Wrapper for the asset update routine with UI support.
    /// </summary>
    public void Update()
    {
      try
      {
        Assets.Update();
      }
      catch (Exception e)
      {
        Exception(e, "Update error");
      }
    }

    /// <summary>
    ///   Successfully exits the SPV3 loader.
    /// </summary>
    public void Quit()
    {
      Environment.Exit(0);
    }

    private void Exception(Exception e, string description)
    {
      WriteAllText(Paths.Exception, e.ToString());

      Error.Visibility = Visibility.Visible;
      Error.Content    = $"{description}: {e.Message.ToLower()}\n\nClick here for more information.";
    }

    public class MainAssets : INotifyPropertyChanged
    {
      private const    string Address = "https://raw.githubusercontent.com/yumiris/SPV3/meta/update.hxe";
      private readonly Update _update = new Update();

      private Visibility _visibility;

      public Visibility Visibility
      {
        get => _visibility;
        set
        {
          if (value == _visibility) return;
          _visibility = value;
          OnPropertyChanged();
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      public void Initialise()
      {
        try
        {
          _update.Import(Address);
          Visibility = Visibility.Visible;
        }
        catch (Exception)
        {
          Visibility = Visibility.Collapsed;
        }
      }

      public void Update()
      {
        _update.Commit();
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }
    }
  }
}