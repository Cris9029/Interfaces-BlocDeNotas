#include <stdio.h>
#include <math.h>
#include <GL/freeglut.h>
#include <GLFW/glfw3.h>
#include <vector>
#include "Vertice.h"
#include "Objeto.h"
#include "Cara.h"

#pragma warning(disable:4996)

GLFWwindow* window;
Objeto modelo;

void display(void)
{
	/*  clear all pixels  */
	glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);



	glBegin(GL_TRIANGLES);	//Dibuja triangulos
	
	//CUBO
	/*glColor3f(1, 0, 0);

	glVertex3f(0, 0, 0);	//Primer vertice
	glVertex3f(1, 1, 0);	//Segundo
	glVertex3f(0, 1, 0);	//Tercer

	glColor3f(0, 1, 0);
	glVertex3f(0, 0, 0);
	glVertex3f(1,0,0);
	glVertex3f(1,1,0);

	glColor3f(0, 0, 1);
	glVertex3f(0, 0, 1);
	glVertex3f(1, 1, 1);
	glVertex3f(0, 1, 1);

	glColor3f(20, 10, 0);
	glVertex3f(0, 0, 1);
	glVertex3f(1, 0, 1);
	glVertex3f(1, 1, 1);

	glColor3f(0, 255, 128);
	glVertex3f(1, 0, 0);
	glVertex3f(1, 1, 1);
	glVertex3f(1, 0, 1);

	glColor3f(255, 0, 127);
	glVertex3f(1, 0, 0);
	glVertex3f(1, 1, 0);
	glVertex3f(1, 1, 1);

	glColor3f(204, 153, 255);
	glVertex3f(0, 0, 0);
	glVertex3f(0, 1, 1);
	glVertex3f(0, 0, 1);

	glColor3f(1, 1, 1);
	glVertex3f(0, 0, 0);
	glVertex3f(0, 1, 0);
	glVertex3f(0, 1, 1);

	glColor3f(160, 160, 160);
	glVertex3f(0, 1, 0);
	glVertex3f(0, 1, 1);
	glVertex3f(1, 1, 0);

	glColor3f(1, 0, 0);
	glVertex3f(1, 1, 0);
	glVertex3f(1, 1, 1);
	glVertex3f(0, 1, 1);

	glColor3f(0, 102, 102);
	glVertex3f(0, 0, 0);
	glVertex3f(0, 0, 1);
	glVertex3f(1, 0, 0);

	glColor3f(0, 0, 153);
	glVertex3f(1, 0, 0);
	glVertex3f(1, 0, 1);
	glVertex3f(0, 0, 1);*/

	//Para todos los modelos
	//Para todas las caras del modelo
	for (int i = 0; i < modelo.caras.size(); i++) {
		glColor3f(1,0,0);	//De 0 a 1
		glVertex3f(modelo.vertices[modelo.caras[i].v1].x, modelo.vertices[modelo.caras[i].v1].y, modelo.vertices[modelo.caras[i].v1].z);
		glVertex3f(modelo.vertices[modelo.caras[i].v2].x, modelo.vertices[modelo.caras[i].v2].y, modelo.vertices[modelo.caras[i].v2].z);
		glVertex3f(modelo.vertices[modelo.caras[i].v3].x, modelo.vertices[modelo.caras[i].v3].y, modelo.vertices[modelo.caras[i].v3].z);
	}
	
	glEnd();	//Termina


	glfwSwapBuffers(window);
	glFlush();
}

void init(void)
{
	/*  select clearing (background) color       */
	glClearColor(0.0f, 0.0f, 0.0f, 0.0f);

	/*  initialize viewing values  */
	glMatrixMode(GL_PROJECTION); //Matriz de projección
	glLoadIdentity();

	//glOrtho(0.0, 1.0, 0.0, 1.0, -1.0, 1.0);
	//gluOrtho2D(0.0, 1.0, 0.0, 1.0);

	gluPerspective(45.0, 800.0 / 600.0, 0.1, 100.0);//45:Equivale al ángulo sobre Y.
	//800/600 si se cambia se debe de cambiar también el tamaño de la ventana.

	glMatrixMode(GL_MODELVIEW);	//Matriz de vista
	glLoadIdentity();

	gluLookAt(2.0f, 2.0f, 3.0f, 0.0f, 0.0f, 0.0f, 0.0f, 1.0f, 0.0f);	//Vista de la cámara (x,y,z)
	//Los primeros 3 f representan donde esta la cámara, los siguientes 3 f representan a donde está apuntando la cámara

	glEnable(GL_DEPTH_TEST);	//MUY IMPORTANTE PARA EL FUNCIONAMIENTO
}

Objeto lectorOBJ(const char* ruta)
{
	FILE* fp = fopen(ruta, "r");	//"C:\Users\paual\OneDrive\Desktop\Cubo.obj"
	if (fp == NULL)
	{
		printf("Error abriendo el archivo");
		return modelo;
	}

	char linea[256];
	float fx, fy, fz;
	int x, y, z;

	while (fgets(linea, sizeof(linea), fp))
	{
		if (linea[0] == 'v')
		{
			sscanf(linea, "v %f %f %f", &fx, &fy, &fz);
			modelo.vertices.push_back(Vertice(fx,fy,fz));
		}
		if (linea[0] == 'f')
		{
			sscanf(linea, "f %d %d %d", &x, &y, &z);
			modelo.caras.push_back(Cara(x-1, y-1, z-1));
		}
	}
	fclose(fp);
	return modelo;
}

int main(int argc, char** argv)
{

	glfwInit();	//Inicializa
	window = glfwCreateWindow(800, 600, "Test", NULL, NULL);	//Crea la ventana con el tamaño dado.
	glfwMakeContextCurrent(window);

	//glfwSetInputMode(window, GLFW_STICKY_KEYS, GL_TRUE);

	init();	//Inicializa OpenGL

	modelo = lectorOBJ("C:\\Users\\paual\\OneDrive\\Desktop\\Cubo.obj");
	//GameLoop/MainLoop
	do
	{
		display();
		glfwPollEvents();

	} while (glfwGetKey(window, GLFW_KEY_S) != GLFW_PRESS && glfwWindowShouldClose(window) == 0);

	glfwTerminate();

	return 0;
}