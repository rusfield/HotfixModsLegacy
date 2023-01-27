
namespace HotfixMods.Infrastructure.Helpers
{
    public static class LoadingHelper
    {
        public static Func<int> GetLoaderFunc(int totalInvokes)
        {
            int currentInvoke = 1;
            Func<int> increaseProgress = () => Math.Min(currentInvoke++ * 100 / totalInvokes, 99);
            return increaseProgress;
        }

        public static string Saving = "Saving";
        public static string Loading = "Loading";
        public static string Deleting = "Deleting";
        public static string Error = "Error";
    }
}
