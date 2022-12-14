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
    using System.Security.Principal;
    using System.ServiceModel;
    using System.ServiceProcess;

    /// <summary>
    /// This class is the Windows Service, which does privileged work
    /// on behalf of the an unprivileged user.
    /// </summary>
    public partial class PanFlipService : ServiceBase
    {
        /// <summary>
        /// A Windows Communication Foundation (WCF) service host which communicates over named pipes.
        /// </summary>
        /// <remarks>
        /// This service host exists for communication on the local computer. It is not accessible
        /// from remote computers, and it is therefore always enabled.
        /// </remarks>
        private ServiceHost namedPipeServiceHost = null;

        /// <summary>
        /// Instantiate a new instance of the PanFlip Windows service.
        /// </summary>
        public PanFlipService()
        {
            InitializeComponent();
        }
        
        /// <summary>
        /// Creates the WCF Service Host which is accessible via named pipes.
        /// </summary>
        private void OpenNamedPipeServiceHost()
        {
            this.namedPipeServiceHost = new ServiceHost(typeof(ClientManager), new Uri(Settings.NamedPipeServiceBaseAddress));
            NetNamedPipeBinding binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.Transport);
            this.namedPipeServiceHost.AddServiceEndpoint(typeof(IContractInterface), binding, Settings.NamedPipeServiceBaseAddress);
            this.namedPipeServiceHost.Open();
        }

        /// <summary>
        /// Handles the startup of the service. 
        /// </summary>
        /// <param name="args">
        /// Data passed the start command.
        /// </param>
        /// <remarks>
        /// This function executes when a Start command is sent to the service by the
        /// Service Control Manager (SCM) or when the operating system starts
        /// (for a service that starts automatically).
        /// </remarks>
        protected override void OnStart(string[] args)
        {
            try
            {
                base.OnStart(args);
            }
            catch (Exception) { };

            // Open the service host which is accessible via named pipes.
            this.OpenNamedPipeServiceHost();
        }

        /// <summary>
        /// Handles the stopping of the service.
        /// </summary>
        /// <remarks>
        /// Executes when a stop command is sent to the service by the Serivce Control Manager (SCM).
        /// </remarks>
        protected override void OnStop()
        {
            if ((this.namedPipeServiceHost != null) && (this.namedPipeServiceHost.State == CommunicationState.Opened))
            {
                this.namedPipeServiceHost.Close();
            }

            base.OnStop();
        }
    }
}
