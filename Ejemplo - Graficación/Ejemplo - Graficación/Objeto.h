#pragma once
#include <vector>		//<> Busca en los directorios del entorno(IDE)
#include "Vertice.h"	//"" Busca donde esta el proyecto
#include "Cara.h"

using std::vector;	//Esto hace que no se necesite escribir std::vector cada vez(EJ: std::vector<Vertice> vertices)

class Objeto
{
public:
	vector<Vertice> vertices;
	vector<Cara> caras;
	/*numbers.push_back(5);
	numbers.push_back(15);
	numbers.push_back(25);

	for (int i = 0; i < numbers.size(); i++)
	{
		printf("%d\n", numbers[i]);
	}*/
};

