using LibGit2Sharp;
using System;
using System.IO;

namespace Lopez_Auto_Sales
{
    internal class GitManager
    {
        const string RemoteURL = "https://github.com/lopezautosales/Lopez-Cars";
        private Repository Repo { get; set; }

        public GitManager()
        {
            if (!File.Exists(WebManager.Paths.OUTPUT + ".git"))
            {
                Repository.Clone(RemoteURL, WebManager.Paths.OUTPUT);
            }

            Repo = new Repository(WebManager.Paths.OUTPUT);
        }

        public void PushChanges()
        {
            Commands.Stage(Repo, "*");
            Signature signature = new Signature(Constants.BUSINESS, Constants.EMAIL, DateTime.Now);
            Repo.Commit("Update Listings", signature, signature);
            Repo.Network.Push(Repo.Branches["master"]);
        }

        public UsernamePasswordCredentials GetCredentials()
        {
            return null;
        }
    }
}