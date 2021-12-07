namespace CustomTowerDefense.GameObjects
{
    /// <summary>
    /// For game objects that have an autonomous behavior that just needs to be triggered,
    /// without having to update inner properties.
    /// </summary>
    public interface IAutonomousBehavior
    {
        void DoCurrentAction();
    }
}