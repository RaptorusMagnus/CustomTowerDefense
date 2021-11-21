using System;
using CustomTowerDefense.Helpers;
using CustomTowerDefense.Shared;
using Microsoft.Xna.Framework;

namespace CustomTowerDefense.GameObjects
{
    public class MoveableGameObject: GameObject
    {
        #region ----- Fields -----

        Coordinate _currentCoordinate;

        #endregion // Fields

        #region ----- Properties -----

        /// <summary>
        /// Last precise time when the object was moved.
        /// This property is useful to avoid move cheats by typing fast on game inputs.
        /// You can control that the object is not moved faster than its max speed.
        /// </summary>
        public DateTime LastMoveTime { get; private set; }


        /// <summary>
        /// To know where the game object is heading
        /// </summary>
        public Vector2 CurrentDirection { get; set; }

        public override Coordinate CurrentCoordinate
        {
            get
            {
                return _currentCoordinate;
            }
            set
            {
                // we don't want to update the last move when there is no move
                if (value.Equals(_currentCoordinate))
                    return;

                _currentCoordinate = value;
                LastMoveTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Standard speed when moving the object.
        /// The value is the number of pixels at each move cycle.
        /// </summary>
        public float Speed { get; protected set; }

        #endregion // Properties

        #region ----- Constructors -----

        public MoveableGameObject(Coordinate coordinate, int width, int height, PreciseObjectType preciseObjectType, float speed)
            : base(coordinate, width, height, preciseObjectType)
        {
            LastMoveTime = DateTime.Now;
            CurrentDirection = Vector2.Zero;
            Speed = speed;
            _currentCoordinate = coordinate;
        }

        #endregion // constructors

        #region ----- Methods -----

        public void Move(Vector2 moveVector)
        {
            if (moveVector.X == 0 && moveVector.Y == 0)
                return;

            var newCoordinate = new Coordinate(CurrentCoordinate.X + (int)moveVector.X,
                                               CurrentCoordinate.Y + (int)moveVector.Y);

            CurrentCoordinate = newCoordinate;
            RotationAngle = AnglesHelper.VectorToAngle(moveVector);
        }

        #endregion // Methods
    }
}