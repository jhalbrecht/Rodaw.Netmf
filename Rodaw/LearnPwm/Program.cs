using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace LearnPwm
{
    public class Program
    {
        static PWM MyFader = new PWM(Cpu.PWMChannel.PWM_3, 10000, 0.1, false);

        public static void Main()
        {



            {
                uint[] time = new uint[] { 500 * 1000, 500 * 1000 };
                //OutputCompare oc = new OutputCompare((Cpu.Pin)FEZ_Pin.Digital.Di2, false, 2);
                //oc.Set(false, time, 0, 2, true);//start the waveform

                //... do more code here and the LED will continue to work.
                //... it is non-blocking.
            }


            double i = 0.0;
            double dirr = 0.1;
            while (true)
            {
                MyFader.DutyCycle = i;
                MyFader.Start();

                i = (double)(i + dirr);

                if (i >= 0.9)
                    dirr = -0.1;
                if (i <= 0.1)
                    dirr = 0.1;

                Debug.Print(i.ToString());

                Thread.Sleep(10);
            }
        }

    }
}
