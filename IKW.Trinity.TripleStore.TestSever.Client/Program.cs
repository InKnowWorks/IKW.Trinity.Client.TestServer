using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Trinity;
using Trinity.Client;
using Trinity.Client.TestProtocols;
using Trinity.Client.TestProtocols.TripleServer;
using Trinity.Diagnostics;

namespace IKW.Trinity.TripleStore.TestSever.Client
{
    class Program
    {
        private static TripleModule tripleModule = null;

        static async Task Main(string[] args)
        {
            TrinityConfig.CurrentRunningMode = RunningMode.Client;
            TrinityConfig.LogEchoOnConsole   = true;
            TrinityConfig.LoggingLevel       = LogLevel.Info;

            Log.LogsWritten += Log_LogsWritten;

            TrinityClient client = new TrinityClient("localhost:5304");

            client.RegisterCommunicationModule<TripleModule>();
            client.Start();

            tripleModule = client.GetCommunicationModule<TripleModule>();

            tripleModule.ExternalTripleReceivedAction.Subscribe(onNext: async tripleObjectFromServer =>
            {
                using var reactiveGraphEngineResponseTask = Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(0).ConfigureAwait(false);

                    Console.WriteLine($"Processing Timestamp: {DateTime.Now.ToString(CultureInfo.InvariantCulture)}");
                    Console.WriteLine($"Triple Subject Node: {tripleObjectFromServer.Subject}");
                    Console.WriteLine($"Triple Predicate Node: {tripleObjectFromServer.Predicate}");
                    Console.WriteLine($"Triple Object Node: {tripleObjectFromServer.Object}");

                },cancellationToken: CancellationToken.None,
                  creationOptions: TaskCreationOptions.HideScheduler,
                  scheduler: TaskScheduler.Current).Unwrap().ContinueWith(async _ =>
                    {
                        await Task.Delay(0).ConfigureAwait(false);

                        Console.WriteLine("Task ExternalTripleReceivedAction Complete...");
                    }, cancellationToken: CancellationToken.None);

                var writeToConsoleTask = reactiveGraphEngineResponseTask.ConfigureAwait(false);

                await writeToConsoleTask;
            });


            tripleModule.ExternalTripleSavedToMemoryAction.Subscribe(onNext: async tripleStoreMemoryContext =>
            {
                using var retrieveTripleStoreFromMemoryCloudTask = Task.Factory.StartNew(async () =>
                    {
                        await Task.Delay(0).ConfigureAwait(false);

                        Console.WriteLine("Try locate the Triple in the TripleStore MemoryCloud");

                        foreach (var tripleNode in Global.LocalStorage.TripleStore_Selector())
                        {
                            if (tripleStoreMemoryContext.CellId != tripleNode.CellId) continue;

                            Console.WriteLine("Success! Found the Triple in the TripleStore MemoryCloud");

                            var node          = tripleNode.TripleCell;
                            var subjectNode   = node.Subject;
                            var predicateNode = node.Predicate;
                            var objectNode    = node.Object;

                            Console.WriteLine($"Triple CellId in MemoryCloud: {tripleNode.CellId}");
                            Console.WriteLine($"Subject Node: {subjectNode}");
                            Console.WriteLine($"Predicate Node: {predicateNode}");
                            Console.WriteLine($"Object Node: {objectNode}");

                            break;
                        }

                    }, cancellationToken: CancellationToken.None,
                    creationOptions: TaskCreationOptions.HideScheduler,
                    scheduler: TaskScheduler.Current).Unwrap().ContinueWith(async _ =>
                {
                    await Task.Delay(0).ConfigureAwait(false);

                    Console.WriteLine("Task ExternalTripleSavedToMemoryAction Complete...");
                }, cancellationToken: CancellationToken.None);

                var storeFromMemoryCloudTask = retrieveTripleStoreFromMemoryCloudTask.ConfigureAwait(false);

                await storeFromMemoryCloudTask;
            });

            // -------------------------------------------------------

            while (true)
            {
                try
                {
                    using var trinityClientProcessingLoopTask = Task.Factory.StartNew(async () =>
                        {
                            await Task.Delay(500).ConfigureAwait(false);

                            List<Triple> triples = new List<Triple>
                                {new Triple {Subject = "foo", Predicate = "is", Object = "bar"}};

                            using var message = new TripleStreamWriter(triples);

                            using var rsp = await client.PostTriplesToServer(message).ConfigureAwait(false);

                            Console.WriteLine($"Server responses with rsp code={rsp.errno}");

                        }, cancellationToken: CancellationToken.None,
                        creationOptions: TaskCreationOptions.LongRunning,
                        scheduler: TaskScheduler.Current).Unwrap().ContinueWith(async _ =>
                    {
                        await Task.Delay(1000).ConfigureAwait(false);

                        Console.WriteLine("Task ExternalTripleSavedToMemoryAction Complete...");
                    }, cancellationToken: CancellationToken.None);

                    var mainLoopTask = trinityClientProcessingLoopTask.ConfigureAwait(false);

                    await mainLoopTask;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

                await Task.Delay(500).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// This is how you intercept LOG I/O from the Trinity Communications Runtime
        /// </summary>
        /// <param name="trinityLogCollection"></param>
        private static void Log_LogsWritten(IList<LOG_ENTRY> trinityLogCollection)
        {
            foreach (var logEntry in trinityLogCollection)
            {
                Console.WriteLine($@"TrinityClient LOG: {logEntry.logLevel}");
                Console.WriteLine($@"TrinityClient LOG: {logEntry.logMessage}");
                Console.WriteLine($@"TrinityClient LOG: {logEntry.logTime}");
            }

            //using var trinityLog = Task.Factory.StartNew(() =>
            //    {
            //        foreach (var logEntry in trinityLogCollection)
            //        {
            //            Console.WriteLine($@"TrinityClient LOG: {logEntry.logLevel}");
            //            Console.WriteLine($@"TrinityClient LOG: {logEntry.logMessage}");
            //            Console.WriteLine($@"TrinityClient LOG: {logEntry.logTime}");
            //        }
            //    }, cancellationToken: CancellationToken.None,
            //    creationOptions: TaskCreationOptions.AttachedToParent,
            //    scheduler: TaskScheduler.Default);

        }
    }
}
