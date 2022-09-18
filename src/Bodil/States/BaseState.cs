namespace Bodil.States
{
    public class BaseState
    {
        public event Action? OnChange;
        protected void NotifyStateChanged() => OnChange?.Invoke();
    }
}
