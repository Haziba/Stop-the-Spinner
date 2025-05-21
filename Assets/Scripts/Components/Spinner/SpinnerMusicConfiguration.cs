public class SpinnerMusicConfiguration : ISpinnerConfiguration
{
    public SpinnerType SpinnerType => SpinnerType.Music;
    public int Notes { get; }

    public SpinnerMusicConfiguration(int notes)
    {
        Notes = notes;
    }
}