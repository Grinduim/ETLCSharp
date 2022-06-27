using System;

using Microsoft.EntityFrameworkCore;

namespace main.DataSource;

public class diagnosticos
{
    public int id { get; set; }
    public Paciente paciente { get; set; }
    public doencas doencas { get; set; }
    public DateTime data_diagnostico { get; set; }

}