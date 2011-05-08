using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Storage;

namespace StoneCircle
{
    class DeviceManager
    {
        private static readonly String SAVE_CONTAINER_DISPLAY_NAME = "Saves";
        
        private Dictionary<PlayerIndex, StorageContainer> availableContainers = new Dictionary<PlayerIndex, StorageContainer>();
        private List<PlayerIndex> upstreamContainerRequests = new List<PlayerIndex>();
        private Dictionary<PlayerIndex, IAsyncResult> downstreamContainerRequests = new Dictionary<PlayerIndex, IAsyncResult>();

        private StorageDevice device;
        private IAsyncResult storageDeviceRequest;

        public void Update()
        {
            if (storageDeviceRequest != null && storageDeviceRequest.IsCompleted)
            {
                setStorageDevice();
            }

            if (hasStorageDevice())
            {
                foreach (PlayerIndex upstreamRequestIndex in upstreamContainerRequests)
                {
                    makeDownstreamRequest(upstreamRequestIndex);
                }

                upstreamContainerRequests.Clear();
            }

            foreach (KeyValuePair<PlayerIndex, IAsyncResult> downstreamRequest in downstreamContainerRequests)
            {
                if (downstreamRequest.Value.IsCompleted)
                {
                    setStorageContainer(downstreamRequest.Key);
                }
            }

            /* Clean up the downstream requests */
            foreach (KeyValuePair<PlayerIndex, StorageContainer> availableContainer in availableContainers) {
                if (downstreamContainerRequests.ContainsKey(availableContainer.Key))
                {
                    downstreamContainerRequests.Remove(availableContainer.Key);
                }
            }
        }

        private void setStorageDevice()
        {
            device = StorageDevice.EndShowSelector(storageDeviceRequest);
            storageDeviceRequest = null;
        }

        private void requestStorageDevice(PlayerIndex index)
        {
            if (hasStorageDevice())
            {
                return;
            }

            if (storageDeviceRequest != null)
            {
                return;
            }

            if (Guide.IsVisible) {
                return;
            }

            device = null;
            storageDeviceRequest = StorageDevice.BeginShowSelector(index, null, null);
        }

        private bool hasStorageDevice()
        {
            return (device != null && device.IsConnected);
        }

        private StorageDevice getStorageDevice()
        {
            if (hasStorageDevice())
            {
                return device;
            }

            return null;
        }

        private void setStorageContainer(PlayerIndex index)
        {
            if (downstreamContainerRequests.ContainsKey(index) && hasStorageDevice())
            {
                /* get the async thingy */
                IAsyncResult async = downstreamContainerRequests[index];
                StorageContainer container = getStorageDevice().EndOpenContainer(async);
                availableContainers.Add(index, container);
            }
        }

        public void RequestStorageContainer(PlayerIndex index)
        {
            requestStorageDevice(index);

            if (HasStorageContainer(index))
            {
                return;
            }

            if (upstreamContainerRequests.Contains(index) || downstreamContainerRequests.ContainsKey(index))
            {
                return;
            }

            upstreamContainerRequests.Add(index);
        }

        private void makeDownstreamRequest(PlayerIndex index)
        {
            if (hasStorageDevice() == false) {
                return;
            }

            IAsyncResult async = getStorageDevice().BeginOpenContainer(SAVE_CONTAINER_DISPLAY_NAME, null, null);

            downstreamContainerRequests.Add(index, async);
        }

        public bool HasStorageContainer(PlayerIndex index)
        {
            return hasStorageDevice() && availableContainers.ContainsKey(index);
        }

        public StorageContainer GetStorageContainer(PlayerIndex index)
        {
            if (HasStorageContainer(index))
            {
                return availableContainers[index];
            }

            return null;
        }


    }
}
