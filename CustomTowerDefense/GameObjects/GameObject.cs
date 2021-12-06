using CustomTowerDefense.Shared;
using Microsoft.Xna.Framework;

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
        #region Private fields

        private Coordinate _coordinate;
        
        private int RotationPointDistanceX => Width / 2;
        private int RotationPointDistanceY => Height / 2;

        #endregion
        
        
        #region ----- Public Properties -----
        
        /// <summary>
        /// To be able to sort objects list in the correct order when drawing all elements.
        /// e.g. structure elements and vortexes must be drawn first and spaceships above.
        /// </summary>
        public int DrawOrder { get; set; }

        public PreciseObjectType PreciseObjectType { get; }

        public float RotationAngle { get; set; }
        
        public virtual Coordinate Coordinate
        {
            get => _coordinate;
            set => _coordinate = value;
        }

        /// <summary>
        /// Object width (texture width in fact),
        /// because we don't have the texture in this object, as explained in the class comment 
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Object Height (texture height in fact),
        /// because we don't have the texture in this object, as explained in the class comment 
        /// </summary>
        public int Height { get; }
        
        /// <summary>
        /// Current scale applied to the object
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Current color effect, applied on the object. Color.White == no effect
        /// </summary>
        public Color ColorEffect { get; set; }

        public Vector2 RotationOrigin => new Vector2(Width / 2, Height / 2);

        public Rectangle BoundaryRect
        {
            get
            {
                var topLeftX = (int)Coordinate.X - (int)RotationOrigin.X;
                var topLeftY = (int)Coordinate.Y - (int)RotationOrigin.Y;

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
            PreciseObjectType preciseObjectType,
            int drawOrder)
        {
            _coordinate = coordinate;
            Width = width;
            Height = height;
            ColorEffect = Color.White;
            PreciseObjectType = preciseObjectType;
            Scale = 1f;
            DrawOrder = drawOrder;
        }

        #endregion // Constructors

        /// <summary>
        /// Corrects the coordinates to avoid going outside of the map.
        /// Placing the checks in this method, avoids the necessity to run them manually at every single movement
        /// </summary>
        public void CorrectCoordinateAfterOuterBoundControl(int screenWidth, int screenHeight)
        {
            float newX = Coordinate.X;
            float newY = Coordinate.Y;

            if ((Coordinate.X + RotationPointDistanceX) > screenWidth)
            {
                newX = screenWidth - RotationPointDistanceX;
            }
            else if ((Coordinate.X - RotationPointDistanceX) < 0)
            {
                newX = RotationPointDistanceX;
            }

            if ((Coordinate.Y + RotationPointDistanceY) > screenHeight)
            {
                newY = screenHeight - RotationPointDistanceY;
            }
            else if ((Coordinate.Y - RotationPointDistanceY) < 0)
            {
                newY = RotationPointDistanceY;
            }

            Coordinate = new Coordinate(newX, newY);
        }

        public Vector2 GetCurrentCoordinateAsVector()
        {
            return new Vector2(Coordinate.X, Coordinate.Y);
        }

        public Rectangle GetRectangle()
        {
            return new((int)Coordinate.X, (int)Coordinate.Y, Width, Height);
        }
        
        public Rectangle GetScaledRectangle(float scale)
        {
            return new((int)Coordinate.X, (int)Coordinate.Y, (int) (Width * scale), (int) (Height * scale));
        }
    }
}