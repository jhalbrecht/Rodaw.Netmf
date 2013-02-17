using System;
using System.Threading;
using Microsoft.SPOT;

using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Rodaw.Netmf.Led
{
    // jha  2/14/2013 Cleaned up library. Refactored with upperCase

    public enum SingleLedModes 
    {
        On, Off, Blink
    }

    public class SingleLed 
    {
        private SingleLedModes _mode;
        private bool _ledState;
        private OutputPort _led;

        public SingleLed(OutputPort led)
        {
            _led = led;
            LedOnState = true;                         // Default ledOnState to true in case not initialized in calling code
            // _ledState = true; // _led.InitialState;
            //_ledState = _led.InitialState;
            // _led.Write(_ledState);
            _led.Write(_ledState = _led.InitialState);  // set and write the startup ledState from OutputPort led constructor

            // ledState = true; 
        }

        public bool BlinkMode
        { get; set; }

        public int BlinkDuration
        { get; set; } // TODO need to set a default in case it's not assigned in code. 

        public bool LedOnState { get; set; }

        public bool LedState
        {
            get
            {
                return this._ledState;
            }
            set
            {
                this._ledState = value;
                if (_ledState)
                {
                    _led.Write(LedOnState);
                }
                else
                {
                    _led.Write(!LedOnState);
                }
            }
        }

        public SingleLedModes Mode
        {
            get
            {
                return this.Mode;
            }
            set
            {
                this._mode = value;

                switch (_mode)
                {
                    case SingleLedModes.On:
                        _led.Write(LedOnState);
                        break;

                    case SingleLedModes.Off:
                        _led.Write(!LedOnState);
                        break;

                    case SingleLedModes.Blink:
                        _led.Write(LedOnState);
                        Thread.Sleep(BlinkDuration);
                        _led.Write(!LedOnState);
                        break;
                }
            }
        }
    }
}
