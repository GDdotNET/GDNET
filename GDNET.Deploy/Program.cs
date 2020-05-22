using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using osu.Framework;
using osu.Framework.IO.Network;
using FileWebRequest = osu.Framework.IO.Network.FileWebRequest;
using WebRequest = osu.Framework.IO.Network.WebRequest;

namespace GDNET.Deploy
{
    internal static class Program
    {
        private const string staging_folder = "staging";
        private const string releases_folder = "releases";

        public static string GitHubAccessToken = ConfigurationManager.AppSettings["GitHubAccessToken"];
        public static bool GitHubUpload = bool.Parse(ConfigurationManager.AppSettings["GitHubUpload"] ?? "false");
        public static bool NuGetUpload = bool.Parse(ConfigurationManager.AppSettings["NuGetUpload"] ?? "false");
        public static bool AnnounceDiscord = bool.Parse(ConfigurationManager.AppSettings["AnnounceDiscord"] ?? "false");
        public static string GitHubUsername = ConfigurationManager.AppSettings["GitHubUsername"];
        public static string GitHubRepoName = ConfigurationManager.AppSettings["GitHubRepoName"];
        public static string ProjectName = ConfigurationManager.AppSettings["ProjectName"];
        public static bool IncrementVersion = bool.Parse(ConfigurationManager.AppSettings["IncrementVersion"] ?? "true");
        public static string PackageName = ConfigurationManager.AppSettings["PackageName"];
        public static string CodeSigningCertificate = ConfigurationManager.AppSettings["CodeSigningCertificate"];
        public static string NugetPublishToken = ConfigurationManager.AppSettings["NugetPublishToken"];
        public static string DiscordWebhookURL = ConfigurationManager.AppSettings["DiscordWebhookURL"];
        
        public static string GitHubApiEndpoint => $"https://api.github.com/repos/{GitHubUsername}/{GitHubRepoName}/";

        private static string solutionPath;

        private static string StagingPath => Path.Combine(Environment.CurrentDirectory, staging_folder);
        
        private static readonly Stopwatch stopwatch = new Stopwatch();

        private static bool interactive;

        public static int Main(string[] args)
        {
            interactive = args[0] == "1";
            
            FindSolutionPath();
            
            if (!Directory.Exists(releases_folder))
            {
                Write("WARNING: No release directory found. Make sure you want this!", ConsoleColor.Yellow);
                Directory.CreateDirectory(releases_folder);
            }
            
            GithubRelease lastRelease = null;
            
            if (CanGitHub)
            {
                Write("Checking GitHub releases...");
                lastRelease = GetLastGithubRelease();

                if (lastRelease == null)
                    Write("This is the first GitHub release");
                else
                {
                    Write($"Last GitHub release was {lastRelease.Name}.");
                    if (lastRelease.Draft)
                        Write("WARNING: This is a pending draft release! You might not want to push a build with this present.", ConsoleColor.Red);
                }
            }
            
            string version;

            if (args.Length > 1 && !string.IsNullOrEmpty(args[1]))
                version = args[1];
            else
            {
                Write("Unable to find version number");
                return -1;
            }
            
            Console.ResetColor();
            Console.WriteLine($"Increment Version:       {IncrementVersion}");
            Console.WriteLine($"Signing Certificate:     {CodeSigningCertificate}");
            Console.WriteLine($"Upload to GitHub:        {GitHubUpload}");
            Console.WriteLine($"Upload to Nuget:         {NuGetUpload}");
            Console.WriteLine($"Announce to Discord:     {AnnounceDiscord}");
            Console.WriteLine();
            Console.Write($"Ready to deploy {version}!");
            
            PauseIfInteractive();
            
            stopwatch.Start();
            
            RefreshDirectory(staging_folder);
            Write("Running build process...");

            switch (RuntimeInfo.OS)
            {
                case RuntimeInfo.Platform.Windows:
                    if (lastRelease != null)
                        GetAssetsFromRelease(lastRelease);

                    RunCommand("dotnet", $"build {ProjectName} -o {StagingPath} --configuration Release /p:Version={version}");

                    string nupkgFilename = $"{PackageName}.{version}.nupkg";
                    
                    // upload to NuGet
                    if (NuGetUpload)
                    {
                        Write("Uploading to NuGet...");
                        
                        Write($"Running dotnet nuget push {nupkgFilename} -k {NugetPublishToken} -s https://api.nuget.org/v3/index.json...");

                        var psi = new ProcessStartInfo("dotnet", $"nuget push {nupkgFilename} -k {NugetPublishToken} -s https://api.nuget.org/v3/index.json")
                        {
                            WorkingDirectory = Path.Combine(Directory.GetCurrentDirectory(), @"staging"),
                            CreateNoWindow = true,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            UseShellExecute = false,
                            WindowStyle = ProcessWindowStyle.Hidden
                        };

                        Process p = Process.Start(psi);
                        if (p == null) throw new NullReferenceException();

                        string output = p.StandardOutput.ReadToEnd();
                        output += p.StandardError.ReadToEnd();

                        p.WaitForExit();

                        if (p.ExitCode != 0)
                        {
                            Write(output);
                            Error($"Command dotnet nuget push {nupkgFilename} -k {NugetPublishToken} -s https://api.nuget.org/v3/index.json failed!");
                        }
                    }
                    
                    break;
            }
            
            if (GitHubUpload)
                UploadBuild(version);

            AnnounceToDiscord(version);
            
            Write("Done!");
            PauseIfInteractive();
            
            return 0;
        }

        private static void FindSolutionPath()
        {
            solutionPath = Path.Combine("D:", "Projects", "GDNET-Core");
        }
        
        private static void RefreshDirectory(string directory)
        {
            if (Directory.Exists(directory))
                Directory.Delete(directory, true);
            Directory.CreateDirectory(directory);
        }
        
        private static void PauseIfInteractive()
        {
            if (interactive)
                Console.ReadLine();
            else
                Console.WriteLine();
        }

        private static void Write(string message, ConsoleColor col = ConsoleColor.Gray)
        {
            if (stopwatch.ElapsedMilliseconds > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(stopwatch.ElapsedMilliseconds.ToString().PadRight(8));
            }

            Console.ForegroundColor = col;
            Console.WriteLine(message);
        }
        
        private static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"FATAL ERROR: {message}");

            PauseIfInteractive();
            Environment.Exit(-1);
        }
        
        private static void RunCommand(string command, string args, bool useSolutionPath = true)
        {
            Write($"Running {command} {args}...");

            var psi = new ProcessStartInfo(command, args)
            {
                WorkingDirectory = useSolutionPath ? solutionPath : Environment.CurrentDirectory,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            Process p = Process.Start(psi);
            if (p == null) return;

            string output = p.StandardOutput.ReadToEnd();
            output += p.StandardError.ReadToEnd();

            p.WaitForExit();

            if (p.ExitCode == 0) return;

            Write(output);
            Error($"Command {command} {args} failed!");
        }
        
        private static bool CanGitHub => !string.IsNullOrEmpty(GitHubAccessToken);
        
        private static GithubRelease GetLastGithubRelease()
        {
            var req = new JsonWebRequest<List<GithubRelease>>($"{GitHubApiEndpoint}releases");
            req.AuthenticatedBlockingPerform();
            return req.ResponseObject.FirstOrDefault();
        }
        
        private static List<GithubPullRequest> GetPullRequests()
        {
            // Gets the last release
            var previousVersion = GetLastGithubRelease();
            var reqMinified = new JsonWebRequest<List<GithubMinifiedPullRequest>>($"{GitHubApiEndpoint}pulls?state=closed");
            reqMinified.AuthenticatedBlockingPerform();

            List<GithubMinifiedPullRequest> minifiedPullRequests = reqMinified.ResponseObject;
            List<GithubPullRequest> requests = new List<GithubPullRequest>();

            foreach (var pr in minifiedPullRequests)
            {
                // We want to ignore all previous pr's from the last release
                if (pr.MergedAt < previousVersion.PublishedAt) continue;
                
                var req = new JsonWebRequest<GithubPullRequest>($"{GitHubApiEndpoint}pulls/{pr.Number}");
                req.AuthenticatedBlockingPerform();
                
                requests.Add(req.ResponseObject);
            }

            pullRequests = requests;
            return requests;
        }

        private static void UploadBuild(string version)
        {
            if (!CanGitHub)
                return;
            
            Write("Publishing to GitHub...");
            
            var req = new JsonWebRequest<GithubRelease>($"{GitHubApiEndpoint}releases")
            {
                Method = HttpMethod.Post,
            };

            GithubRelease targetRelease = GetLastGithubRelease();
            
            if (targetRelease == null || targetRelease.TagName != version)
            {
                Write($"- Creating release {version}...", ConsoleColor.Yellow);
                
                // Get all of the previous PR's name
                string body = $"Version {version} for {GitHubRepoName} has been released! this now fixes and adds:\r\n";
                var requests = GetPullRequests();
                
                // Adds every pull requests after the last release
                requests.ForEach(pr => { body += $"- {pr.Title} | [{pr.User.Name}]({pr.User.Link})\r\n"; });
                
                req.AddRaw(JsonConvert.SerializeObject(new GithubRelease
                {
                    Name = $"{GitHubRepoName} v{version}",
                    TagName = version,
                    Description = body,
                    Draft = true,
                    PublishedAt = DateTime.Now
                }));
                
                req.AuthenticatedBlockingPerform();

                targetRelease = req.ResponseObject;
            }
            else
            {
                Write($"- Adding to existing release {version}...", ConsoleColor.Yellow);
            }
            
            var assetUploadUrl = targetRelease.UploadUrl.Replace("{?name,label}", "?name={0}");
            foreach (var a in Directory.GetFiles(staging_folder).Reverse())
            {
                if (Path.GetFileName(a).StartsWith('.'))
                    continue;
                
                if (!Path.GetFileName(a).EndsWith(".nupkg"))
                    continue;

                var upload = new WebRequest(assetUploadUrl, Path.GetFileName(a))
                {
                    Method = HttpMethod.Post,
                    Timeout = 240000,
                    ContentType = "application/octet-stream",
                };
                
                upload.AddRaw(File.ReadAllBytes(a));
                upload.AuthenticatedBlockingPerform();
            }

            OpenGitHubReleasePage();
        }
        
        private static void AnnounceToDiscord(string version)
        {
            if (!AnnounceDiscord) return;
            
            var req = new JsonWebRequest<DiscordWebhook>(DiscordWebhookURL)
            {
                Method = HttpMethod.Post,
                ContentType = "application/json"
            };
            
            // Get all of the previous PR's name
            var body = $"Version {version} for {GitHubRepoName} has been released! this now fixes and adds:\r\n";
                
            // Adds every pull requests after the last release
            pullRequests.ForEach(pr => { body += $"- {pr.Title} | {pr.User.Name}\r\n"; });

            body += $"\r\nDownload:\r\n<https://github.com/{GitHubUsername}/{GitHubRepoName}/releases>\r\n<https://www.nuget.org/packages/{GitHubRepoName}/{version}>";
            
            req.AddRaw(JsonConvert.SerializeObject(new DiscordWebhook
            {
                Content = body
            }));
            
            req.Perform();
        }
        
        private static List<GithubPullRequest> pullRequests;
        
        private static void OpenGitHubReleasePage() => Process.Start(new ProcessStartInfo
        {
            FileName = $"https://github.com/{GitHubUsername}/{GitHubRepoName}/releases/",
            UseShellExecute = true //see https://github.com/dotnet/corefx/issues/10361
        });
        
        /// <summary>
        /// Download assets from a previous release into the releases folder.
        /// </summary>
        /// <param name="release"></param>
        private static void GetAssetsFromRelease(GithubRelease release)
        {
            if (!CanGitHub) return;

            //there's a previous release for this project.
            var assetReq = new JsonWebRequest<List<GithubObject>>($"{GitHubApiEndpoint}releases/{release.Id}/assets");
            assetReq.AuthenticatedBlockingPerform();
            var assets = assetReq.ResponseObject;

            //make sure our RELEASES file is the same as the last build on the server.
            var releaseAsset = assets.FirstOrDefault(a => a.Name == "RELEASES");

            //if we don't have a RELEASES asset then the previous release likely wasn't a Squirrel one.
            if (releaseAsset == null) return;

            bool requireDownload = false;

            if (!File.Exists(Path.Combine(releases_folder, $"{PackageName}-{release.Name}-full.nupkg")))
            {
                Write("Last version's package not found locally.", ConsoleColor.Red);
                requireDownload = true;
            }
            else
            {
                var lastReleases = new RawFileWebRequest($"{GitHubApiEndpoint}releases/assets/{releaseAsset.Id}");
                lastReleases.AuthenticatedBlockingPerform();
                if (File.ReadAllText(Path.Combine(releases_folder, "RELEASES")) != lastReleases.ResponseString)
                {
                    Write("Server's RELEASES differed from ours.", ConsoleColor.Red);
                    requireDownload = true;
                }
            }

            if (!requireDownload) return;

            Write("Refreshing local releases directory...");
            RefreshDirectory(releases_folder);

            foreach (var a in assets)
            {
                if (a.Name.EndsWith(".exe") || a.Name.EndsWith(".app.zip")) continue;

                Write($"- Downloading {a.Name}...", ConsoleColor.Yellow);
                new FileWebRequest(Path.Combine(releases_folder, a.Name), $"{GitHubApiEndpoint}releases/assets/{a.Id}").AuthenticatedBlockingPerform();
            }
        }
        
        public static void AuthenticatedBlockingPerform(this WebRequest r)
        {
            r.AddHeader("Authorization", $"token {GitHubAccessToken}");
            r.Perform();
        }
    }
    
    internal class RawFileWebRequest : WebRequest
    {
        public RawFileWebRequest(string url)
            : base(url)
        {
        }

        protected override string Accept => "application/octet-stream";
    }
}