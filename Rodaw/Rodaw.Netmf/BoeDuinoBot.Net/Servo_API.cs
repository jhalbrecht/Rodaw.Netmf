/* Servo NETMF Driver
*      Coded by Chris Seto August 2010
*      <chris@chrisseto.com> 
*      http://forums.netduino.com/index.php?/topic/160-netduino-servo-class/
*      
* Use this code for whatveer you want. Modify it, redistribute it, I don't care.
* I do ask that you please keep this header intact, however.
* If you modfy the driver, please include your contribution below:
* 
* Chris Seto: Inital release (1.0)
* Chris Seto: Netduino port (1.0 -> Netduino branch)
* Chris Seto: bool pin state fix (1.1 -> Netduino branch)
* 
* November 6, 2011 Jeff Albrecht @jhalbrecht 
*  Changes denoted by: "// jha v1.2"
*  Added 
*   Speed for continuous rotation servos
*   Speed range
*   getter for getRange
* 
* 
* */

using System;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;

namespace Servo_API
{
    public class Servo : IDisposable
    {

        /// <summary>
        /// PWM handle
        /// </summary>
        private PWM servo;

        /// <summary>
        /// Timings range
        /// </summary>
        private int[] range = new int[2];

        // jha for Speed range v1.2
        private int reverseRange;
        private int forwardRange;

        /// <summary>
        /// Set servo inversion
        /// </summary>
        public bool inverted = false;

        /// <summary>
        /// Create the PWM pin, set it low and configure timings
        /// </summary>
        /// <param name="pin"></param>
        public Servo(Cpu.Pin pin)
        {
            // Init the PWM pin
            // servo = new PWM((Cpu.Pin)pin);
            
            servo = new PWM((Cpu.PWMChannel)pin, 10000, 0.1, false);
            servo.Period((uint)0); 
            servo.SetDutyCycle(0);

            // Typical settings
            range[0] = 1000;
            range[1] = 2000;

            // jha v1.2
            forwardRange = 100;
            reverseRange = -100;
        }

        public void Dispose()
        {
            disengage();
            servo.Dispose();
        }

        /// <summary>
        /// Allow the user to set custom timings
        /// </summary>
        /// <param name="fullLeft"></param>
        /// <param name="fullRight"></param>
        public void setRange(int fullLeft, int fullRight)
        {
            range[1] = fullLeft;
            range[0] = fullRight;
        }

        // jha v1.2
        public int[] getRange
        {
            get { return range; }
        }

        // TODO jha add setSpeedRange(...)

        /// <summary>
        /// Disengage the servo. 
        /// The servo motor will stop trying to maintain an angle
        /// </summary>
        public void disengage()
        {
            // See what the Netduino team say about this... 
            servo.SetDutyCycle(0);
        }

        /// <summary>
        /// Set the servo degree
        /// </summary>

        private double _Degree;
        public double Degree
        {
            get { return this._Degree; } // jha v1.2
            set                // TODO: Shouldn't I check for null ? 
            {
                /// Range checks
                if (value > 180)
                    value = 180;

                if (value < 0)
                    value = 0;

                // Are we inverted?
                if (inverted)
                    value = 180 - value;

                this._Degree = value;

                // Set the pulse
                servo.SetPulse(20000, (uint)Map((long)value, 0, 180, range[0], range[1]));
            }
        }

        // jha November 5, 2011 For continuous rotation servos     
        /// <summary>
        /// Set the servo speed. Check speed range denoted by forwardRange and reverseRange
        /// </summary>

        private double _speed;
        public double Speed
        {
            get { return this._speed; }

            set
            {
                /// Speed checks
                if (value > forwardRange)
                    value = forwardRange;

                if (value < reverseRange)
                    value = reverseRange;

                // Are we inverted?
                if (inverted)
                    // value = 180 - value;
                    value = value * -1;

                _speed = value;
                // Set the pulse
                servo.SetPulse(20000, (uint)Map((long)value, reverseRange, forwardRange, range[0], range[1]));
            }
        }

        // SetPwmValue() came from the following which are some modifications of the original Chris Seto code. 
        // http://geekswithblogs.net/kobush/archive/2010/08/31/netduino-controlled-servo-robot.aspx
        // http://forums.netduino.com/index.php?/topic/160-netduino-servo-class/

        // jha adding a GetPwmValue
        // ok now wait a minute.... Serb is setting a degree in PwmValue. That's misleading. I can't seem to get to the PWM pulsewidth so I'll put it here. 
        private uint _PulseWidth;
        public uint PulseWidth
        {
            get
            {
                return this._PulseWidth;
            }
            set
            {
                this._PulseWidth = value;
            }
        }

        public void SetPulseWidth(uint pulse)
        {
            // Typical settings
            // range[0] = 1000;
            // range[1] = 2000;

            // TODO: should I do a range check? I'd rather impose the range on the calling code. I don't know how to do that. 


            // Range checks 
            //if (value > 180)
            //    value = 180;
            //    PwmValue = value;

            //if (value < 0)
            //    value = 0;
            //    PwmValue = value;

            //// Are we inverted? 
            //if (inverted)
            //    value = 180 - value;


            // Set the pulse 
            //servo.SetPulse(20000, (uint)Map((long)value, 0, 180, range[0], range[1]));

            PulseWidth = pulse;
            // _PwmValue = value;
            servo.SetPulse(20000, pulse);
        }

        /// <summary>
        /// Used internally to map a value of one scale to another
        /// </summary>
        /// <param name="x"></param>
        /// <param name="in_min"></param>
        /// <param name="in_max"></param>
        /// <param name="out_min"></param>
        /// <param name="out_max"></param>
        /// <returns></returns>
        private long Map(long x, long in_min, long in_max, long out_min, long out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }
    }
}