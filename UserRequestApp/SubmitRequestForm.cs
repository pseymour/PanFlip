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
    using System.Security.Principal;
    using System.ServiceModel;
    using System.ServiceProcess;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    internal partial class SubmitRequestForm : Form
    {
        private bool clientEnabled;

        private delegate void ConfigureButtonDelegate();
        private ConfigureButtonDelegate buttonConfigDelegate;

        private delegate void SetButtonTextDelegate(Button button, string text);
        private SetButtonTextDelegate setButtonTextDelegate;

        /*
        private delegate void SetButtonEnabledDelegate(Button button, bool enabled);
        private SetButtonEnabledDelegate setButtonEnabledDelegate;
        */

        /// <summary>
        /// Initializes a new instance of the SubmitRequestForm class.
        /// </summary>
        public SubmitRequestForm()
        {
            this.InitializeComponent();
            
            this.buttonConfigDelegate = new ConfigureButtonDelegate(this.ConfigureButton);
            this.setButtonTextDelegate = new SetButtonTextDelegate(this.SetButtonText);

            this.clientEnabled = VPNClientStatus.Enabled;

            /*
            this.setButtonEnabledDelegate = new SetButtonEnabledDelegate(this.SetButtonEnabled);
            */

            this.Icon = Properties.Resources.SecurityLock;

            this.SetFormText();
        }

        private void SetButtonText(Button button, string text)
        {
            button.Text = text;
        }

        /*
        private void SetButtonEnabled(Button button, bool enabled)
        {
            button.Enabled = enabled;
        }
        */

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
            this.clientEnabled = VPNClientStatus.Enabled;
            if (clientEnabled)
            {
                toggleButton.ImageKey = "green-dot";
                this.toggleButton.Invoke(this.setButtonTextDelegate, this.toggleButton, "GlobalProtect is enabled.");
                //this.toggleButton.Text = "GlobalProtect is enabled.";
            }
            else
            {
                toggleButton.ImageKey = "red-dot";
                this.toggleButton.Invoke(this.setButtonTextDelegate, this.toggleButton, "GlobalProtect is disabled.");
                //this.toggleButton.Text = "GlobalProtect is disabled.";
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
            //ConfigureButton();
            ThreadStart serviceThreadStart = new ThreadStart(this.buttonConfigDelegate);
            new Thread(serviceThreadStart).Start();
        }


        private void ToggleButtonClickHandler(object sender, EventArgs e)
        {
            this.toggleButton.Enabled = false;
            //this.toggleButton.Invoke(this.setButtonEnabledDelegate, this.toggleButton, false);

            //Task.Run(() =>
            //{
                NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
                ChannelFactory<IContractInterface> namedPipeFactory = new ChannelFactory<IContractInterface>(binding, Settings.NamedPipeServiceBaseAddress);
                IContractInterface channel = namedPipeFactory.CreateChannel();

                channel.SetClientState(!clientEnabled);

            
                namedPipeFactory.Close();
            //}).ContinueWith(shit =>
            //{
            //this.toggleButton.Invoke(this.buttonConfigDelegate);

            this.ConfigureButton();

            /*
                
                if (clientEnabled)
                {
                    toggleButton.ImageKey = "green-dot";
                    this.toggleButton.Text = "GlobalProtect is enabled.";
                }
                else
                {
                    toggleButton.ImageKey = "red-dot";
                    this.toggleButton.Text = "GlobalProtect is disabled.";
                }
            */
                
            //});

            /*
  var collection = await Task.Run(() =>
  {
    // Some code Create Collection ...
    // Some code with business logic  ..
    return currentCollection;
  });
  saleDataGrid.ItemsSource = collection;
  saleDataGrid.Items.Refresh();
            */


            //Task requestRightsTask = Task.Factory
            //    .StartNew(() => this.ToggleVPNClient())
            ////requestRightsTask.Wait(new TimeSpan(0, 0, 10));
            //.ContinueWith(resultTask => /*LoadServices());*/
            //{
            //    // Thread.Sleep(2500);

            //    if (clientEnabled)
            //    {
            //        toggleButton.ImageKey = "green-dot";
            //        this.toggleButton.Invoke(this.setButtonTextDelegate, this.toggleButton, "GlobalProtect is enabled.");
            //    }
            //    else
            //    {
            //        toggleButton.ImageKey = "red-dot";
            //        this.toggleButton.Invoke(this.setButtonTextDelegate, this.toggleButton, "GlobalProtect is disabled.");
            //    }

            //    this.toggleButton.Invoke(this.setButtonEnabledDelegate, this.toggleButton, true);

            //    /* tasks[j].ContinueWith(pt => Shutdown(hostName, forceShutdown, pt.Result)); */

            //});

            /*
            ThreadStart serviceThreadStart = new ThreadStart(this.buttonConfigDelegate);
            new Thread(serviceThreadStart).Start();
            */

            this.toggleButton.Enabled = true;
            //this.toggleButton.Invoke(this.setButtonEnabledDelegate, this.toggleButton, true);

        }

        private void ToggleVPNClient()
        {
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
            ChannelFactory<IContractInterface> namedPipeFactory = new ChannelFactory<IContractInterface>(binding, Settings.NamedPipeServiceBaseAddress);
            IContractInterface channel = namedPipeFactory.CreateChannel();

            channel.SetClientState(!clientEnabled);

            namedPipeFactory.Close();
        }
    }
}
