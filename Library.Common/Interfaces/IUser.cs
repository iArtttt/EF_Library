namespace Library.Shared.Interfaces
{
	public interface IUser : IID
    {
		public string Login { get; set; }
		public string Password { get; set; }
		public string? Email { get; set; }
	}
}
