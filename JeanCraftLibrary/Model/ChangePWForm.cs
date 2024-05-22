namespace JeanCraftLibrary.Model
{
    public class ChangePWForm
    {
        public Guid UserID { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}
