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
    using System.ServiceProcess;
    using System.ServiceModel;

    /// <summary>
    /// This class implements the WCF service contract.
    /// </summary>
#if DEBUG
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
#else
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = false)]
#endif
    public class ClientManager : IContractInterface
    {
        public void SetClientState(ComponentState state)
        {
            VPNClientStatus.State = state;
        }

        public void SetApplicationState(bool enabled)
        {
            VPNClientStatus.StartupAppEnabled = enabled;
        }

        public void SetRegistryState(bool enabled)
        {
            VPNClientStatus.EnabledInRegistry = enabled;
        }

        public void SetServiceState(bool enabled)
        {
            VPNClientStatus.ServiceEnabled = enabled;
        }
    }
}
