using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;
using System.Linq;
using Microsoft.Phone.Shell;
using System.Net;
using System;


namespace ScheduledTaskAgentForLiveTile
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        /// 
        ShellTile appTile = ShellTile.ActiveTiles.First();
        protected override void OnInvoke(ScheduledTask task)
        {
            //TODO: Add code to perform your task in background

            var tileData = new FlipTileData()
            {
                BackContent = quote,
                BackTitle = "i ♥ Quotes",
                Title = "i ♥ Quotes",
                WideBackContent = quote
            };

            appTile.Update(tileData);

            NotifyComplete();
        }

        public string quote;
        public void getQuote()
        {
            var webClient = new WebClient();

            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_RandomQuoteGenerated);
            webClient.DownloadStringAsync(new Uri("http://iheartquotes.com/api/v1/random?max_lines=4"));

        }

        private void webClient_RandomQuoteGenerated(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                var RandomQuote = e.Result;
                string[] splitedString = RandomQuote.Split('[');
                var QuoteToDisplay = splitedString[0];

                if (QuoteToDisplay.Contains("&quot;"))
                {
                    QuoteToDisplay = QuoteToDisplay.Replace("&quot;", " \" ");
                }

                quote = QuoteToDisplay;
            }
        }

    }
}