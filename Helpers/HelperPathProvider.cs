﻿using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.AspNetCore.Hosting.Server;

namespace Eshop.Helpers
{
    public enum Folder { General, Products, Stores }
    public class HelperPathProvider
    {
        private IWebHostEnvironment hostEnvironment;
        private IServer server;

        public HelperPathProvider(IWebHostEnvironment hostEnvironment, IServer server)
        {
            this.hostEnvironment = hostEnvironment;
            this.server = server;
        }

        public string MapPath(string filename, Folder folder)
        {
            string carpeta = this.getFolderName(folder);
            string rootPath = this.hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath, carpeta, filename);
            return path;
        }

        public string getFolderName(Folder folder)
        {
            string carpeta = "";
            switch (folder)
            {
                case Folder.General:
                    carpeta = "images/general";
                    break;
                case Folder.Products:
                    carpeta = "images/products";
                    break;
                case Folder.Stores:
                    carpeta = "images/stores";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(folder), folder, null);
            }
            return carpeta;
        }

        public string MapUrlPath(string filename, Folder folder)
        {
            string carpeta = this.getFolderName(folder);
            var addresses = this.server.Features.Get<IServerAddressesFeature>().Addresses;
            string serverUrl = addresses.FirstOrDefault();
            string url = $"{serverUrl}/{carpeta}/{filename}";
            return url;
        }
    }
}
