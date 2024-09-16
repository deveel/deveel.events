using BenchmarkDotNet.Running;

using DeveelEventsBenchmarks;

var summary = BenchmarkRunner.Run<PublishBenchmarks>();
