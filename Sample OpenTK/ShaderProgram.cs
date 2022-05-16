using OpenTK.Graphics.OpenGL4;

namespace Sample_OpenTK
{
    public class ShaderProgram : IDisposable
    {
        bool isDisposed;

        public readonly int Handle;

        private readonly int vertexShaderHandle;
        private readonly int fragmentShaderHandle;

        public ShaderProgram(string vertexShaderCode, string fragmentShaderCode)
        {
            if(!CompileVertexShader(vertexShaderCode, out vertexShaderHandle, out string vertexShaderErrorMessage))
            {
                throw new ArgumentException(vertexShaderErrorMessage);
            }
            
            if(!CompileFragmentShader(fragmentShaderCode, out fragmentShaderHandle, out string fragmentShaderErrorMessage))
            {
                throw new ArgumentException(fragmentShaderErrorMessage);
            }

            Handle = CreateLinkProgram(vertexShaderHandle, fragmentShaderHandle);
            

            int[] viewport = new int[4];
            GL.GetInteger(GetPName.Viewport, viewport);

            GL.UseProgram(Handle);
            int viewportUniformLocation = GL.GetUniformLocation(Handle, "viewport");
            GL.Uniform2(viewportUniformLocation, (float)viewport[2], (float)viewport[3]);
            GL.UseProgram(0);

            Console.WriteLine();
        }

        private bool CompileVertexShader(string vertexShaderCode, out int vertexShaderHandle, out string errorMessage)
        {
            errorMessage = String.Empty;

            vertexShaderHandle = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShaderHandle, vertexShaderCode);

            GL.CompileShader(vertexShaderHandle);
            string info_log_vertex = GL.GetShaderInfoLog(vertexShaderHandle);
            if (!string.IsNullOrEmpty(info_log_vertex))
            {
                errorMessage = info_log_vertex;
                return false;
            }

            return true;
        }

        private bool CompileFragmentShader(string fragmentShaderCode, out int fragmentShaderHandle, out string errorMessage)
        {
            errorMessage = string.Empty;

            fragmentShaderHandle = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShaderHandle, fragmentShaderCode);

            GL.CompileShader(fragmentShaderHandle);
            string info_log_fragment = GL.GetShaderInfoLog(fragmentShaderHandle);
            if (!string.IsNullOrEmpty(info_log_fragment))
            {
                errorMessage = info_log_fragment;
                return false;
            }

            return true;
        }

        private int CreateLinkProgram(int vertexShaderHandle, int fragmentShaderHandle)
        {
            int handle = GL.CreateProgram();

            GL.AttachShader(handle, vertexShaderHandle);
            GL.AttachShader(handle, fragmentShaderHandle);
            
            GL.LinkProgram(handle);

            GL.DetachShader(Handle, vertexShaderHandle);
            GL.DetachShader(Handle, fragmentShaderHandle);

            return handle;
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        ~ShaderProgram()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (isDisposed)
                return;

            isDisposed = true;

            GL.DeleteShader(vertexShaderHandle);
            GL.DeleteShader(fragmentShaderHandle);

            GL.UseProgram(0);
            GL.DeleteProgram(Handle);

            GC.SuppressFinalize(this);
        }
    }
}