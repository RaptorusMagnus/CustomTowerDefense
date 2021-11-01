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
        #region ----- Properties -----

        public int RotationPointDistanceX => Width / 2;
        public int RotationPointDistanceY => Height / 2;

        public Coordinate CurrentCoordinate { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        /// <summary>
        /// Image path and name (e.g. "Images/Tank1")
        /// Will be used by the loadContent method to fetch the correct Texture2D
        /// </summary>
        public string TextureImagePath { get; }

        /// <summary>
        /// This field will contain null, until the LoadContent method is executed
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Current color effect, applied on the object. Color.White == no effect
        /// </summary>
        public Color CurrentColorEffect { get; set; }

        public Vector2 RotationVector => new Vector2(Width / 2, Height / 2);

        public Rectangle BoundingRect
        {
            get
            {
                int topLeftX = CurrentCoordinate.X - (int)RotationVector.X;
                int topLeftY = CurrentCoordinate.Y - (int)RotationVector.Y;

                return new Rectangle(
                    topLeftX, topLeftY,
                    Width, Height
                );
            }

        }

        #endregion // Properties

        #region ----- Constructors -----

        public GameObject(Coordinate coordinate, int width, int height, string imagePath)
        {
            CurrentCoordinate = coordinate;
            Width = width;
            Height = height;
            TextureImagePath = imagePath;
            CurrentColorEffect = Color.White;
        }

        #endregion // Constructors

        /// <summary>
        /// Corrects the coordinates to avoid going outside of the map.
        /// Placing the checks in this method, avoids the necessity to run them manually at every single movement
        /// </summary>
        public void CorrectCoordinateAfterOuterBoundControl(int screenWidth, int screenHeight)
        {
            int newX = CurrentCoordinate.X;
            int newY = CurrentCoordinate.Y;

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
            return new Rectangle(CurrentCoordinate.X, CurrentCoordinate.Y, Width, Height);
        }
    }
}