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

    /// <summary>
    /// This class manages application settings.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Gets the base address for the service host that is available via named pipes.
        /// </summary>
        public static string NamedPipeServiceBaseAddress
        {
            get
            {
                return string.Format("net.pipe://{0}/PanFlip/Service", FullyQualifiedHostName);
            }
        }

        /// <summary>
        /// Returns the fully-qualified host name of the local computer.
        /// </summary>
        /// <remarks>
        /// If there is an error determining the fully-qualified name, the NetBIOS name is returned.
        /// </remarks>
        public static string FullyQualifiedHostName
        {
            get
            {
                string hostName;
                try
                {
                    hostName = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).HostName.ToLowerInvariant();
                }
                catch (System.Net.Sockets.SocketException) { hostName = System.Environment.MachineName; }
                catch (System.ArgumentNullException) { hostName = System.Environment.MachineName; }
                catch (System.ArgumentOutOfRangeException) { hostName = System.Environment.MachineName; }
                catch (System.ArgumentException) { hostName = System.Environment.MachineName; }
                return hostName;
            }
        }
    }
}
