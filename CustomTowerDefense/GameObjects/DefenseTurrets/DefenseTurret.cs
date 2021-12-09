using System;
using System.Linq;
using CustomTowerDefense.GameGrids;
using CustomTowerDefense.GameObjects.Missiles;
using CustomTowerDefense.GameObjects.SpaceShips;
using CustomTowerDefense.Helpers;
using CustomTowerDefense.Shared;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.GameObjects.DefenseTurrets
{
    public abstract class DefenseTurret: GameObject, IAutonomousBehavior
    {
        #region Private members

        // To code some behaviors we need information concerning the surroundings
        private readonly LogicalGameGridMultiple _logicalGameGrid;
        
        private TimeSpan _timeSpanSinceLastFiring = TimeSpan.Zero;
        private ushort _firingDelay;

        #endregion
        
        /// <summary>
        /// How far the turret can see (in pixels).
        /// Note that missiles may go further than the sight range when fired,
        /// but turrets will only turn and target spaceships that are within the sight range.
        /// </summary>
        public ushort SightRange { get; }
        
        /// <summary>
        /// Max rotation angle increment that can be added in a single update cycle
        /// </summary>
        public float RotationSpeed { get; }
        
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="coordinate"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="preciseObjectType"></param>
        /// <param name="drawOrder"></param>
        /// <param name="sightRange"></param>
        /// <param name="rotationSpeed"></param>
        /// <param name="firingDelay">Number of milliseconds.</param>
        /// <param name="logicalGameGrid"></param>
        public DefenseTurret(
            Coordinate coordinate,
            int width,
            int height,
            PreciseObjectType preciseObjectType,
            int drawOrder,
            ushort sightRange,
            float rotationSpeed,
            ushort firingDelay,
            LogicalGameGridMultiple logicalGameGrid)
            : base(coordinate, width, height, preciseObjectType, drawOrder)
        {
            SightRange = sightRange;
            RotationSpeed = rotationSpeed;
            _logicalGameGrid = logicalGameGrid;
            _firingDelay = firingDelay;
        }

        /// <inheritdoc cref="IAutonomousBehavior.DoCurrentAction"/>
        public void DoCurrentAction(GameTime gameTime)
        {
            // We must find the best target among all the spaceships.
            // So, we filter to keep spaceships only
            var spaceShips = _logicalGameGrid.GameObjects.OfType<SpaceShip>();
            
            // So which ones are in our sight range?
            var inSight =
                spaceShips.Where(ship => Vector2.Distance(ship.GetCurrentCoordinateAsVector(),
                                                          GetCurrentCoordinateAsVector()) <= SightRange).ToList();
            
            if (!inSight.Any())
                return;

            // TODO: The "intelligence" rules below could be changed by an option selected by the player.
            // e.g. maybe it's not always the best solution to follow the shit that is the closest to the end vortex.
            
            // From all the enemies in sight, we must focus on the one that is further than the others,
            // to avoid its escape in the end vortex (Note that when they are in the vortex, it's too late)
            var target = inSight.Where(s => s.CurrentAction != SpaceshipAction.GoingInVortex)
                                .OrderByDescending(s => ((SpaceShip) s).CurrentPathIndex).First();

            var angleToReachTarget = AnglesHelper.GetAngleToReachTarget(GetCurrentCoordinateAsVector(), target.GetCurrentCoordinateAsVector());
            var rotationDifference = Math.Abs(RotationAngle - angleToReachTarget);

            if (rotationDifference <= RotationSpeed)
            {
                RotationAngle = angleToReachTarget;
            }
            else
            {
                if (RotationAngle > angleToReachTarget)
                {
                    RotationAngle -= RotationSpeed;
                }
                else
                {
                    RotationAngle += RotationSpeed;
                }
            }
            
            _timeSpanSinceLastFiring = _timeSpanSinceLastFiring.Add(gameTime.ElapsedGameTime);
            
            if (_timeSpanSinceLastFiring.TotalMilliseconds >= _firingDelay)
            {
                var newMissile = new DoubleGunsTurretMissile(Coordinate, _logicalGameGrid)
                {
                    RotationAngle = RotationAngle,
                    Direction = AnglesHelper.AngleToVector(RotationAngle)
                };
                
                // Currently the missile is located in the center of the turret.
                // We move it a bit, so that it goes at the edge of the turret sprite.
                newMissile.Move(newMissile.Direction * _logicalGameGrid.TilesSize / 2);
                
                _logicalGameGrid.AddMissile(newMissile);
                
                // TODO: Fire if the enemy is in front or nearly in front (Tolerance angle to be determined)
                
                _timeSpanSinceLastFiring = TimeSpan.Zero;
            }
        }
    }
}