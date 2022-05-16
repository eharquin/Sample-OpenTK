using OpenTK.Graphics.OpenGL4;

namespace Sample_OpenTK
{
    public class VertexArray : IDisposable
    {
        bool isDisposed;

        public readonly int Handle;

        public VertexArray(VertexBuffer vertexBuffer)
        {
            Handle = GL.GenVertexArray();
            GL.BindVertexArray(Handle);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBuffer.Handle);

            VertexInfo vertexInfo = vertexBuffer.VertexInfo;

            foreach (var attribute in vertexInfo.Attributes)
            {
                GL.VertexAttribPointer(attribute.Index, attribute.Size, attribute.PointerType, attribute.IsNormalize, vertexInfo.Size, attribute.Offset);
                GL.EnableVertexAttribArray(attribute.Index);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public void Use()
        {
            GL.BindVertexArray(Handle);
        }

        ~VertexArray()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;

            GL.BindVertexArray(0);
            GL.DeleteVertexArray(Handle);

            GC.SuppressFinalize(this);
        }
    }
}