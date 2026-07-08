namespace Web.Services
{
    public enum Modal { None, Register, VerifyEmail, Login, ForgotPassword, ResetPassword, Filter, Settings }

    public class UiStateService
    {
        public Modal CurrentStep { get; private set; } = Modal.None;
        public string? PendingEmail { get; private set; }
        public event Action? OnChange;

        public void Show(Modal step, string? email = null)
        {
            CurrentStep = step;
            if (email is not null) PendingEmail = email;
            OnChange?.Invoke();
        }
    }
}