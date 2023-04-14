

    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    [Description("Класс для изменения микшера громкости windows")]
    public class AudioEndpoint
    {
        [ComImport]
        [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
        internal class MMDeviceEnumerator { }

        [Flags]
        internal enum EDataFlow
        {
            eRender,
            eCapture,
            eAll,
            EDataFlow_enum_count
        }
        [Flags]
        internal enum ERole
        {
            eConsole,
            eMultimedia,
            eCommunications,
            ERole_enum_count
        }

        [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IMMDeviceEnumerator
        {
            int NotImpl1();
            [PreserveSig]
            int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppDevice);
        }

        [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IMMDevice
        {
            [PreserveSig]
            int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);
        }

        [Guid("657804FA-D6AD-4496-8A60-352752AF4F89"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IAudioEndpointVolumeCallback
        {
            int OnNotify(IntPtr pNotifyData);
        };

        [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IAudioEndpointVolume
        {
            int RegisterControlChangeNotify(IAudioEndpointVolumeCallback pNotify);
            int UnregisterControlChangeNotify(IAudioEndpointVolumeCallback pNotify);
            int GetChannelCount(out int pnChannelCount);
            int SetMasterVolumeLevel(float fLevelDB, Guid pguidEventContext);
            int SetMasterVolumeLevelScalar(float fLevel, Guid pguidEventContext);
            int GetMasterVolumeLevel(out float pfLevelDB);
            int GetMasterVolumeLevelScalar(out float pfLevel);
            int SetChannelVolumeLevel(uint nChannel, float fLevelDB, Guid pguidEventContext);
            int SetChannelVolumeLevelScalar(uint nChannel, float fLevel, Guid pguidEventContext);
            int GetChannelVolumeLevel(uint nChannel, out float pfLevelDB);
            int GetChannelVolumeLevelScalar(uint nChannel, out float pfLevel);
            int SetMute([MarshalAs(UnmanagedType.Bool)] Boolean bMute, Guid pguidEventContext);
            int GetMute(out bool pbMute);
            int GetVolumeStepInfo(out uint pnStep, out uint pnStepCount);
            int VolumeStepUp(Guid pguidEventContext);
            int VolumeStepDown(Guid pguidEventContext);
            int QueryHardwareSupport(out uint pdwHardwareSupportMask);
            int GetVolumeRange(out float pflVolumeMindB, out float pflVolumeMaxdB, out float pflVolumeIncrementdB);
        }

        /// <summary>
        /// <c>Метод для изменения громкости звука windows <br>(Работает, начиная с Windows 7)</br></c>
        /// <b><br>Значение : Уровень громкости</br></b>
        /// <br>(0f) : 0%</br>
        /// <br>(0.10f) : 10%</br>
        /// <br>(0.50f) : 50%</br>
        /// <br>(0.90f) : 90%</br>
        /// <br>(1f) : 100%</br>
        /// <b><br>Пример использования:</br></b>
        /// <br>var audio = new AudioEndpoint();</br>
        /// <br>audio.Inizialize(1f);</br>
        /// </summary>
        /// <param name="level"></param>
        public void Inizialize(float level)
        {
            var deviceEnumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
            IMMDevice speakers = null;
            IAudioEndpointVolume vol = null;
            try
            {
                deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);
                Guid IID_IAudioEndpointVolume = typeof(IAudioEndpointVolume).GUID;
                speakers.Activate(ref IID_IAudioEndpointVolume, 0, IntPtr.Zero, out object o);
                vol = (IAudioEndpointVolume)o;
                vol.SetMasterVolumeLevelScalar(level, Guid.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // System.IO.File.WriteAllText("VolumeError.txt", $"{ex.Message}{Environment.NewLine}");
            }

            if (vol != null) Marshal.ReleaseComObject(vol);
            if (speakers != null) Marshal.ReleaseComObject(speakers);
            if (deviceEnumerator != null) Marshal.ReleaseComObject(deviceEnumerator);
        }
    }
