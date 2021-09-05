using System;
using System.Collections.Generic;
using System.Threading;

namespace Mycobot.csharp
{
    class Test 
    {
        static List<int[]> pose = new List<int[]>() { };
        static void Main(string[] args)
        {
            using (MyCobot mc = new MyCobot("COM5"))
            {
                mc.Open();
                Thread.Sleep(5000);
                mc.PowerOn();

                Console.WriteLine($"System Version: {mc.GetSystemVersion()}");
                Console.WriteLine($"Robot Version : {mc.GetRobotVersion()}");
                // Get current pose & Initialize
                var init = mc.GetAngles();
                for (int i = 0; i < 3; i++)
                {
                    var data = new int[6];
                    Array.Copy(init, data, 6);
                    pose.Add(data);
                }
                while (mc.GetCoords().Length==0){
                    // wait for first coords data received
                }

                // set LED color
                Random random = new Random();
                mc.SetColor((byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255));
                Thread.Sleep(100);

                bool stop = false;
                byte duty = 128;
                while (!stop)
                {
                    var angles = mc.GetAngles();
                    var coords = mc.GetCoords();

                    if (Console.KeyAvailable)
                    {
                        var a = Console.ReadKey();
                        switch (a.Key)
                        {
                            // Save current pose
                            case ConsoleKey.D1: // Key '1'
                                SaveAngles(mc, 0);
                                break;
                            case ConsoleKey.D2:
                                SaveAngles(mc, 1);
                                break;
                            case ConsoleKey.D3:
                                SaveAngles(mc, 2);
                                break;

                            // Load a pose
                            case ConsoleKey.D4:
                                LoadAngles(mc, 0);
                                break;
                            case ConsoleKey.D5:
                                LoadAngles(mc, 1);
                                break;
                            case ConsoleKey.D6:
                                LoadAngles(mc, 2);
                                break;
 
                            // Release All Servos
                            case ConsoleKey.R:
                                mc.ReleaseAllServos();
                                break;

                            // Suction pump controll
                            // https://www.elephantrobotics.com/docs/myCobot-en/3-development/7-side_products/7.1.2-sunction_pump.html
                            case ConsoleKey.N:
                                mc.SetBasicDigitalOutput(2, true);
                                Thread.Sleep(100);
                                mc.SetBasicDigitalOutput(5, true);
                                break;
                            case ConsoleKey.M:
                                mc.SetBasicDigitalOutput(2, false);
                                Thread.Sleep(100);
                                mc.SetBasicDigitalOutput(5, false);
                                break;

                            case ConsoleKey.D:
                                Console.WriteLine($"Duty: {100 * duty / 256} %");
                                mc.SetPwmOutput(23, 1000, duty);
                                duty = (byte)((duty + 10) & 0xFF);
                                break;

                            // Exit
                            case ConsoleKey.Escape:
                                stop = true;
                                continue;
                        }
                    }
                    Console.WriteLine($"Servo 1: {mc.IsServoEnable(1)}");
                    Console.WriteLine($"All Servo: {mc.IsAllServoEnable()}");
                    foreach (var a in angles)
                    {
                        Console.Write($"{a} ");
                    }
                    Console.Write("\t");
                    foreach (var v in coords)
                    {
                        Console.Write($"{v} ");
                    }
                    Console.WriteLine();
                }
                Thread.Sleep(1000);
                mc.Close();
            }
        }
        static void SaveAngles(MyCobot mc, int key)
        {
            var angles = mc.GetAngles();
            pose[key] = angles;
            Console.WriteLine($">> {angles[0]},{angles[1]},{angles[2]}");
            mc.SendAngles(angles, 100);
        }
        static void LoadAngles(MyCobot mc, int key)
        {
            var angles = pose[key];
            Console.WriteLine($">> {angles[0]},{angles[1]},{angles[2]}");
            mc.SendAngles(angles, 100);
        }
    }
}