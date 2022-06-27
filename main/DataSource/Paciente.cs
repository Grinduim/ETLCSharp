using System;

using Microsoft.EntityFrameworkCore;

namespace main.DataSource;

public class Paciente {
    int id;
    string nome;
    int idade;
    Estados estado;
    Class_social class_Social;
}