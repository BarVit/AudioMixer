public class EffectsTuner : AudioTuner
{
    private string _effectsVolumeName = "Effects Volume";

    protected override void Awake()
    {
        base.Awake();
        TunerName = _effectsVolumeName;
    }
}