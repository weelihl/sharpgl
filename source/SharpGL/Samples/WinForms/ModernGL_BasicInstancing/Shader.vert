#version 420 core
layout(location = 0) in vec2 in_Vertex;
layout(location = 1) in vec4 instance_Color;
layout(location = 2) in vec2 instance_Position;

out vec4 pass_Color;

uniform mat4 mvp;

void main(void) {
	gl_Position = mvp * vec4(in_Vertex + instance_Position,0,1);
	pass_Color = instance_Color;
}