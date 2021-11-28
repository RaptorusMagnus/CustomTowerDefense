using CustomTowerDefense.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CustomTowerDefense.GameObjects
{
    /// <summary>
    /// Base type for all drawable objects in the game.
    /// A GameObject is the conceptual representation of a game element with all its properties.
    /// Note that an object mustn't draw itself (we don't want any reference to an external sprite batch).
    /// The Game scene is in charge of actually drawing objects. 
    /// Note as well that we did not store the physical texture within the object,
    /// because the load content phase comes after the initialization phase. We don't want half initialized objects in our game.
    /// </summary>
    public class GameObject
    {
        private Coordinate _currentCoordinate;
        
        private int RotationPointDistanceX => Width / 2;
        private int RotationPointDistanceY => Height / 2;
        
        #region ----- Public Properties -----

        public PreciseObjectType PreciseObjectType { get; }

        public float RotationAngle { get; set; }
        
        public virtual Coordinate CurrentCoordinate
        {
            get => _currentCoordinate;
            set => _currentCoordinate = value;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        /// <summary>
        /// Current color effect, applied on the object. Color.White == no effect
        /// </summary>
        public Color CurrentColorEffect { get; set; }

        public Vector2 RotationOrigin => new Vector2(Width / 2, Height / 2);

        public Rectangle BoundaryRect
        {
            get
            {
                var topLeftX = (int)CurrentCoordinate.X - (int)RotationOrigin.X;
                var topLeftY = (int)CurrentCoordinate.Y - (int)RotationOrigin.Y;

                return new Rectangle(
                    topLeftX, topLeftY,
                    Width, Height
                );
            }

        }

        #endregion // Properties

        #region ----- Constructors -----

        public GameObject(
            Coordinate coordinate,
            int width,
            int height,
            PreciseObjectType preciseObjectType)
        {
            _currentCoordinate = coordinate;
            Width = width;
            Height = height;
            CurrentColorEffect = Color.White;
            PreciseObjectType = preciseObjectType;
        }

        #endregion // Constructors

        /// <summary>
        /// Corrects the coordinates to avoid going outside of the map.
        /// Placing the checks in this method, avoids the necessity to run them manually at every single movement
        /// </summary>
        public void CorrectCoordinateAfterOuterBoundControl(int screenWidth, int screenHeight)
        {
            float newX = CurrentCoordinate.X;
            float newY = CurrentCoordinate.Y;

            if ((CurrentCoordinate.X + RotationPointDistanceX) > screenWidth)
            {
                newX = screenWidth - RotationPointDistanceX;
            }
            else if ((CurrentCoordinate.X - RotationPointDistanceX) < 0)
            {
                newX = RotationPointDistanceX;
            }

            if ((CurrentCoordinate.Y + RotationPointDistanceY) > screenHeight)
            {
                newY = screenHeight - RotationPointDistanceY;
            }
            else if ((CurrentCoordinate.Y - RotationPointDistanceY) < 0)
            {
                newY = RotationPointDistanceY;
            }

            CurrentCoordinate = new Coordinate(newX, newY);
        }

        public Vector2 GetCurrentCoordinateAsVector()
        {
            return new Vector2(CurrentCoordinate.X, CurrentCoordinate.Y);
        }

        public Rectangle GetRectangle()
        {
            return new((int)CurrentCoordinate.X, (int)CurrentCoordinate.Y, Width, Height);
        }
        
        public Rectangle GetScaledRectangle(float scale)
        {
            return new((int)CurrentCoordinate.X, (int)CurrentCoordinate.Y, (int) (Width * scale), (int) (Height * scale));
        }
    }
}