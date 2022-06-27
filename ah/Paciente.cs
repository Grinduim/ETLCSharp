using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace main.DataSource;

public class Paciente {
   public int id;
   public string nome;
   public int idade;
  public  int id_estado;

   public int id_classe_Social;
}