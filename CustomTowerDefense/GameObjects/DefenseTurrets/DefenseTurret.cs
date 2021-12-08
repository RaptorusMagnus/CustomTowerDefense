using System;
using System.Linq;
using CustomTowerDefense.GameGrids;
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
        
        public DefenseTurret(
            Coordinate coordinate,
            int width,
            int height,
            PreciseObjectType preciseObjectType,
            int drawOrder,
            ushort sightRange,
            float rotationSpeed,
            LogicalGameGridMultiple logicalGameGrid)
            : base(coordinate, width, height, preciseObjectType, drawOrder)
        {
            SightRange = sightRange;
            RotationSpeed = rotationSpeed;
            _logicalGameGrid = logicalGameGrid;
        }

        //
        //              _.---._    /\\
        //           ./'       "--`\//
        //         ./              o \
        //        /./\  )______   \__ \
        //       ./  / /\ \   | \ \  \ \
        //          / /  \ \  | |\ \  \7
        //           "     "    "  "
        //
        /// <inheritdoc cref="IAutonomousBehavior.DoCurrentAction"/>
        public void DoCurrentAction()
        {
            // We must find the best target among all the spaceships.
            // So, we filter to keep spaceships only
            var spaceShips = _logicalGameGrid.GameObjects.Where(s => s is SpaceShip);
            
            // So which ones are in our sight range?
            var inSight =
                spaceShips.Where(ship => Vector2.Distance(ship.GetCurrentCoordinateAsVector(),
                                                          GetCurrentCoordinateAsVector()) <= SightRange).ToList();
            
            if (!inSight.Any())
                return;

            // From all the enemies in sight, we must focus on the one that is further than the others,
            // to avoid that it gets out in the end vortex
            var target = inSight.OrderByDescending(s => ((SpaceShip) s).CurrentPathIndex).First();

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
            
            // TODO: Fire if the enemy is in front
        }
    }
}