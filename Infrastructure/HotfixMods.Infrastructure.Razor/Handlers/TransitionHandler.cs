using MudBlazor;

namespace HotfixMods.Infrastructure.Razor.Handlers
{
    public static class TransitionHandler
    {
        public static Transition Transition { get; set; } = Transition.Custom;
        public static string EnterTransition { get; set; } = "";
        public static string ExitTransition { get; set; } = "";

        public static void SetToSlideNavigation()
        {
            Transition = Transition.Slide;
        }

        public static void SetToNavigateLeftBySlide()
        {
            Transition = Transition.Custom;
            EnterTransition = "transition-slide-enter-right";
            ExitTransition = "transition-slide-exit-left";
        }

        public static void SetToNavigateRightBySlide()
        {
            Transition = Transition.Custom;
            EnterTransition = "transition-slide-enter-left";
            ExitTransition = "transition-slide-exit-right";
        }

        public static void SetToNavigateInstance()
        {
            Transition = Transition.Custom;
            EnterTransition = "transition-instance";
            ExitTransition = "transition-fade";
        }

        public static void SetToNavigateTab()
        {
            Transition = Transition.Custom;
            EnterTransition = "transition-tab";
            ExitTransition = "transition-fade";
        }

        public static void SetToRemoveInstance()
        {
            Transition = Transition.Custom;
            EnterTransition = "transition-fade";
            ExitTransition = "transition-page-remove";
        }

        public static void Reset()
        {
            Transition = Transition.Custom;
            EnterTransition = "";
            ExitTransition = "";
        }
    }
}
