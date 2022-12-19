// 
// Copyright © 2019, Patrick S. Seymour
// Licensed under the GNU General Public License, version 3.
// See the LICENSE file in the project root for full license information.  
//
// This file is part of PanFlip.
//
// PanFlip is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, version 3.
//
// PanFlip is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with PanFlip. If not, see <http://www.gnu.org/licenses/>.
//

namespace PanFlip
{
    using System;
    using System.Reflection;
    using System.ServiceModel;
    using System.Windows.Forms;

    internal partial class SubmitRequestForm : Form
    {
        private readonly System.Timers.Timer notifyIconTimer;

        private delegate void SetButtonTextDelegate(Button button, string text);
        private readonly SetButtonTextDelegate setButtonTextDelegate;

        /// <summary>
        /// Initializes a new instance of the SubmitRequestForm class.
        /// </summary>
        public SubmitRequestForm()
        {
            this.InitializeComponent();

            this.setButtonTextDelegate = new SetButtonTextDelegate(this.SetButtonText);

            // Configure the notification timer.
            this.notifyIconTimer = new System.Timers.Timer
            {
                Interval = 2000,
                AutoReset = true
            };
            this.notifyIconTimer.Elapsed += NotifyIconTimerElapsed;

            this.Icon = Properties.Resources.pan;

            this.SetFormText();
        }

        /// <summary>
        /// Handles the Elapsed event for the notification area icon.
        /// </summary>
        /// <param name="sender">
        /// The timer whose Elapsed event is firing.
        /// </param>
        /// <param name="e">
        /// Data related to the event.
        /// </param>
        void NotifyIconTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.ConfigureButton();
        }

        private void SetButtonText(Button button, string text)
        {
            button.Text = text;
        }

        /*
        /// <summary>
        /// Returns true if the current process is running with administrator privileges.
        /// </summary>
        /// <returns></returns>
        private bool ProcessIsElevated
        {
            get
            {
                return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
            }
        }
        */

        private void ConfigureButton()
        {
            switch (VPNClientStatus.State)
            {
                case ComponentState.Enabled:
                    {
                        toggleButton.ImageKey = "green-dot";
                        this.toggleButton.Invoke(this.setButtonTextDelegate, this.toggleButton, "&GlobalProtect is enabled.");
                        break;
                    }
                case ComponentState.Disabled:
                    {
                        toggleButton.ImageKey = "red-dot";
                        this.toggleButton.Invoke(this.setButtonTextDelegate, this.toggleButton, "&GlobalProtect is disabled.");
                        break;
                    }
                case ComponentState.PartiallyEnabled:
                    {
                        toggleButton.ImageKey = "yellow-dot";
                        this.toggleButton.Invoke(this.setButtonTextDelegate, this.toggleButton, "&GlobalProtect is partially enabled.");
                        break;
                    }
            }

            switch (VPNClientStatus.ServiceEnabled)
            {
                case true:
                    {
                        serviceButton.ImageKey = "green-dot";
                        break;
                    }
                case false:
                    {
                        serviceButton.ImageKey = "red-dot";
                        break;
                    }
                default:
                    {
                        serviceButton.ImageKey = "yellow-dot";
                        break;
                    }
            }
            switch (VPNClientStatus.EnabledInRegistry)
            {
                case true:
                    {
                        registryButton.ImageKey = "green-dot";
                        break;
                    }
                case false:
                    {
                        registryButton.ImageKey = "red-dot";
                        break;
                    }
                default:
                    {
                        registryButton.ImageKey = "yellow-dot";
                        break;
                    }
            }
            switch (VPNClientStatus.StartupAppEnabled)
            {
                case true:
                    {
                        appButton.ImageKey = "green-dot";
                        break;
                    }
                case false:
                    {
                        appButton.ImageKey = "red-dot";
                        break;
                    }
                default:
                    {
                        appButton.ImageKey = "yellow-dot";
                        break;
                    }
            }
        }

        /// <summary>
        /// Sets the form's text to the name of the application plus a partial version number.
        /// </summary>
        private void SetFormText()
        {
            System.Text.StringBuilder formText = new System.Text.StringBuilder();
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length == 0)
            {
                formText.Append(Properties.Resources.ApplicationName);
            }
            else
            {
                formText.Append(((AssemblyProductAttribute)attributes[0]).Product);
            }
            /*
            formText.Append(' ');
            formText.Append(Assembly.GetExecutingAssembly().GetName().Version.ToString(2));
            */
            Text = formText.ToString();
        }

        /// <summary>
        /// Handles the Load event for the form.
        /// </summary>
        /// <param name="sender">
        /// The form being loaded.
        /// </param>
        /// <param name="e">
        /// Data specific to this event.
        /// </param>
        private void FormLoad(object sender, EventArgs e)
        {
            ConfigureButton();

            this.notifyIconTimer.Start();
        }

        private void ToggleButtonClickHandler(object sender, EventArgs e)
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
            ChannelFactory<IContractInterface> namedPipeFactory = new ChannelFactory<IContractInterface>(binding, Settings.NamedPipeServiceBaseAddress);
            IContractInterface channel = namedPipeFactory.CreateChannel();

            if (VPNClientStatus.State == ComponentState.Disabled)
            {
                channel.SetClientState(ComponentState.Enabled);
            }
            else
            {
                channel.SetClientState(ComponentState.Disabled);
            }

            namedPipeFactory.Close();
        }

        private void ServiceButtonClickHandler(object sender, EventArgs e)
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
            ChannelFactory<IContractInterface> namedPipeFactory = new ChannelFactory<IContractInterface>(binding, Settings.NamedPipeServiceBaseAddress);
            IContractInterface channel = namedPipeFactory.CreateChannel();

            channel.SetServiceState(!VPNClientStatus.ServiceEnabled);

            namedPipeFactory.Close();
        }

        private void RegistryButtonClickHandler(object sender, EventArgs e)
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
            ChannelFactory<IContractInterface> namedPipeFactory = new ChannelFactory<IContractInterface>(binding, Settings.NamedPipeServiceBaseAddress);
            IContractInterface channel = namedPipeFactory.CreateChannel();

            channel.SetRegistryState(!VPNClientStatus.EnabledInRegistry);

            namedPipeFactory.Close();
        }

        private void ApplicationButtonClickHandler(object sender, EventArgs e)
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
            ChannelFactory<IContractInterface> namedPipeFactory = new ChannelFactory<IContractInterface>(binding, Settings.NamedPipeServiceBaseAddress);
            IContractInterface channel = namedPipeFactory.CreateChannel();

            channel.SetApplicationState(!VPNClientStatus.StartupAppEnabled);

            namedPipeFactory.Close();
        }
    }
}
