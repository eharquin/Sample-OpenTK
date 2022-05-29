using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;


namespace Sample_OpenTK
{
    public class Renderer
    {
        public Renderer() {}

        public void ClearColor(Color4 color)
        {
            GL.ClearColor(color);
        }

        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Draw(VertexArray vertexArray, IndexBuffer indexBuffer, ShaderProgram shaderProgram)
        {
            vertexArray.Use();
            indexBuffer.Use();
            shaderProgram.Use();
            GL.DrawElements(PrimitiveType.Triangles, indexBuffer.Size, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }

        public void Draw(VertexArray vertexArray, IndexBuffer indexBuffer, ShaderProgram shaderProgram, Texture texture)
        {
            texture.Use();
            vertexArray.Use();
            indexBuffer.Use();
            shaderProgram.Use();
            GL.DrawElements(PrimitiveType.Triangles, indexBuffer.Size, DrawElementsType.UnsignedInt, IntPtr.Zero);
        }
    }
}
