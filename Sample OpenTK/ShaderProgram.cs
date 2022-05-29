using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace Sample_OpenTK
{
    public struct ShaderAttribute
    {
        public ActiveAttribType Type;
        public string Name;
        public int Location;

        public ShaderAttribute(ActiveAttribType type, string name, int location)
        {
            Type = type;
            Name = name;
            Location = location;
        }
    }
    public struct ShaderUniform
    {
        public ActiveUniformType Type;
        public string Name;
        public int Location;

        public ShaderUniform(ActiveUniformType type, string name, int location)
        {
            Type = type;
            Name = name;
            Location = location;
        }
    }

    public class ShaderProgram : IDisposable
    {
        bool isDisposed;

        public readonly int Handle;

        private readonly int vertexShaderHandle;
        private readonly int fragmentShaderHandle;

        public ShaderAttribute[] ShaderAttribute;
        public ShaderUniform[] ShaderUniform;

        public ShaderProgram(string vertexShaderCode, string fragmentShaderCode)
        {
            if (!CompileVertexShader(vertexShaderCode, out vertexShaderHandle, out string vertexShaderErrorMessage))
            {
                throw new ArgumentException(vertexShaderErrorMessage);
            }

            if (!CompileFragmentShader(fragmentShaderCode, out fragmentShaderHandle, out string fragmentShaderErrorMessage))
            {
                Console.WriteLine(fragmentShaderErrorMessage);
                throw new ArgumentException(fragmentShaderErrorMessage);
            }

            Handle = CreateLinkProgram(vertexShaderHandle, fragmentShaderHandle);

            ShaderAttribute = getAttributes();
            ShaderUniform = getUniforms();
            foreach(var uniform in ShaderUniform)
                Console.WriteLine(uniform.Name);

            GL.UseProgram(Handle);


            int location1 = GL.GetUniformLocation(Handle, "texture1");

            int location2 = GL.GetUniformLocation(Handle, "texture2");

            Console.Write(location1 + " " + location2);

            GL.UseProgram(0);

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

        private ShaderAttribute[] getAttributes()
        {
            GL.GetProgram(Handle, GetProgramParameterName.ActiveAttributes, out int count);

            ShaderAttribute[] attributes = new ShaderAttribute[count];

            for (int i = 0; i < count; i++)
            {
                GL.GetActiveAttrib(Handle, i, 256, out int length, out int size, out ActiveAttribType type, out string name);
                int location = GL.GetAttribLocation(Handle, name);
                attributes[i] = new ShaderAttribute(type, name, location);
            }

            return attributes;
        }

        private ShaderUniform[] getUniforms()
        {
            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int count);
            Console.WriteLine(count);

            ShaderUniform[] uniforms = new ShaderUniform[count];
            
            for (int i = 0; i < count; i++)
            {
                GL.GetActiveUniform(Handle, i, 256, out int length, out int size, out ActiveUniformType type, out string name);
                int location = GL.GetUniformLocation(Handle, name);
                uniforms[i] = new ShaderUniform(type, name, location);
            }

            return uniforms;
        }

        public void SetUniform(string name, int v)
        {
            if (!getShaderUniform(name, out ShaderUniform uniform))
            {
                throw new ArgumentException("Uniform does not exist");
            }

            if (uniform.Type != ActiveUniformType.Sampler2D)
            {
                throw new ArgumentException("Uniform and value doesn't match");
            }

            GL.UseProgram(Handle);
            GL.Uniform1(uniform.Location, v);
            GL.UseProgram(0);
        }

        public void SetUniform(string name, float v)
        {
            if(!getShaderUniform(name, out ShaderUniform uniform))
            {
                throw new ArgumentException("Uniform does not exist");
            }

            if(uniform.Type != ActiveUniformType.Float)
            {
                throw new ArgumentException("Uniform and value doesn't match");
            }

            GL.UseProgram(Handle);
            GL.Uniform1(uniform.Location, v);
            GL.UseProgram(0);
        }

        public void SetUniform(string name, float v1, float v2)
        {
            if (!getShaderUniform(name, out ShaderUniform uniform))
            {
                throw new ArgumentException("Uniform does not exist");
            }

            if (uniform.Type != ActiveUniformType.FloatVec2)
            {
                throw new ArgumentException("Uniform and value doesn't match");
            }

            GL.UseProgram(Handle);
            GL.Uniform2(uniform.Location, v1, v2);
            GL.UseProgram(0);
        }

        public void SetUniform(string name, float[] v)
        {
            if (!getShaderUniform(name, out ShaderUniform uniform))
            {
                throw new ArgumentException("Uniform does not exist");
            }

            if (uniform.Type != ActiveUniformType.FloatVec2)
            {
                throw new ArgumentException("Uniform and value doesn't match");
            }

            GL.UseProgram(Handle);
            GL.Uniform3(uniform.Location, v.Length, v);
            GL.UseProgram(0);
        }

        public void SetUniform(string name, Matrix4 v)
        {
            if (!getShaderUniform(name, out ShaderUniform uniform))
            {
                throw new ArgumentException("Uniform does not exist");
            }

            if (uniform.Type != ActiveUniformType.FloatMat4)
            {
                throw new ArgumentException("Uniform and value doesn't match");
            }

            GL.UseProgram(Handle);
            GL.UniformMatrix4(uniform.Location, false, ref v);
            GL.UseProgram(0);
        }


        private bool getShaderUniform(string name, out ShaderUniform uniform)
        {
            uniform = new ShaderUniform();

            foreach(var uniform0 in ShaderUniform)
            {
                if(uniform0.Name == name)
                {
                    uniform = uniform0;
                    return true;
                }
            }
            return false;
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