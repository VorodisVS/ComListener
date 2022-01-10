using System;
using System.IO.Ports;
using System.Threading;

namespace ComListener
{
    class Program
    {
        static SerialPort _serialPort;

        static void Main(string[] args)
        {
            int buferSize;
            _serialPort = new SerialPort
            {
                PortName = SetPortName("COM7"),
                BaudRate = 9600,
                DataBits = 8,
                StopBits = StopBits.One,
                Parity = Parity.None,
                ReadTimeout = 1000
            };
            _serialPort.Open();
            while (true)
            {
                buferSize = _serialPort.BytesToRead;
                if (buferSize != 0 && _serialPort.IsOpen)
                {
                    Console.WriteLine(buferSize);
                    for (int i = 0; i < buferSize; ++i)
                    {
                        byte bt = (byte)_serialPort.ReadByte();
                        Console.WriteLine(bt);
                    }
                }
            }
        }

        public static string SetPortName(string defaultPortName)
        {
            string portName;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                Console.WriteLine("{0}", s);
            }

            Console.Write("Enter COM port value (Default: {0}): ", defaultPortName);
            portName = Console.ReadLine();

            if (portName == "" || !(portName.ToLower()).StartsWith("com"))
            {
                portName = defaultPortName;
            }
            return portName;
        }
    }
}
