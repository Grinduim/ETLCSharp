using System;

using Microsoft.EntityFrameworkCore;

namespace main.DataSource;

public class Estados
{

    public int id { get; set; }
    public string nome { get; set; }

    public regioes regiao { get; set; }

}