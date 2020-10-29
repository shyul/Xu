/// ***************************************************************************
/// Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Network related basic functions.
/// 
/// ***************************************************************************

using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Xu
{
    public enum ConnectionStatus : int
    {
        Disconnected = 0,
        Connecting = 1,
        Connected = 2,
        Disconnecting = 3,
    }

    public enum DownloadStatus : int
    {
        NonExistent = -1,
        Completed = 0,
        Interrupted = 1,
        Timeout = 3,
    }

    public delegate void ConnectionStatusEventHandler(ConnectionStatus status, DateTime time, string message = "");

    public static class HttpClientExtensions
    {
        public static async Task StartDownload(string DownloadUrl, string filePath, IProgress<float> progress, CancellationToken cancellationToken)
        {
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(5);

            // Create a file stream to store the downloaded data.
            // This really can be any type of writeable stream.
            using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // Use the custom extension method below to download the data.
            // The passed progress-instance will receive the download status updates.
            await client.DownloadAsync(DownloadUrl, file, progress, cancellationToken);
        }

        public static async Task DownloadAsync(this HttpClient client, string requestUri, Stream destination, IProgress<float> progress = null, CancellationToken cancellationToken = default)
        {
            // Get the http headers first to examine the content length
            using var response = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);
            var contentLength = response.Content.Headers.ContentLength;

            using var download = await response.Content.ReadAsStreamAsync();

            // Ignore progress reporting when no progress reporter was 
            // passed or when the content length is unknown
            if (progress == null || !contentLength.HasValue)
            {
                await download.CopyToAsync(destination);
                return;
            }

            // Convert absolute progress (bytes downloaded) into relative progress (0% - 100%)
            var relativeProgress = new Progress<long>(totalBytes => progress.Report((float)totalBytes / contentLength.Value));
            // Use extension method to report progress while downloading
            await download.CopyToAsync(destination, 81920, cancellationToken);
            progress.Report(1);
        }
    }
}
