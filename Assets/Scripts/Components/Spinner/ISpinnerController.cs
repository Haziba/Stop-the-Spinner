public interface ISpinnerController
{
    SpinnerType SpinnerType { get; }
    void UpdateConfig(ISpinnerConfiguration config);
    void StartSpinning(AgentStatusEffects statusEffects);
    SpinnerResult StopSpinning();
    bool IsSpinning();
}