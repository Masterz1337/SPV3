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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using HXE;
using SPV3.Annotations;

namespace SPV3
{
  public partial class Configuration
  {
    public class ConfigurationOpenSauce : INotifyPropertyChanged
    {
      private bool   _bloom;
      private bool   _detailedMaps;
      private double _fieldOfView;
      private bool   _normalMaps;
      private bool   _specularLighting;
      private bool   _specularMaps;

      public OpenSauce Configuration { get; } = (OpenSauce) global::HXE.Paths.Custom.OpenSauce(Paths.Directory);

      public bool NormalMaps
      {
        get => _normalMaps;
        set
        {
          if (value == _normalMaps) return;
          _normalMaps = value;
          OnPropertyChanged();
        }
      }

      public bool DetailedMaps
      {
        get => _detailedMaps;
        set
        {
          if (value == _detailedMaps) return;
          _detailedMaps = value;
          OnPropertyChanged();
        }
      }

      public bool SpecularMaps
      {
        get => _specularMaps;
        set
        {
          if (value == _specularMaps) return;
          _specularMaps = value;
          OnPropertyChanged();
        }
      }

      public bool SpecularLighting
      {
        get => _specularLighting;
        set
        {
          if (value == _specularLighting) return;
          _specularLighting = value;
          OnPropertyChanged();
        }
      }

      public bool Bloom
      {
        get => _bloom;
        set
        {
          if (value == _bloom) return;
          _bloom = value;
          OnPropertyChanged();
        }
      }

      public double FieldOfView
      {
        get => _fieldOfView;
        set
        {
          if (value.Equals(_fieldOfView)) return;
          _fieldOfView = value;
          OnPropertyChanged();
        }
      }

      public event PropertyChangedEventHandler PropertyChanged;

      public void Save()
      {
        if (Configuration.Exists())
          Configuration.Load();

        Configuration.Rasterizer.ShaderExtensions.Object.NormalMaps       = NormalMaps;
        Configuration.Rasterizer.ShaderExtensions.Object.DetailNormalMaps = DetailedMaps;
        Configuration.Rasterizer.ShaderExtensions.Object.SpecularMaps     = SpecularMaps;
        Configuration.Rasterizer.ShaderExtensions.Object.SpecularLighting = SpecularLighting;
        Configuration.Rasterizer.PostProcessing.Bloom.Enabled             = Bloom;
        Configuration.Camera.FieldOfView                                  = FieldOfView;
        Configuration.Save();
      }

      public void Load()
      {
        if (!Configuration.Exists()) return;

        Configuration.Load();
        NormalMaps       = Configuration.Rasterizer.ShaderExtensions.Object.NormalMaps;
        DetailedMaps     = Configuration.Rasterizer.ShaderExtensions.Object.DetailNormalMaps;
        SpecularMaps     = Configuration.Rasterizer.ShaderExtensions.Object.SpecularMaps;
        SpecularLighting = Configuration.Rasterizer.ShaderExtensions.Object.SpecularLighting;
        Bloom            = Configuration.Rasterizer.PostProcessing.Bloom.Enabled;
        FieldOfView      = Configuration.Camera.FieldOfView;
      }

      [NotifyPropertyChangedInvocator]
      protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
      {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      }

      public void CalculateFOV()
      {
        Configuration.Camera.CalculateFOV();
        FieldOfView = Configuration.Camera.FieldOfView;
      }
    }
  }
}