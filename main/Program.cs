using System;

using main.DataLoad;

using main.DataSource;

namespace main;

class main{
    public static void Main(string[] args){

       using(var context = new SourceContext()){
            var teste = context.pacientes.Where(u=> u.id <10);
            foreach(var item in teste){
                Console.WriteLine(item.nome);
            }
       }
    }
}