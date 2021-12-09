using System;
using CustomTowerDefense.Shared;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.GameObjects.Missiles
{
    /// <summary>
    /// Base class with specific behaviors for all missiles.
    /// </summary>
    public abstract class Missile: MoveableGameObject, IAutonomousBehavior
    {
        protected Missile(Coordinate coordinate, int width, int height, PreciseObjectType preciseObjectType, float speed, int drawOrder) :
            base(coordinate, width, height, preciseObjectType, speed, drawOrder)
        {
        }

        public void DoCurrentAction(GameTime gameTime)
        {
            Move(Direction * Speed);
        }
    }
}