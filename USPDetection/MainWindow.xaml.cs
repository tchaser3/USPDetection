using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Management;
using System.Timers;

namespace USPDetection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class USBDeviceInfo
        {
            private static System.Timers.Timer aTimer = new Timer;

            public USBDeviceInfo(string deviceID, string pnpDeviceID, string description)
            {
                this.DeviceID = deviceID;
                this.PnpDeviceID = pnpDeviceID;
                this.Description = description;
            }
            public string DeviceID { get; private set; }
            public string PnpDeviceID { get; private set; }
            public string Description { get; private set; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }


        private void btnFindDrives_Click(object sender, RoutedEventArgs e)
        {
            var usbDevices = GetUSBDevices();
            string strDrives = "";
            string strDeviceID;
            string strDescription;

            foreach (var usbDevice in usbDevices)
            {
                strDeviceID = Convert.ToString(usbDevice.DeviceID);
                strDescription = Convert.ToString(usbDevice.Description);

                if(strDeviceID.Contains("USB"))
                {
                    if(strDescription.Contains("Input Device") == false)
                    {
                        if(strDescription.Contains("USB Hub") == false)
                        {
                            if(strDescription.Contains("USB Root Hub") == false)
                            {
                                strDrives += strDeviceID + "\t" + strDescription;
                                strDrives += "\n\n";
                            }
                            
                        }
                        
                    }
                }
            }

            txtDrives.Text = strDrives;
        }
        static List<USBDeviceInfo> GetUSBDevices()
        {
            List<USBDeviceInfo> devices = new List<USBDeviceInfo>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnpEntity"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                devices.Add(new USBDeviceInfo(
                (string)device.GetPropertyValue("DeviceID"),
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();
            return devices;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtDrives.Text = "";
        }
    }
}
