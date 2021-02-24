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
    public delegate void ConnectionStatusEventHandler(ConnectionStatus status, DateTime time, string message = "");

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

    public static class HttpClientTools
    {
        public static async Task StartDownload(string DownloadUrl, string filePath, TaskControl<float> taskControl = null)
        {
            using var client = new HttpClient
            {
                Timeout = taskControl is not null ? taskControl.TimeOut : TimeSpan.FromMinutes(1)
            };

            // Create a file stream to store the downloaded data.
            // This really can be any type of writeable stream.
            using var file = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);

            // Use the custom extension method below to download the data.
            // The passed progress-instance will receive the download status updates.

            if (taskControl is not null)
                await client.DownloadAsync(DownloadUrl, file, taskControl.Progress, taskControl.Cts.Token);
            else
                await client.DownloadAsync(DownloadUrl, file);
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
