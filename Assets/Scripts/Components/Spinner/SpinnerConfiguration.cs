public class SpinnerConfiguration : ISpinnerConfiguration
{
    public SpinnerType SpinnerType => SpinnerType.Wheel;
    
    float _hit;
    float _crit;

    public SpinnerConfiguration(float hit, float crit)
    {
        _hit = hit;
        _crit = crit;
    }

    public float Hit()
    {
        return _hit;
    }

    public float Crit()
    {
        return _crit;
    }
}