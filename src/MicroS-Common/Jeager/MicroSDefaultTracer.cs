using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using OpenTracing;
using System.Reflection;

namespace MicroS_Common.Jeager
{
    public class MicroSDefaultTracer
    {
        public static ITracer Create()
            => new Tracer.Builder(Assembly.GetEntryAssembly().FullName)
                .WithReporter(new NoopReporter())
                .WithSampler(new ConstSampler(false))
                .Build();
    }
}
