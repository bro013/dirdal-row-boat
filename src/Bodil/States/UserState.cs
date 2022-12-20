namespace Bodil.States
{
    public class UserState : BaseState
    {
        private Guid _userId = default;
        public Guid UserId
        {
            get { return _userId; }
            set { _userId = value; NotifyStateChanged(); }
        }

        public bool IsLoggedIn => _userId != default;
    }
}
