namespace RusBlazor.Handlers
{
    public static class TooltipHandler
    {
        static Action<string>? _tooltipAction;
        public static void RegisterTooltipEventCallback(Action<string> action)
        {
            _tooltipAction = action;
        }

        public static void DisplayTooltip(string text)
        {
            if (_tooltipAction != null)
                _tooltipAction.Invoke(text);
        }

        public static void ResetTooltip()
        {
            if (_tooltipAction != null)
                _tooltipAction.Invoke("");
        }
    }
}