using System;
using EphemeralMongo;

namespace MovieSearch.Tests.Repositories;

public static class MongoRunnerProvider
{
    private static readonly object LockObj = new ();
    private static IMongoRunner? _runner;
    private static int _useCounter;

    public static IMongoRunner Get()
    {
        lock (LockObj)
        {
            _runner ??= MongoRunner.Run(new MongoRunnerOptions
            {
                // Set shared static options
            });
            _useCounter++;
            return new MongoRunnerWrapper(_runner);
        }
    }

    private sealed class MongoRunnerWrapper : IMongoRunner
    {
        private IMongoRunner? _underlyingMongoRunner;

        public MongoRunnerWrapper(IMongoRunner underlyingMongoRunner)
        {
            _underlyingMongoRunner = underlyingMongoRunner;
        }

        public string ConnectionString
        {
            get => _underlyingMongoRunner?.ConnectionString ?? throw new ObjectDisposedException(nameof(IMongoRunner));
        }

        public void Import(string database, string collection, string inputFilePath, string? additionalArguments = null, bool drop = false)
        {
            if (_underlyingMongoRunner == null)
            {
                throw new ObjectDisposedException(nameof(IMongoRunner));
            }

            _underlyingMongoRunner.Import(database, collection, inputFilePath, additionalArguments, drop);
        }

        public void Export(string database, string collection, string outputFilePath, string? additionalArguments = null)
        {
            if (_underlyingMongoRunner == null)
            {
                throw new ObjectDisposedException(nameof(IMongoRunner));
            }

            _underlyingMongoRunner.Export(database, collection, outputFilePath, additionalArguments);
        }

        public void Dispose()
        {
            if (_underlyingMongoRunner != null)
            {
                _underlyingMongoRunner = null;
                StaticDispose();
            }
        }

        private static void StaticDispose()
        {
            lock (LockObj)
            {
                if (_runner != null)
                {
                    _useCounter--;
                    if (_useCounter == 0)
                    {
                        _runner.Dispose();
                        _runner = null;
                    }
                }
            }
        }
    }
}