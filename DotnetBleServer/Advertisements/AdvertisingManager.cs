using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DotnetBleServer.Core;
using Tmds.DBus;

namespace DotnetBleServer.Advertisements
{
    public class AdvertisingManager
    {
        private readonly ServerContext _Context;

        public AdvertisingManager(ServerContext context)
        {
            _Context = context;
        }

        public async Task<byte> GetActiveInstances()
        {
            LEAdvertisingManager1Properties props = await GetAdvertisingManager().GetAllAsync();
            return props.ActiveInstances;
        }

        public async Task<byte> GetSupportedInstances()
        {
            LEAdvertisingManager1Properties props = await GetAdvertisingManager().GetAllAsync();
            return props.SupportedInstances;
        }

        public async Task<string[]> GetSupportedIncludes()
        {
            LEAdvertisingManager1Properties props = await GetAdvertisingManager().GetAllAsync();
            return props.SupportedIncludes;
        }

        public async Task RegisterAdvertisement(Advertisement advertisement)
        {
            await _Context.Connection.RegisterObjectAsync(advertisement);
            Console.WriteLine($"advertisement object {advertisement.ObjectPath} created");

            await GetAdvertisingManager().RegisterAdvertisementAsync(((IDBusObject) advertisement).ObjectPath,
                new Dictionary<string, object>());

            Console.WriteLine($"advertisement {advertisement.ObjectPath} registered in BlueZ advertising manager");
        }

        public async Task UnregisterAdvertisement(Advertisement advertisement)
        {
            await GetAdvertisingManager().UnregisterAdvertisementAsync(((IDBusObject) advertisement).ObjectPath);
            Console.WriteLine($"advertisement {advertisement.ObjectPath} unregistered in BlueZ advertising manager");

            _Context.Connection.UnregisterObject(advertisement);
            Console.WriteLine($"advertisement object {advertisement.ObjectPath} deleted");
        }

        private ILEAdvertisingManager1 GetAdvertisingManager()
        {
            return _Context.Connection.CreateProxy<ILEAdvertisingManager1>("org.bluez", "/org/bluez/hci0");
        }

        public async Task CreateAdvertisement(AdvertisementProperties advertisementProperties)
        {
            var advertisement = new Advertisement("/org/bluez/example/advertisement0", advertisementProperties);
            await new AdvertisingManager(_Context).RegisterAdvertisement(advertisement);
        }
    }
}