using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Vector = System.Windows.Vector;

namespace KugelnUndGitter
{
    public partial class Form1 : Form
    {
        private int cellSize;
        private int columns;
        private int rows;
        private Vector[,] points;
        private List<Sphere> spheres;
        Random r = new Random();

        public Form1()
        {
            InitializeComponent();
            cellSize = 20;
            columns = pictureBox.Width / cellSize;
            rows = pictureBox.Height / cellSize;
            points = new Vector[columns + 1, rows + 1];
            spheres = new List<Sphere>();

            CreatePoints();
            CreateStartingSpheres();
            timer.Start();

        }


        // Herr Mouton: Bitte kommentieren Sie diese Funktion.
        // Sind die Berechnungen korrekt?
        // In der pictureBox fehlen manchmal Linien.
        private void TransformPoints()
        {
            foreach(Sphere sphere in spheres)
            {
                for(int i = 0; i <= columns; i++)
                {
                    for(int j = 0; j <= rows; j++)
                    {
                        Vector pointLocation = points[i, j];
                        Vector sphereLocation = sphere.Position;

                        int radius = sphere.Radius * sphere.Radius;

                        double distance = 
                            (pointLocation.X - sphereLocation.X) * (pointLocation.X - sphereLocation.X) + 
                            (pointLocation.Y - sphereLocation.Y) * (pointLocation.Y - sphereLocation.Y);

                        if(distance <= radius && sphereLocation != pointLocation)
                        {
                            int maxTransform = sphere.Radius * 2;

                            double transform = maxTransform * Math.Sqrt(1.0 - (sphere.Radius * sphere.Radius / (distance * distance)));

                            points[i, j].X += transform * (pointLocation.X - sphereLocation.X) / distance;
                            points[i, j].Y += transform * (pointLocation.Y - sphereLocation.Y) / distance;
                        }

                    }
                }
                
            }
        }

        private void CreateStartingSpheres()
        {
            spheres.Add(new Sphere(new Vector(0, r.Next(1, pictureBox.Height)), new Vector(r.Next(10, 20), 0), r.Next(20, 100)));
            spheres.Add(new Sphere(new Vector(r.Next(1, pictureBox.Width), 0), new Vector(0, r.Next(10, 20)), r.Next(20, 100)));
            spheres.Add(new Sphere(new Vector(0, r.Next(1, pictureBox.Height)), new Vector(r.Next(10, 20), 0), r.Next(20, 100)));
        }

        private void RandomizePoints(int min, int max)
        {
            Vector[,] randomPoints = points;

            for (int x = 0; x <= columns; ++x)
            {
                for (int y = 0; y <= rows; ++y)
                {
                    randomPoints[x, y] = new Vector(
                        (x * cellSize) + r.Next(min, max), 
                        (y * cellSize) + r.Next(min, max));
                }
            }
            points = randomPoints;
        }

        private void CreatePoints()
        {
            // Create columns
            for (int x = 0; x <= columns; ++x)
            {
                // Create rows
                for (int y = 0; y <= rows; ++y)
                {
                    points[x, y] = new Vector(x * cellSize, y * cellSize);
                }
            }
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black);

            for(int x = 0; x < columns; ++x)
            {
                for(int y = 0; y < rows; ++y)
                {
                    // Draw columns
                    e.Graphics.DrawLine(pen,
                                        (float)points[x, y].X,
                                        (float)points[x, y].Y,
                                        (float)points[x + 1, y].X,
                                        (float)points[x + 1, y].Y);
                    // Draw rows
                    e.Graphics.DrawLine(pen,
                                        (float)points[x, y].X,
                                        (float)points[x, y].Y,
                                        (float)points[x, y + 1].X,
                                        (float)points[x, y + 1].Y);
                }
            }
        }

        public bool GetRandomBoolean()
        {
            return r.Next(0, 2) == 0;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            CreatePoints();
            TransformPoints();

            for(int i = 0; i < spheres.Count; i++)
            {
                if((spheres[i].Position.X - spheres[i].Radius) > pictureBox.Width || (spheres[i].Position.Y - spheres[i].Radius) > pictureBox.Height)
                {
                    spheres.Remove(spheres[i]);

                    bool sphereFromTop = GetRandomBoolean();
                    if (sphereFromTop)
                    {
                        spheres.Add(new Sphere(new Vector(r.Next(1, pictureBox.Width), 0), new Vector(0, r.Next(10, 20)), r.Next(20, 100)));
                    }
                    else
                    {
                        spheres.Add(new Sphere(new Vector(0, r.Next(1, pictureBox.Height)), new Vector(r.Next(10, 20), 0), r.Next(20, 100)));
                    }
                }
                spheres[i].move();
            }
            pictureBox.Refresh();

        }
    }
}
