public class MasterTuner : AudioTuner
{
    private string _masterVolumeName = "Master Volume";

    protected override void Awake()
    {
        base.Awake();
        TunerName = _masterVolumeName;
    }
}