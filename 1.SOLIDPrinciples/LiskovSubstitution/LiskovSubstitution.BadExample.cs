namespace _1.SOLIDPrinciples.LiskovSubstitution
{
    public class RectangleBad
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public RectangleBad() { }

        public RectangleBad(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"{nameof(Width)}: {Width}, {nameof(Height)}: {Height}";
        }
    }

    public class SquareBad : Rectangle
    {
        public new int Width
        {
            set
            {
                base.Width = base.Height = value;
            }
        }

        public new int Height
        {
            set
            {
                base.Width = base.Height = value;
            }
        }
    }

    // Square instance can be replaced by rectangle instance but if they are not market as virtual it won't check in child classes for the appropiate values
}
