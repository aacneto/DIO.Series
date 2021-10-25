using System;
using System.Collections.Generic;
using System.IO;
namespace DIO.Series
{
    public class SerieRepositorio : IRepositorio<Serie>
    {
        private List<Serie> listaSerie = new List<Serie>();
        public void Atualiza(int id, Serie objeto)
        {
            listaSerie[id]= objeto;
        }

        public void Exclui(int id)
        {
            listaSerie[id].Exclui();
        }

        public void Ativa(int id)
        {
            listaSerie[id].Ativa();
        }

        public void Insere(Serie objeto)
        {
            listaSerie.Add(objeto);
        }

        public List<Serie> Lista()
        {
            return listaSerie;
        }

        public int ProximoId()
        {
            return listaSerie.Count;
        }

        public Serie RetornaPorId(int id)
        {
           if(id<0 || id >=listaSerie.Count)return null; 
           return listaSerie[id];
        }

        public void Load(){
            FileStream fs = new FileStream("Series.txt", FileMode.Open, FileAccess.Read);  
            StreamReader sr = new StreamReader(fs);  
            //Limpa lista atual
            listaSerie.Clear();
            //Console.WriteLine("Program to show content of test file");  
            sr.BaseStream.Seek(0, SeekOrigin.Begin);  
            string str = sr.ReadLine();  
            while (str != null){  
                //Console.WriteLine(str);
                string[] words = str.Split(',');
                // foreach(string s in words){
                //     Console.Write($"{s} ");
                // }
                int id=this.ProximoId();
                Serie novaSerie=new Serie(id: id,
                                    genero: (Genero)int.Parse(words[1]),
                                    titulo: words[2],
                                    ano: int.Parse(words[3]),
                                    descricao: words[4],
                                    excluido: bool.Parse(words[5]));
                this.Insere(novaSerie);
                //Console.WriteLine();
                str = sr.ReadLine();  
            }  
            //Console.ReadLine();  
            sr.Close();  
            fs.Close();  
        }   
        public void Save(){
           
            FileStream fs = new FileStream("Series.txt", FileMode.Create, FileAccess.Write);  
            StreamWriter sw = new StreamWriter(fs);  
            
            foreach(Serie serie in listaSerie) {
                    sw.Write("{0},",serie.Id);
                    sw.Write("{0},",((int)serie.retornaGenero()));
                    sw.Write("{0},",serie.retornaTitulo());
                    sw.Write("{0},",serie.retornaAno());
                    sw.Write("{0},",serie.retornaDescricao());
                    sw.WriteLine("{0}",serie.retornaExcluido());
            }
                  
            sw.Flush();  
            sw.Close();  
            fs.Close();  
        }

        public void Pack(){
            Serie serie;
            // Salva, apaga repositorio em memoria, e carrega novamente 
            for (int i = listaSerie.Count - 1; i >= 0; i--){
                serie=this.RetornaPorId(i);
                if(serie.retornaExcluido()) listaSerie.RemoveAt(i);
            }
            Save();
            listaSerie.Clear();
            Load();
            
        }

    }
}