using System;
using System.Linq;
using CustomTowerDefense.GameGrids;
using CustomTowerDefense.GameObjects.SpaceShips;
using CustomTowerDefense.Shared;
using JetBrains.Annotations;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.GameObjects.Missiles
{
    /// <summary>
    /// Base class with specific behaviors for all missiles.
    /// </summary>
    public abstract class Missile: MoveableGameObject, IAutonomousBehavior
    {
        // To code some behaviors we need information concerning the surroundings
        private readonly LogicalGameGridMultiple _logicalGameGrid;

        public bool HasHitTarget => HitSpaceShip != null;
        
        [CanBeNull]
        public SpaceShip HitSpaceShip { get; private set; } 
        
        /// <summary>
        /// Number of hit points removed from the target after collision.
        /// Note that this figure may be lessen or upped at the end, when some special rules apply. 
        /// </summary>
        public ushort DamagePoints { get; private set; } 
        
        protected Missile(
            Coordinate coordinate,
            int width,
            int height,
            PreciseObjectType preciseObjectType,
            float speed,
            int drawOrder,
            ushort damagePoints,
            LogicalGameGridMultiple gameGrid) :
            base(coordinate, width, height, preciseObjectType, speed, drawOrder)
        {
            _logicalGameGrid = gameGrid;
            DamagePoints = damagePoints;
        }

        public void DoCurrentAction(GameTime gameTime)
        {
            Move(Direction * Speed);

            // Let's do some collision detection with spaceships.
            var spaceShips = _logicalGameGrid.GameObjects.OfType<SpaceShip>();
            
            // Since we could have quite a lot of spaceships, we can restrict the list to those that are not too far
            var closeSpaceships = spaceShips.Where(s => s.Coordinate.X < Coordinate.X + _logicalGameGrid.TilesSize &&
                                                        s.Coordinate.Y < Coordinate.Y + _logicalGameGrid.TilesSize &&
                                                        s.Coordinate.X > Coordinate.X - _logicalGameGrid.TilesSize &&
                                                        s.Coordinate.Y > Coordinate.Y - _logicalGameGrid.TilesSize);

            foreach (var spaceship in closeSpaceships)
            {
                if (spaceship.BoundaryRect.Intersects(BoundaryRect))
                {
                    // To be destroyed by the screen
                    HitSpaceShip = spaceship;
                }
            }
        }
    }
}