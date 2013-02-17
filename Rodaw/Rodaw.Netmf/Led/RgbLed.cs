using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Rodaw.Netmf.Led
{
// jha 2/14/2013 Cleaned up library. Refactored with upperCase

    public enum RgbLedColors
    {
        Red, Green, Blue, None, Yellow, Cyan, Magenta, White
    }

    public class RgbLed
    // think, think, think RbgLed or ThreeColorLed think, think, think
    // changed naming from ThreeColoredLED per coding standards. Hmmm....
    // create a three color led object. Only one color, or none, can be on at any one time.
    {
        private RgbLedColors _color;
        private OutputPort _red, _green, _blue;

        public RgbLed(OutputPort red, OutputPort green, OutputPort blue)
        {
            _red = red;       // this seems cumbersome, not elegant. Code review?
            _green = green;
            _blue = blue;
        }

        // jha 11/29/2011
        public RgbLed()
        {
        }


        public bool BlinkMode { get; set; }
        public int BlinkDuration { get; set; } // TODO need to set a default in case it's not assigned in code. 
        public bool LedOnState { get; set; }

        public RgbLedColors Color
        {
            get
            {
                return this._color;
            }

            set
            {
                this._color = value;

                // turn off all LED colors in this common Anode LED.
                _red.Write(!LedOnState);
                _green.Write(!LedOnState);
                _blue.Write(!LedOnState);

                // Now turn on just one color or leave all off in the case of LEDColors.none
                // currently using a common annode so false enables the LED. 
                switch (_color)
                {
                    case RgbLedColors.Red:
                        _red.Write(LedOnState);
                        if (BlinkMode)
                        {
                            Thread.Sleep(BlinkDuration);
                            _red.Write(!LedOnState);
                        }
                        break;

                    case RgbLedColors.Green:
                        _green.Write(LedOnState);
                        if (BlinkMode)
                        {
                            Thread.Sleep(BlinkDuration);
                            _green.Write(!LedOnState);
                        }
                        break;

                    case RgbLedColors.Blue:
                        _blue.Write(LedOnState);
                        if (BlinkMode)
                        {
                            Thread.Sleep(BlinkDuration);
                            _blue.Write(!LedOnState);
                        }
                        break;

                    case RgbLedColors.Yellow:
                        {
                            _red.Write(LedOnState);
                            _green.Write(LedOnState);
                            if (BlinkMode)
                            {
                                Thread.Sleep(BlinkDuration);
                                _red.Write(!LedOnState);
                                _green.Write(!LedOnState);
                            }
                        }
                        break;

                    case RgbLedColors.Cyan:
                        {
                            _green.Write(LedOnState);
                            _blue.Write(LedOnState);
                            if (BlinkMode)
                            {
                                Thread.Sleep(BlinkDuration);
                                _green.Write(!LedOnState);
                                _blue.Write(!LedOnState);
                            }
                        }
                        break;

                    case RgbLedColors.Magenta:
                        {
                            _red.Write(LedOnState);
                            _blue.Write(LedOnState);
                            if (BlinkMode)
                            {
                                Thread.Sleep(BlinkDuration);
                                _red.Write(!LedOnState);
                                _blue.Write(!LedOnState);
                            }
                        }
                        break;

                    case RgbLedColors.White:
                        {
                            _red.Write(LedOnState);
                            _green.Write(LedOnState);
                            _blue.Write(LedOnState);

                            if (BlinkMode)
                            {
                                Thread.Sleep(BlinkDuration);
                                _red.Write(!LedOnState);
                                _green.Write(!LedOnState);
                                _blue.Write(!LedOnState);
                            }
                        }
                        break;

                    case RgbLedColors.None:            // none. Leave them all off
                        break;

                    default:                        // should never get here. Leave them off.
                        break;
                }
            }
        }
    }
}
