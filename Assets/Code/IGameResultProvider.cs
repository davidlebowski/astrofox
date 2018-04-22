namespace Astrofox
{
	public interface IGameResultProvider
	{
		int PreviousHighScore { get; }
		int CurrentHighScore { get; }
	}
}