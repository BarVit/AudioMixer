public class MusicTuner : AudioTuner
{
    private string _musicVolumeName = "Music Volume";

    protected override void Awake()
    {
        base.Awake();
        TunerName = _musicVolumeName;
    }
}