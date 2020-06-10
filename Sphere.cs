using Vector = System.Windows.Vector;

namespace KugelnUndGitter
{
    public class Sphere
    {
        private Vector position;
        private Vector speed;
        private int radius;

        public Sphere(Vector position, Vector speed, int radius)
        {
            this.position = position;
            this.speed = speed;
            this.radius = radius;
        }

        public Vector Position { get => position; set => position = value; }
        public Vector Speed { get => speed; set => speed = value; }
        public int Radius { get => radius; set => radius = value; }

        public void move()
        {
            position += speed;
        }
    }
}
