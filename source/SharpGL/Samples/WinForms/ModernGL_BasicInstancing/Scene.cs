using System;
using GlmNet;
using SharpGL;
using SharpGL.Shaders;
using SharpGL.VertexBuffers;

namespace ModernOpenGLSample
{
    /// <summary>
    /// Represents the Scene for this sample.
    /// </summary>
    /// <remarks>
    /// This code is based on work from the OpenGL 4.x Swiftless tutorials, please see:
    /// http://www.swiftless.com/opengl4tuts.html
    /// </remarks>
    public class Scene
    {
        //  The projection, view and model matrices.
        mat4 mvp;

        //  Constants that specify the attribute indexes.
        public readonly uint[] attributeIndex = {0,1,2};
        

        //  The vertex buffer array which contains the vertex and colour buffers.
        VertexBufferArray vertexBufferArray;
    
        //  The shader program for our vertex and fragment shader.
        private ShaderProgram shaderProgram;

        /// <summary>
        /// Initialises the scene.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="width">The width of the screen.</param>
        /// <param name="height">The height of the screen.</param>
        public void Initialise(OpenGL gl, float width, float height)
        {
            //  Set a blue clear colour.
            gl.ClearColor(0.4f, 0.6f, 0.9f, 0.0f);

            //  Create the shader program.
            var vertexShaderSource = ManifestResourceLoader.LoadTextFile("Shader.vert");
            var fragmentShaderSource = ManifestResourceLoader.LoadTextFile("Shader.frag");
            shaderProgram = new ShaderProgram();
            shaderProgram.Create(gl, vertexShaderSource, fragmentShaderSource, null);
            shaderProgram.BindAttributeLocation(gl, attributeIndex[0], "in_Vertex");
            shaderProgram.BindAttributeLocation(gl, attributeIndex[1], "instance_Color");
            shaderProgram.BindAttributeLocation(gl, attributeIndex[2], "instance_Position");
            shaderProgram.AssertValid(gl);

            //  Create a model-view-perspective (MVP) projection matrix.            
            mvp = glm.ortho(0f, 1f, 0f, 1f, -1f, 1f);

            //  Now create the geometry for the square.
            CreateVerticesForSquare(gl);
        }

        /// <summary>
        /// Draws the scene.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public void Draw(OpenGL gl)
        {
            //  Clear the scene.
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT | OpenGL.GL_STENCIL_BUFFER_BIT);

            //  Bind the shader, set the matrices.
            shaderProgram.Bind(gl);
            shaderProgram.SetUniformMatrix4(gl, "mvp", mvp.to_array());

            //  Bind the out vertex array.
            vertexBufferArray.Bind(gl);

            //  Draw the square.
            gl.DrawArraysInstanced(OpenGL.GL_TRIANGLE_STRIP, 0, 4, 5);            

            //  Unbind our vertex array and shader.
            vertexBufferArray.Unbind(gl);
            shaderProgram.Unbind(gl);
        }
        
        /// <summary>
        /// Creates the geometry for the square, also creating the vertex buffer array.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        private void CreateVerticesForSquare(OpenGL gl)
        {
            float[] vertices = new float[]{
                0.0f,   0.0f,    // Top-left
                0.0f,   0.1f,    // Top-right
                0.1f,   0.0f,      // Bottom-right
                0.1f,   0.1f       // Bottom-left
            };

            var instance_colors = new float[] { 
                1.0f, 0.0f, 0.0f, 1.0f, // Red
                0.0f, 1.0f, 0.0f, 1.0f, // Green
                0.0f, 0.0f, 1.0f, 1.0f, // Blue
                1.0f, 1.0f, 0.0f, 1.0f, // Yellow
                1.0f, 1.0f, 1.0f, 1.0f  // White
            };

            var instance_positions = new float[]{
                 0.1f,   0.1f, 
                 0.1f,   0.8f, 
                 0.8f,   0.8f, 
                 0.8f,   0.1f,
                 0.45f,   0.45f
            };

            //  Create the vertex array object.
            vertexBufferArray = new VertexBufferArray();
            vertexBufferArray.Create(gl);
            vertexBufferArray.Bind(gl);

            //  Create a vertex buffer for the vertex data.
            var vertexDataBuffer = new VertexBuffer();
            vertexDataBuffer.Create(gl);
            vertexDataBuffer.Bind(gl);
            vertexDataBuffer.SetData(gl, 0, vertices, false, 2, OpenGL.GL_STATIC_DRAW);

            //  Now do the same for the colour data.
            var colourDataBuffer = new VertexBuffer();
            colourDataBuffer.Create(gl);
            colourDataBuffer.Bind(gl);
            colourDataBuffer.SetData(gl, 1, instance_colors, false, 4, OpenGL.GL_STATIC_DRAW);

            //  Now do the same for the instance position data.
            var positionDataBuffer = new VertexBuffer();
            positionDataBuffer.Create(gl);
            positionDataBuffer.Bind(gl);
            positionDataBuffer.SetData(gl, 2, instance_positions, false, 2, OpenGL.GL_STATIC_DRAW);

            //  Instancing divisor
            gl.VertexAttribDivisor(0, 0);
            gl.VertexAttribDivisor(1, 1); //update per instance
            gl.VertexAttribDivisor(2, 1); //update per instance


            //  Unbind the vertex array, we've finished specifying data for it.
            vertexBufferArray.Unbind(gl);           
        }

    }
}
