package com.gartham.opengl.shaders;

import java.io.InputStream;

import com.jogamp.opengl.GL4;
import com.jogamp.opengl.GLAutoDrawable;
import com.jogamp.opengl.GLEventListener;

public final class ShaderappEventListener implements GLEventListener {

	private final InputStream input;

	public ShaderappEventListener(String shaderpath) {
		input = ShaderappEventListener.class.getResourceAsStream(shaderpath);
	}

	@Override
	public void reshape(GLAutoDrawable drawable, int x, int y, int width, int height) {
		drawable.getGL().glViewport(0, 0, width, height);
	}

	@Override
	public void init(GLAutoDrawable drawable) {
		GL4 gl = drawable.getGL().getGL4();

	}

	@Override
	public void dispose(GLAutoDrawable drawable) {
		// TODO Auto-generated method stub

	}

	@Override
	public void display(GLAutoDrawable drawable) {
		// TODO Auto-generated method stub

		// Draw code goes here.
	}
}