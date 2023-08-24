using SharpSvn;

namespace HotfixMods.Infrastructure.Helpers
{
    public class FileDownloadHelper
    {
        // This downloads the content of a folder in GitHub. 
        // The best way to get estimatedBytesSize is to just run the download once.

        static HttpClient? _httpClient;
        string callbackTitle = "Downloading";

        public async Task DownloadGitHubFolderContentAsync(string gitHubUrl, string destinationPath, long? estimatedBytesSize = null, Action<string, string, int>? progressCallback = null)
        {
            try
            {
                if (Directory.Exists(destinationPath))
                {
                    Directory.Delete(destinationPath, true);
                }

                await Task.Run(() =>
                {
                    using (var client = new SvnClient())
                    {
                        if (estimatedBytesSize != null && progressCallback != null)
                        {
                            IProgress<long> progress = new Progress<long>(value =>
                            {
                                double percentageDone = (double)value / (long)estimatedBytesSize * 100;
                                if (percentageDone > 99)
                                    percentageDone = 99;
                                progressCallback.Invoke(callbackTitle, gitHubUrl, (int)percentageDone);
                            });
                            client.Progress += (sender, e) =>
                            {
                                progress.Report(e.Progress);
                            };
                        }

                        var target = new SvnUriTarget(gitHubUrl);
                        client.Export(target, destinationPath);
                    }
                });

                progressCallback?.Invoke(callbackTitle, gitHubUrl, 100);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task DownloadFileAsync(string url, string destinationPath, string fileNameWithExtension, Action<string, string, int>? progressCallback = null)
        {
            try
            {
                _httpClient = _httpClient ?? new HttpClient();
                var fullPath = Path.Combine(destinationPath, fileNameWithExtension);

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }

                using (var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    var totalRead = 0L;
                    var buffer = new byte[8192];
                    var totalBytesToRead = response.Content.Headers.ContentLength.GetValueOrDefault(-1L);
                    var isMoreToRead = totalBytesToRead != -1;

                    do
                    {
                        var read = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (read == 0)
                        {
                            isMoreToRead = false;
                            continue;
                        }

                        await fileStream.WriteAsync(buffer, 0, read);

                        if (progressCallback != null)
                        {
                            totalRead += read;
                            var percentage = totalBytesToRead > 0 ? ((double)totalRead / totalBytesToRead) * 100 : 0;
                            progressCallback.Invoke(callbackTitle, url, (int)Math.Floor(percentage));
                        }
                    }
                    while (isMoreToRead);
                }
            }catch(Exception ex)
            {

            }
            progressCallback?.Invoke(callbackTitle, url, 100);
        }
    }
}
