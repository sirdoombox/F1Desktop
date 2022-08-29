using System.Reflection;
using StyletIoC;

namespace F1Desktop.Misc.Extensions;

public static class StyletIoCExtensions
{
    public static void BindAllImplementers<T>(this IStyletIoCBuilder builder, bool singletonScope = false)
    {
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsSubclassOf(typeof(T))))
        {
            if (type.IsAbstract) continue;
            var binding = builder.Bind<T>().To(type);
            if (!singletonScope) continue;
            binding.InSingletonScope();
        }
    }
}