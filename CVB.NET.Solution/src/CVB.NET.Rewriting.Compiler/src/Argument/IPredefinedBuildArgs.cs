namespace CVB.NET.Rewriting.Compiler.Argument
{
    using Reflection.Caching.Cached;

    public interface IPredefinedBuildArgs
    {
        CachedType CompilerDomainBootstrapperType { get; }

        /// <summary>
        /// Allows you to attach the debugger to a compilation unit that you have the source for while the compilation API is hosted inside a build engine.
        /// </summary>
        bool AttachDebugger { get; set; }
    }
}