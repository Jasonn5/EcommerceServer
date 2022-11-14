namespace Authentication.Services.Interfaces
{
    public interface IPasswordService
    {
        string GeneratePassword(int length);
    }
}
