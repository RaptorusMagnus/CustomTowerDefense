using System;
using System.Linq;
using CustomTowerDefense.GameGrids;
using CustomTowerDefense.GameObjects.SpaceShips;
using CustomTowerDefense.Shared;
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
        
        protected Missile(
            Coordinate coordinate,
            int width,
            int height,
            PreciseObjectType preciseObjectType,
            float speed,
            int drawOrder,
            LogicalGameGridMultiple gameGrid) :
            base(coordinate, width, height, preciseObjectType, speed, drawOrder)
        {
            _logicalGameGrid = gameGrid;
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
                    //
                    //         \`-"'"-'/
                    //          } 6 6 {
                    //         =.  Y  ,=
                    //           /^^^\  .
                    //          /     \  )
                    //         (  )-(  )/
                    //          ""   ""  
                    //
                    // TODO: This is just a test, the missile must not kow what to do with the target, but must only notify the screen that a collision occurred
                    spaceship.ColorEffect = new Color(Math.Clamp(spaceship.ColorEffect.R + 10, 0, 255),
                                                        Math.Clamp(spaceship.ColorEffect.G - 10, 0, 255),
                                                        Math.Clamp(spaceship.ColorEffect.B - 10, 0, 255),
                                                        spaceship.ColorEffect.A);

                    // To be destroyed by the screen
                    Coordinate = new Coordinate(-1, -1);
                }
            }
        }
    }
}