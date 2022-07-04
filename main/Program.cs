using System;
using main.Services;
using main.DataLoad;
using Microsoft.EntityFrameworkCore;


namespace main;
class main
{
    public static void Main(string[] args)
    {
        //FirstService.DiagnosticosPorClassEMes();
        // FirstService.SeparaDados();
        // DoencaIdadeRegiaoService.doencaIdadeRegiaoService();
        FirstService.MediaSalarialDoençaIdade();
    }
}