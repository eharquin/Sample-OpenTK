using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;

namespace Sample_OpenTK
{
    internal class Game1 : GameWindow
    {
        //int vertexBufferHandle;
        //int shaderProgamHandle;
        //int vertexArrayHandle;
        //int indexBufferHandle;

        VertexBuffer vertexBuffer;
        ShaderProgram shaderProgram;
        VertexArray vertexArray;
        IndexBuffer indexBuffer;

        Texture texture;

        int vertexCount;
        int indexCount;

        public Game1(int width = 1280, int height = 720, string title = "Test")
            : base(
                GameWindowSettings.Default,
                new NativeWindowSettings()
                {
                    Title = title,
                    Size = new Vector2i(width, height),
                    //WindowBorder = WindowBorder.Fixed,
                    StartVisible = false,
                    StartFocused = false,
                    API = ContextAPI.OpenGL,
                    Profile = ContextProfile.Core,
                    APIVersion = new Version(4, 6),
                })
        {
            CenterWindow(new Vector2i(1280, 720));
            GL.Enable(EnableCap.Texture2D);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            GL.Viewport(0, 0, e.Width, e.Height);
            base.OnResize(e);
        }

        protected override void OnLoad()
        {
            IsVisible = true;

            GL.ClearColor(Color4.LightGray);

            texture = new Texture("BeachBallTexture.png");

            //int boxCount = 1000;

            //vertexCount = 0;
            //indexCount = 0;

            //VertexPositionColor[] vertices = new VertexPositionColor[boxCount * 4];
            //int[] indices = new int[boxCount * 6];

            //Random random = new Random();

            //float side = 0.1f;

            //for (int i = 0; i < boxCount; i++)
            //{
            //    int width = random.Next(32, 128);
            //    int height = random.Next(32, 128);

            //    int x = random.Next(0, 1280 - width);
            //    int y = random.Next(0, 720 - height);

            //    float r = (float)random.NextDouble();
            //    float g = (float)random.NextDouble();
            //    float b = (float)random.NextDouble();


            //    vertices[vertexCount + 0] = new VertexPositionColor(new Vector3(x, y, 0f), new Vector4(r, g, b, 1f));
            //    vertices[vertexCount + 1] = new VertexPositionColor(new Vector3(x + width, y, 0f), new Vector4(r, g, b, 1f));
            //    vertices[vertexCount + 2] = new VertexPositionColor(new Vector3(x + width, y + height, 0f), new Vector4(r, g, b, 1f));
            //    vertices[vertexCount + 3] = new VertexPositionColor(new Vector3(x, y + height, 0f), new Vector4(r, g, b, 1f));


            //    indices[indexCount + 0] = vertexCount + 0;
            //    indices[indexCount + 1] = vertexCount + 1;
            //    indices[indexCount + 2] = vertexCount + 2;
            //    indices[indexCount + 3] = vertexCount + 0;
            //    indices[indexCount + 4] = vertexCount + 2;
            //    indices[indexCount + 5] = vertexCount + 3;

            //    indexCount += 6;
            //    vertexCount += 4;
            //}

            Random random = new Random();
            int width = 1280;
            int height = 720;

            //int x = random.Next(0, 1280 - width);
            //int y = random.Next(0, 720 - height);

            int x = 0;
            int y = 0;

            VertexPositionColorTexture[] vertices =
            {
                new VertexPositionColorTexture(new Vector3(x, y, 0f), new Vector4(1f, 0f, 0f, 1f), new Vector2(0f, 0f)),  // bottom left
                new VertexPositionColorTexture(new Vector3(x + width, y, 0f), new Vector4(0f, 1f, 0f, 1f), new Vector2(1f, 0f)),  // bottom right
                new VertexPositionColorTexture(new Vector3(x + width, y + height, 0f), new Vector4(0f, 0f, 1f, 1f), new Vector2(1f, 1f)),  // top right
                new VertexPositionColorTexture(new Vector3(x, y + height, 0f), new Vector4(1f, 1f, 0f, 1f), new Vector2(0f, 1f))  // top left
            };

            VertexInfo vertexInfo = VertexPositionColorTexture.VertexInfo;

            //VertexPositionColor[] vertices =
            //{
            //    new VertexPositionColor(new Vector3(x, y, 0f), new Vector4(1f, 0f, 0f, 1f)),
            //    new VertexPositionColor(new Vector3(x + width, y, 0f), new Vector4(0f, 1f, 0f, 1f)),
            //    new VertexPositionColor(new Vector3(x + width, y + height, 0f), new Vector4(0f, 0f, 1f, 1f)),
            //    new VertexPositionColor(new Vector3(x, y + height, 0f), new Vector4(1f, 1f, 0f, 1f))
            //};

            //VertexInfo vertexInfo = VertexPositionColor.VertexInfo;


            vertexBuffer = new VertexBuffer(vertexInfo);
            vertexBuffer.SetData(vertices, vertices.Length);

            vertexArray = new VertexArray(vertexBuffer);

            int[] indices =
            {
                0, 1, 2,
                0, 2, 3
            };

            indexBuffer = new IndexBuffer();
            indexBuffer.SetData(indices, indices.Length);

            string vertexShaderCode =
                @"
                #version 460 core
                
                uniform vec2 viewport;

                layout (location = 0) in vec3 a_position;
                layout (location = 1) in vec4 a_color;
                layout (location = 2) in vec2 a_texture;

                out vec4 v_color;
                out vec2 v_texture;
                
                void main()
                {
                    float nx = a_position.x / viewport.x * 2.0 - 1.0;
                    float ny = a_position.y / viewport.y * 2.0 - 1.0;

                    gl_Position = vec4(nx, ny, 0.0, 1.0);
                    v_color = a_color;
                    v_texture = a_texture;
                }
                ";

            string fragmentShaderCode =
                @"
                #version 460 core

                out vec4 f_color;

                in vec4 v_color;
                in vec2 v_texture;

                uniform sampler2D texture0;

                void main()
                {
                    f_color = texture(texture0, v_texture);
                }
                ";

            shaderProgram = new ShaderProgram(vertexShaderCode, fragmentShaderCode);
            
            base.OnLoad();
        }

        protected override void OnUnload()
        {
            vertexBuffer?.Dispose();
            shaderProgram?.Dispose();
            vertexArray?.Dispose();
            indexBuffer?.Dispose();

            texture?.Dispose();

            base.OnUnload();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            GL.UseProgram(shaderProgram.Handle);

            //GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texture.Handle);

            GL.BindVertexArray(vertexArray.Handle);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, indexBuffer.Handle);

            GL.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, IntPtr.Zero);

            GL.UseProgram(0);
            GL.BindVertexArray(0);
            //GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

            Context.SwapBuffers();
            base.OnRenderFrame(args);
        }
    }
}
