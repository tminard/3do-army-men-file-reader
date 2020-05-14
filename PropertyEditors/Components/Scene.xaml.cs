using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AMMEdit.PropertyEditors.Components
{
    /// <summary>
    /// Interaction logic for Scene.xaml
    /// </summary>
    public partial class Scene : UserControl
    {
        public GeometryModel3D Model { get; private set; }

        public Scene()
        {
            InitializeComponent();

            BuildGround();
        }

        private void BuildGround()
        {
            MeshGeometry3D mesh = new MeshGeometry3D();

            // define all the points in terms of Line Segments
            // We have to duplicate the points to handle texture atlas stuff
      
            for (int t = 0; t < 256*256; t++)
            {
                int x = t % 256;
                int y = t / 256;

                int xOffset = x * 16;
                int yOffset = y * 16;

                // Points
                // clockwise
                int basePoint = mesh.Positions.Count;
                mesh.Positions.Add(new Point3D(xOffset, 0, yOffset)); // UL
                mesh.Positions.Add(new Point3D(xOffset + 16, 0, yOffset + 16)); // LR
                mesh.Positions.Add(new Point3D(xOffset, 0, yOffset + 16)); // LL

                mesh.Positions.Add(new Point3D(xOffset, 0, yOffset)); // UL
                mesh.Positions.Add(new Point3D(xOffset + 16, 0, yOffset)); // UR
                mesh.Positions.Add(new Point3D(xOffset + 16, 0, yOffset + 16)); // LR

                // texture atlas coords
                // TODO - this part is not fun

                // triangle indices - defined clockwise
                mesh.TriangleIndices.Add(basePoint);
                mesh.TriangleIndices.Add(basePoint + 1);
                mesh.TriangleIndices.Add(basePoint + 2);

                mesh.TriangleIndices.Add(basePoint + 3);
                mesh.TriangleIndices.Add(basePoint + 4);
                mesh.TriangleIndices.Add(basePoint + 5);
            }

            Model = new GeometryModel3D(mesh, null);
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
        }
    }
}
