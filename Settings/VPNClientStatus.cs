
namespace PanFlip
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;
    using System.Security.Principal;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;

    public static class VPNClientStatus
    {
        private static readonly string ServiceName = "PanGPS";

        public static bool Enabled
        {
            get
            {
                return (VPNClientStatus.EnabledInRegistry ||
                        VPNClientStatus.ServiceEnabled ||
                        VPNClientStatus.StartupAppEnabled);
            }
            set
            {
                ServiceEnabled = value;
                StartupAppEnabled = value;
                EnabledInRegistry = value;
            }
        }

        private static bool EnabledInRegistry
        {
            get
            {
                bool returnValue = false;
                Microsoft.Win32.RegistryKey baseKey = Microsoft.Win32.Registry.LocalMachine;
                if ((Environment.Is64BitOperatingSystem) && (!Environment.Is64BitProcess))
                {
                    baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);
                }

                string[] registryKeyPaths = new string[] { @"SOFTWARE\Palo Alto Networks\GlobalProtect\PanGPS", @"SOFTWARE\Palo Alto Networks\GlobalProtect\Settings" };
                foreach (string path in registryKeyPaths)
                {
                    Microsoft.Win32.RegistryKey panGPSKey = baseKey.OpenSubKey(path, Microsoft.Win32.RegistryKeyPermissionCheck.ReadSubTree, System.Security.AccessControl.RegistryRights.QueryValues);
                    if (null != panGPSKey)
                    {
                        object registryValue = panGPSKey.GetValue("disable-globalprotect");
                        bool booleanRegValue = !Convert.ToBoolean(registryValue);
                        returnValue |= booleanRegValue;
                        panGPSKey.Close();
                    }
                    else
                    {
                        Console.WriteLine("Unable to open {0} key.", System.IO.Path.GetFileName(path));
                    }
                }

                baseKey.Close();
                return returnValue;
            }
            set
            {
                Microsoft.Win32.RegistryKey baseKey = Microsoft.Win32.Registry.LocalMachine;
                if ((Environment.Is64BitOperatingSystem) && (!Environment.Is64BitProcess))
                {
                    baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);
                }

                string[] registryKeyPaths = new string[] { @"SOFTWARE\Palo Alto Networks\GlobalProtect\PanGPS", @"SOFTWARE\Palo Alto Networks\GlobalProtect\Settings" };
                foreach (string path in registryKeyPaths)
                {
                    Microsoft.Win32.RegistryKey panGPSKey = baseKey.OpenSubKey(path, Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.SetValue);
                    if (null != panGPSKey)
                    {
                        panGPSKey.SetValue("disable-globalprotect", Convert.ToUInt32(!value), Microsoft.Win32.RegistryValueKind.DWord);
                        panGPSKey.Flush();
                        panGPSKey.Close();
                    }
                    else
                    {
                        Console.WriteLine("Unable to open {0} key.", System.IO.Path.GetFileName(path));
                    }
                }

                baseKey.Close();
            }
        }

        private static bool ServiceEnabled
        {
            get
            {
                if (ServiceExists(ServiceName))
                {
                    ServiceController controller = new ServiceController(ServiceName);
                    bool returnValue = (controller.StartType != ServiceStartMode.Disabled) || (controller.Status != ServiceControllerStatus.Stopped);
                    controller.Close();
                    return returnValue;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (ServiceExists(ServiceName))
                {
                    ServiceController controller = new ServiceController(ServiceName);
                    if (value)
                    {
                        ChangeServiceStartMode("Automatic");
                        controller.Start();
                    }
                    else
                    {
                        ChangeServiceStartMode("Disabled");
                        if (controller.CanStop)
                        {
                            controller.Stop();
                        }
                    }
                    controller.Close();
                }
                else
                {
                    Console.WriteLine("Service does not exist.");
                }

            }
        }

        private static uint ChangeServiceStartMode(string startMode)
        {
            SelectQuery selQuery = new SelectQuery("Win32_Service", string.Format("Name = '{0}'", ServiceName), new string[] { "Name", "StartMode" });
            uint returnValue = uint.MaxValue;
            if (selQuery != null)
            {
                ManagementScope scope = new ManagementScope(@"\root\cimv2");
                ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, selQuery);
                foreach (ManagementObject service in searcher.Get())
                {
                    object WMIReturnValue = service.InvokeMethod("ChangeStartMode", new string[] { startMode });
                    returnValue = (UInt32)WMIReturnValue;
                }

                searcher.Dispose();
            }
            return returnValue;
        }

        private static bool StartupAppEnabled
        {
            get
            {
                bool returnValue = true;
                Microsoft.Win32.RegistryKey baseKey = Microsoft.Win32.Registry.LocalMachine;
                if ((Environment.Is64BitOperatingSystem) && (!Environment.Is64BitProcess))
                {
                    baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);
                }

                Microsoft.Win32.RegistryKey panGPSKey = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run", Microsoft.Win32.RegistryKeyPermissionCheck.ReadSubTree, System.Security.AccessControl.RegistryRights.QueryValues);
                if (null != panGPSKey)
                {
                    byte[] registryValue = (byte[])panGPSKey.GetValue("GlobalProtect");
                    returnValue = (registryValue[0] == 2);
                    panGPSKey.Close();
                }

                baseKey.Close();
                return returnValue;
            }
            set
            {
                Microsoft.Win32.RegistryKey baseKey = Microsoft.Win32.Registry.LocalMachine;
                if ((Environment.Is64BitOperatingSystem) && (!Environment.Is64BitProcess))
                {
                    baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Microsoft.Win32.RegistryView.Registry64);
                }

                Microsoft.Win32.RegistryKey panGPSKey = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run", Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.SetValue | System.Security.AccessControl.RegistryRights.QueryValues);
                if (null != panGPSKey)
                {
                    byte[] registryValue = (byte[])panGPSKey.GetValue("GlobalProtect");
                    if (value)
                    {
                        registryValue[0] = 2;
                        for (int i = 1; i < registryValue.Length; i++)
                        {
                            registryValue[i] = 0;
                        }
                    }
                    else
                    {
                        registryValue[0] = 3;
                    }
                    panGPSKey.SetValue("GlobalProtect", registryValue, Microsoft.Win32.RegistryValueKind.Binary);
                    panGPSKey.Flush();
                    panGPSKey.Close();
                }
                else
                {
                    Console.WriteLine("Unable to open StartupApproved\\Run key.");
                }

                baseKey.Close();
            }
        }

        private static bool ServiceExists(string serviceName)
        {
            bool serviceExists = false;
            ServiceController[] services = ServiceController.GetServices();
            for (int i = 0; (i < services.Length) && (!serviceExists); i++)
            {
                serviceExists |= (string.Compare(services[i].ServiceName, serviceName, true) == 0);
            }
            return serviceExists;
        }

        /// <summary>
        /// Returns true if the current process is running with administrator privileges.
        /// </summary>
        /// <returns></returns>
        private static bool ProcessIsElevated
        {
            get
            {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

    }
}
