namespace CustomTowerDefense.GameObjects.SpaceShips
{
    /// <summary>
    /// All available actions a spaceship can do
    /// </summary>
    public enum SpaceshipAction
    {
        GoingOutOfVortex,
        GoingInVortex,
        MoveToNextPathLocation,
        
        /// <summary>
        /// when hit points have reached zero
        /// </summary>
        Exploding,
        
        /// <summary>
        /// when hit points have reached zero,
        /// and when the explosion animation is finished
        /// </summary>
        ToBeRemovedFromGame
    }
}