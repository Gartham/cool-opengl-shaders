package com.gartham.opengl.shaders;

import java.io.InputStream;
import java.nio.ByteBuffer;
import java.nio.FloatBuffer;
import java.nio.IntBuffer;

import org.alixia.javalibrary.JavaTools;

import com.jogamp.opengl.GL;
import com.jogamp.opengl.GL4;
import com.jogamp.opengl.GLAutoDrawable;
import com.jogamp.opengl.GLEventListener;
import com.jogamp.opengl.glu.GLU;

public final class ShaderappEventListener implements GLEventListener {

	private static GLU glu;

	public static void printError(GL gl) {
		int x = gl.glGetError();
		while (x != GL.GL_NO_ERROR)
			System.out.println("Error: " + (glu == null ? glu = GLU.createGLU() : glu).gluErrorString(x));
	}

	private final InputStream input;
	private int prog;
	private final long startTime = System.currentTimeMillis();

	public ShaderappEventListener(String shaderpath) {
		input = ShaderappEventListener.class.getResourceAsStream(shaderpath);
	}

	@Override
	public void reshape(GLAutoDrawable drawable, int x, int y, int width, int height) {
		if (width < height)
			drawable.getGL().glViewport(0, (height - width) / 2, width, width);
		else
			drawable.getGL().glViewport((width - height) / 2, 0, height, height);
	}

	@Override
	public void init(GLAutoDrawable drawable) {
		GL4 gl = drawable.getGL().getGL4();

		// Create shaders
		int vertshader = gl.glCreateShader(GL4.GL_VERTEX_SHADER),
				fragshader = gl.glCreateShader(GL4.GL_FRAGMENT_SHADER);

		gl.glShaderSource(vertshader, 1,
				new String[] { JavaTools.readText(getClass().getResourceAsStream("vertexShader.vs")) }, null);
		gl.glCompileShader(vertshader);
		gl.glShaderSource(fragshader, 1, new String[] { JavaTools.readText(input) }, null);
		gl.glCompileShader(fragshader);

		prog = gl.glCreateProgram();
		gl.glAttachShader(prog, vertshader);
		gl.glAttachShader(prog, fragshader);
		gl.glLinkProgram(prog);

		gl.glUseProgram(prog);
		
		ByteBuffer bb = ByteBuffer.allocate(100000);
		gl.glGetShaderInfoLog(vertshader, 100000, null, bb);
		for (int i; (i = bb.get()) != 0;)
			System.out.print((char) i);

		bb = ByteBuffer.allocate(100000);
		gl.glGetProgramInfoLog(prog, 100000, null, bb);
		for (int i; (i = bb.get()) != 0;)
			System.out.print((char) i);

		int vbo;
		{
			IntBuffer b = IntBuffer.allocate(1);
			gl.glGenBuffers(1, b);
			vbo = b.get(0);
		}
		gl.glBindBuffer(GL.GL_ARRAY_BUFFER, vbo);
		FloatBuffer triangles = FloatBuffer.wrap(new float[] { -1, -1, -1, 1, 1, 1, -1, -1, 1, -1, 1, 1 });
		gl.glBufferData(GL.GL_ARRAY_BUFFER, triangles.capacity() * Float.BYTES, triangles, GL.GL_STATIC_DRAW);

		int vao;
		{
			IntBuffer i = IntBuffer.allocate(1);
			gl.glGenVertexArrays(1, i);
			vao = i.get(0);
		}
		gl.glBindVertexArray(vao);

		gl.glVertexAttribPointer(0, 2, GL.GL_FLOAT, false, 0, 0);
		gl.glEnableVertexAttribArray(0);
	}

	@Override
	public void dispose(GLAutoDrawable drawable) {
		// TODO Auto-generated method stub

	}

	@Override
	public void display(GLAutoDrawable drawable) {
		GL4 gl = drawable.getGL().getGL4();
		gl.glClearColor(0, 0, 0, 1);
		gl.glClear(GL.GL_COLOR_BUFFER_BIT);
		gl.glDrawArrays(GL.GL_TRIANGLES, 0, 6);

		int loc = gl.glGetUniformLocation(prog, "time");
		if (loc != -1)// If the time uniform is contained in the loaded shader...
			gl.glProgramUniform1f(prog, loc, System.currentTimeMillis() - startTime);
	}

}