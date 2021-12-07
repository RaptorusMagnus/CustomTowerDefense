using CustomTowerDefense.Shared;

namespace CustomTowerDefense.GameObjects.DefenseTurrets
{
    public abstract class DefenseTurret: GameObject, IAutonomousBehavior
    {
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
            float rotationSpeed)
            : base(coordinate, width, height, preciseObjectType, drawOrder)
        {
            SightRange = sightRange;
            RotationSpeed = rotationSpeed;
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
        public void DoCurrentAction()
        {
            // TODO: Check for enemies within the sight range
            
            // TODO: adapt current rotation to face the enemy if possible with the rotation speed
            
            // TODO: Fire if the enemy is in front
        }
    }
}