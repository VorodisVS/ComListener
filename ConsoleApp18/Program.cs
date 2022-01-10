using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace ComListener
{

 
    class DataProvider
    {
        private readonly SerialPort _serialPort;
        public EventHandler MessageReceived;

        private int _currentIndex;
        private byte[] _buffer;

        private int state = 0;

        public DataProvider(DataProviderSettings settings)
        {
            _serialPort = new SerialPort(settings.PortName, settings.BaudRate);
            _buffer = new byte[15];
        }

        public void StartPolling()
        {
            _serialPort.Open();
            _serialPort.DataReceived += SerialPortDataReceived;
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // Здесь надо будет сделать обработку принятия байта и накопить посылку из 15 байт. После этого дернуть MessageReceived.
            if (_serialPort.BytesToRead >= _buffer.Length)
            {
                var firstByte = _serialPort.ReadByte();
            }
        }

        public void StopPolling()
        {
            _serialPort.Close();
        }
    }

    public class DataProviderSettings
    {
        public int BaudRate { get; set; }
        public string PortName { get; set; }

    }

    public interface ISettingsProvider
    {
        DataProviderSettings GetSettingsFromCmdArgs(string[] args);
    }
    public class SettingsProvider : ISettingsProvider
    {
        public DataProviderSettings GetSettingsFromCmdArgs(string[] args)
        {
            return new DataProviderSettings();
        }
    }

    class Program1
    {
        static void Main(string[] args)
        {
            ISettingsProvider settingsProvider = new SettingsProvider();
            var settings = settingsProvider.GetSettingsFromCmdArgs(args);

            DataProvider provider = new DataProvider(settings);
            provider.MessageReceived += Handler;
            provider.StartPolling();


            Console.ReadLine();
            provider.StopPolling();
        }

        private static void Handler(object o, EventArgs e)
        {
            Console.WriteLine($"Here we write data {o} - {e}");
        }
    }
    
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
