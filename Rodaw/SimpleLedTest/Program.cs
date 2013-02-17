using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Rodaw.Netmf.Led;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace SimpleLedTest
{
    public class Program
    {
        static SingleLed singleLed0 = new SingleLed(new OutputPort(Pins.ONBOARD_LED, true));

        static RgbLed rgbLed0 =
            new RgbLed(new OutputPort(Pins.GPIO_PIN_D0, false),
                new OutputPort(Pins.GPIO_PIN_D1, false),
                new OutputPort(Pins.GPIO_PIN_D2, false));

        static Random randomNumber = new Random();
        private const int pauseTime = 250;

        public static void Main()
        {
            // OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);

            singleLed0.Mode = SingleLedModes.Blink;
            singleLed0.LedOnState = true;
            singleLed0.BlinkDuration = 250;
            Thread led0Thread = new Thread(Led0Thread);
            led0Thread.Start();

            rgbLed0.BlinkMode = false;
            rgbLed0.LedOnState = false;
            rgbLed0.BlinkDuration = 75;
            Thread a = new Thread(RgbLed0Thread);
            a.Start();

            Thread.Sleep(Timeout.Infinite);

            //while (true)
            //{
            //    Thread led0Thread = new Thread(Led0Thread);
            //    led0Thread.Start();


            //    // singleLed0.mode(SingleLedModes.on);
            //    // led.Write(true);
            //    Thread.Sleep(250);
            //    // led.Write(false);
            //    Thread.Sleep(250);
            //}
            //return;
        }

        private static void Led0Thread()
        {
            while (true)
            {
                singleLed0.Mode = RandomSingleLedMode();
                Thread.Sleep(randomNumber.Next(pauseTime * 2));
            }
        }

        static void RgbLed0Thread()
        {
            while (true)
            {
                rgbLed0.Color = RandomRgbLedColor();
                Thread.Sleep(randomNumber.Next(pauseTime));
            }
        }

        static SingleLedModes RandomSingleLedMode()
        {
            int a = randomNumber.Next(2);
            return (SingleLedModes)a;
        }

        static RgbLedColors RandomRgbLedColor()
        {
            int a = randomNumber.Next(9); // TODO learn to replace 4 with number of enumerations +1 more elegant like :-)
            return (RgbLedColors)a;
        }
    }
}
