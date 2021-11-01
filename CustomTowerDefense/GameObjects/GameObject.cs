using CustomTowerDefense.ValueObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CustomTowerDefense.GameObjects
{
/// <summary>
    /// Base type for all objects in the game
    /// </summary>
    public class GameObject
    {
        private Coordinate _currentCoordinate;
        
        private int RotationPointDistanceX => Width / 2;
        private int RotationPointDistanceY => Height / 2;
        
        #region ----- Public Properties -----
        
        /// <summary>
        /// 
        /// </summary>
        public PreciseObjectType PreciseObjectType { get; }

        public float RotationAngle { get; protected set; }
        
        public virtual Coordinate CurrentCoordinate
        {
            get => _currentCoordinate;
            set => _currentCoordinate = value;
        }

        public int Width { get; set; }

        public int Height { get; set; }

        /// <summary>
        /// Image path and name (e.g. "Images/Tank1")
        /// Will be used by the loadContent method to fetch the correct Texture2D
        /// </summary>
        public string TextureImagePath { get; }

        /// <summary>
        /// Current color effect, applied on the object. Color.White == no effect
        /// </summary>
        public Color CurrentColorEffect { get; set; }

        public Vector2 RotationVector => new Vector2(Width / 2, Height / 2);

        public Rectangle BoundaryRect
        {
            get
            {
                var topLeftX = (int)CurrentCoordinate.X - (int)RotationVector.X;
                var topLeftY = (int)CurrentCoordinate.Y - (int)RotationVector.Y;

                return new Rectangle(
                    topLeftX, topLeftY,
                    Width, Height
                );
            }

        }

        #endregion // Properties

        #region ----- Constructors -----

        public GameObject(Coordinate coordinate, int width, int height, string imagePath, PreciseObjectType preciseObjectType)
        {
            _currentCoordinate = coordinate;
            Width = width;
            Height = height;
            TextureImagePath = imagePath;
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
            return new Rectangle((int)CurrentCoordinate.X, (int)CurrentCoordinate.Y, Width, Height);
        }
    }
}