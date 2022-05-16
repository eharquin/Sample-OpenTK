using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample_OpenTK
{
    public class Cube
    {
        public VertexBuffer VertexBuffer;

        public Cube()
        {
            VertexPositionTexture[] vertices =
            {
                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, -0.5f),  new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(0.5f, -0.5f, -0.5f),  new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(0.5f,  0.5f, -0.5f),  new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(0.5f,  0.5f, -0.5f),  new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-0.5f,  0.5f, -0.5f),  new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, -0.5f),  new Vector2(0.0f, 0.0f)),

                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, 0.5f),  new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(0.5f, -0.5f, 0.5f),  new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(0.5f,  0.5f, 0.5f),  new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(0.5f,  0.5f, 0.5f),  new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-0.5f,  0.5f, 0.5f),  new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, 0.5f),  new Vector2(0.0f, 0.0f)),

                new VertexPositionTexture(new Vector3(-0.5f, 0.5f, 0.5f),  new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(-0.5f, 0.5f, -0.5f),  new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, -0.5f),  new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, -0.5f),  new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, 0.5f),  new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-0.5f, 0.5f, 0.5f),  new Vector2(0.0f, 0.0f)),

                new VertexPositionTexture(new Vector3(0.5f, 0.5f, 0.5f),  new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(0.5f, 0.5f, -0.5f),  new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(0.5f, -0.5f, -0.5f),  new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(0.5f, -0.5f, -0.5f),  new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(0.5f, -0.5f, 0.5f),  new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(0.5f, 0.5f, 0.5f),  new Vector2(0.0f, 0.0f)),

                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, -0.5f),  new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(0.5f, -0.5f, -0.5f),  new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(0.5f, -0.5f, 0.5f),  new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(0.5f, -0.5f, 0.5f),  new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, 0.5f),  new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, -0.5f),  new Vector2(0.0f, 0.0f)),

                new VertexPositionTexture(new Vector3(-0.5f, 0.5f, -0.5f),  new Vector2(0.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(0.5f, 0.5f, -0.5f),  new Vector2(1.0f, 0.0f)),
                new VertexPositionTexture(new Vector3(0.5f, 0.5f, 0.5f),  new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(0.5f, 0.5f, 0.5f),  new Vector2(1.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-0.5f, 0.5f, 0.5f),  new Vector2(0.0f, 1.0f)),
                new VertexPositionTexture(new Vector3(-0.5f, 0.5f, -0.5f),  new Vector2(0.0f, 0.0f))
            };
        }
    }
}
