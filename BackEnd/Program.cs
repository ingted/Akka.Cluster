﻿namespace Backend
{
    using System;
    using Akka.Actor;
    using Akka.Configuration;
    using Backend.Actor;

    internal class Program
    {
        internal static IActorRef Tasker;

        private static void Main(string[] args)
        {
            Console.Title = "BackEnd";
            var config = ConfigurationFactory.ParseString(@"
akka
    {
    loglevel = INFO
    loggers=[""Akka.Logger.NLog.NLogLogger, Akka.Logger.NLog""]
    actor {
    provider=cluster
        debug
            {
              receive = on      # log any received message
              autoreceive = on  # log automatically received messages, e.g. PoisonPill
              lifecycle = on    # log actor lifecycle changes
              event-stream = on # log subscription changes for Akka.NET event stream
              unhandled = on    # log unhandled messages sent to actors
            }
        }
    remote
        {
        dot-netty.tcp {
        port = 0
        hostname = ""localhost""
        }
    }
    cluster {
        seed-nodes = [""akka.tcp://ClusterSystem@localhost:8081""]
        roles=[""backend""]
        }
}
");
            var system = ActorSystem.Create("ClusterSystem", config);
            Tasker = system.ActorOf(Props.Create<TaskActor>(), "tasker");
            Console.Read();
        }
    }
}