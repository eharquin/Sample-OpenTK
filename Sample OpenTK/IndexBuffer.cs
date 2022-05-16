using OpenTK.Graphics.OpenGL4;

namespace Sample_OpenTK
{
    public class IndexBuffer : IDisposable
    {
        bool isDisposed;
        bool isStatic;

        public readonly int Handle;

        public static readonly int MinIndexCount = 3;
        public static readonly int MaxIndexCount = 250_000;

        public IndexBuffer(bool isStatic = true)
        {
            this.isStatic = isStatic;

            Handle = GL.GenBuffer();
        }

        public void SetData(int[] indices, int count)
        {
            if (indices == null)
                throw new ArgumentNullException(nameof(indices));

            if (count < VertexBuffer.MinVertexCount)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (count > VertexBuffer.MaxVertexCount)
                throw new ArgumentOutOfRangeException(nameof(count));

            BufferUsageHint usageHint = BufferUsageHint.StaticDraw;
            if (!isStatic)
                usageHint = BufferUsageHint.DynamicDraw;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, count * sizeof(int), indices, usageHint);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Use()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle);
        }

        ~IndexBuffer()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.DeleteBuffer(Handle);

            GC.SuppressFinalize(this);
        }
    }
}