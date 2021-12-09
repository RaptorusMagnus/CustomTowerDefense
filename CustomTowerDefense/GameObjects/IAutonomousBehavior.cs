using System;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.GameObjects
{
    /// <summary>
    /// For game objects that have an autonomous behavior that just needs to be triggered,
    /// without having to update inner properties.
    /// </summary>
    public interface IAutonomousBehavior
    {
        /// <summary>
        /// Triggers one more step for the action that was being done by the game object.
        /// Something like "Go on doing what ever you were doing". e.g. moving, firing, following the path,...
        /// This method is supposed to be called in the Update refresh cycle.
        /// </summary>
        void DoCurrentAction(GameTime gameTime);
    }
}
