using System;
using System.Windows.Forms;
using GTA;
using SharpDX.DirectInput;

namespace JoystickSteering
{
    public class JoystickSteering : Script
    {
        private bool _firstTime = true;

        private DirectInput _directInput = new DirectInput();
        private Guid _joystickGuid = Guid.Empty;
        private Joystick _joystick;

        public JoystickSteering()
        {
            foreach (var deviceInstance in _directInput.GetDevices(DeviceType.Joystick, DeviceEnumerationFlags.AllDevices))
            {
                _joystickGuid = deviceInstance.InstanceGuid;
            }

            if (!(_joystickGuid == Guid.Empty))
            {
                _joystick = new Joystick(_directInput, _joystickGuid);
                _joystick.Properties.BufferSize = 128;
                _joystick.Acquire();
            }


            Tick += OnTick;
            KeyDown += OnKeyDown;
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (_firstTime)
            {
                if (_joystickGuid == Guid.Empty || _joystick == null)
                {
                    UI.Notify("No joystick found.", true);
                }
                else
                {
                    UI.Notify("Found joystick: " + _joystick.Properties.ProductName);
                }
                _firstTime = false;
            }
            else
            {
                if (!(_joystickGuid == Guid.Empty) && _joystick != null)
                {
                    _joystick.Poll();
                    var data = _joystick.GetBufferedData();
                    foreach (var state in data)
                    {
                        // Handle joystick data
                    }
                }
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.K)
            {
                if (!(_joystickGuid == Guid.Empty) && _joystick != null)
                {
                    UI.ShowSubtitle("Using device: " + _joystick.Properties.ProductName);
                }
                else
                {
                    UI.ShowSubtitle("No joystick found.");
                }
            }

        }
    }
}