using LibGit2Sharp;
using LibGit2Sharp.Handlers;
using Lopez_Auto_Sales.Static;
using System;
using System.IO;

namespace Lopez_Auto_Sales.Web
{
    /// <summary>
    /// Handles any methods relating to Git.
    /// </summary>
    internal class GitManager
    {
        /// <summary>
        /// The remote URL
        /// </summary>
        private const string RemoteURL = "https://github.com/lopezautosales/Lopez-Cars";

        /// <summary>
        /// The credentials path
        /// </summary>
        private const string CredentialsPath = WebManager.Paths.ROOT + "credentials.txt";

        /// <summary>
        /// Gets or sets the repository.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        private Repository Repo { get; set; }

        /// <summary>
        /// The identity
        /// </summary>
        private readonly Identity identity = new Identity(Constants.DealerInfo.NAME, Constants.DealerInfo.EMAIL);

        /// <summary>
        /// The credentials
        /// </summary>
        private readonly UsernamePasswordCredentials credentials = TryGetCredentials();

        /// <summary>
        /// Initializes a new instance of the <see cref="GitManager"/> class.
        /// </summary>
        internal GitManager()
        {
            if (!Directory.Exists(WebManager.Paths.OUTPUT + ".git"))
                Repository.Clone(RemoteURL, WebManager.Paths.OUTPUT);

            Repo = new Repository(WebManager.Paths.OUTPUT);
            TryPullChanges();
        }

        /// <summary>
        /// Tries the pull any changes from the remote repository.
        /// </summary>
        private void TryPullChanges()
        {
            try
            {
                PullOptions options = new PullOptions
                {
                    FetchOptions = new FetchOptions()
                };
                options.FetchOptions.CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) => credentials);
                Commands.Pull(Repo, new Signature(identity, DateTime.Now), options);
            }
            catch { }
        }

        /// <summary>
        /// Tries to push any repository changes to the remote repository.
        /// </summary>
        internal void TryPushChanges()
        {
            try
            {
                Commands.Stage(Repo, "*");
                Signature signature = new Signature(identity, DateTime.Now);
                Repo.Commit("Update Listings", signature, signature);
                PushOptions options = new PushOptions
                {
                    CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) => credentials)
                };
                Repo.Network.Push(Repo.Network.Remotes["origin"], @"refs/heads/master", options);
            }
            catch { }
        }

        /// <summary>
        /// Tries the get credentials.
        /// </summary>
        /// <returns></returns>
        private static UsernamePasswordCredentials TryGetCredentials()
        {
            UsernamePasswordCredentials credentials = new UsernamePasswordCredentials();
            try
            {
                using (StreamReader sr = new StreamReader(CredentialsPath))
                {
                    credentials.Username = sr.ReadLine();
                    credentials.Password = sr.ReadLine(); //uses PAT
                }
            }
            catch { }

            return credentials;
        }
    }
}