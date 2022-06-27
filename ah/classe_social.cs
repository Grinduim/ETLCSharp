using System;

using Microsoft.EntityFrameworkCore;

namespace main.DataSource;

public class Classe_social
{
    public int id { get; set; }
    public float salario_piso { get; set; }
    public float salario_teto { get; set; }
}