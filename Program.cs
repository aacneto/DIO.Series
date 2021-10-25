using System;


namespace DIO.Series
{
    class Program
    {
        static SerieRepositorio repositorio = new SerieRepositorio();
        static void Main(string[] args)
        {
            
            //Pega opçãp do usuario
           string opcaoUsuario = ObterOpcaoUsuario();
           //Processa opção do usuario
           while(opcaoUsuario.ToUpper()!="X"){
               switch(opcaoUsuario){
                   case "1":
                   //Lista simplificada das Series Cadastradas
                   ListaSeries();
                   break;
                   case "2":
                   //Inclui nova Serie
                   InserirSerie();
                   break;
                   case "3":
                   //Permite alteração dos dados da Serie
                   AtualizarSerie();
                   break;
                   case "4":
                   //Exclui Serie 
                   ExcluirSerie();
                   break;
                   case "5":
                   //Mostra todos os dados da Serie informada
                   VisualizarSerie();
                   break;
                   case "6":
                   //Re-Ativa serie
                   AtivaSerie();
                   break;
                   case "C":
                   //Limpa tela
                   Console.Clear();
                   break;
                   case "L":
                   //Carrega Series de arquivo externo
                   CarregarSeries();
                   break;
                   case "S":
                   //Salva Series de arquivo externo
                   SalvarSeries();
                   break;
                    case "P":
                   //Compacta series na memoria para arquivo
                   Console.WriteLine("Salvar");
                   PackSeries();
                   break;

                   default:
                   //Processa opção invalida
                   opçãoInvalida();
                   break;

               }
               opcaoUsuario=ObterOpcaoUsuario();
           }
        }

        private static void opçãoInvalida()
        {
            Console.WriteLine("Opção Invalida");
            Console.WriteLine("Tecle Enter para continuiar") ;
            Console.ReadLine();
        }

        private static void PackSeries()
        {
            //Compacta series na memoria para arquivo fisico
            //Series com excuido true são fisicamente deletadas
            //Ids são renumerados
             Console.WriteLine("Essao operação sobreescreve dados anteriores" + Environment.NewLine + 
                                "Series excluidas serão definitivamente eliminadas"+ Environment.NewLine +
                                "Numeração de Ids das series sera alterado" + Environment.NewLine);
            if (Resposta_SN() == "N") return;
            repositorio.Pack();
        }

        private static void AtivaSerie()
        {
            //Exclui (desativa Serie) serie permanece no repositorio
            //só será fisicamente excluida em caso de Pack
           
            //Le serie indicada
            Serie serie=leSerie();
             if(serie==null)return;
            //Verifica se serie esta ativa
            if(!serie.retornaExcluido()){
                Console.WriteLine("Serie esta ativa."+
                         Environment.NewLine+"Caso deseje deletar use a opção Excluir");
            } else {
                //Confirma Exclusão
                Console.WriteLine($"Essa ação reativara a serie {serie.retornaTitulo()}" +
                                Environment.NewLine);
                if (Resposta_SN() == "N") return;
                repositorio.Ativa(serie.retornaId());
            }
        }

        private static void VisualizarSerie()
        {
            var serie=leSerie();
             if(serie==null)return;
            Console.WriteLine(serie);
        }

        private static void AtualizarSerie(){
            bool mostraSalvar=false;
            //lw id a ser atualizado           
            Serie serie=leSerie();
            if(serie==null) return;
            //preenche variaveis temporsarias com valor lido
            int indiceSerie=serie.retornaId();
            int entradaGenero=(int)serie.retornaGenero();
            int entradaAno=serie.retornaAno();
            string entradaTitulo=serie.retornaTitulo();
            string entradaDescricao=serie.retornaDescricao();
            //Le opção do usuario
            string opcaoMenu=opcaoMenuAlteracao(serie, mostraSalvar);
            //Rocessa opção do usuario
            while(opcaoMenu!="X"){
               switch(opcaoMenu){
                   case "1":
                   //Altera Genero
                   foreach(int i in Enum.GetValues(typeof(Genero))){
                     Console.WriteLine("{0}-{1}",i,Enum.GetName(typeof(Genero),i));
                    }
                    Console.WriteLine("Digite o genero entre as opções acima:");
                    entradaGenero=int.Parse(Console.ReadLine());
                   break;
                   case "2":
                   //Altera Titulo
                   Console.WriteLine("Digite o titulo da serie:");
                    entradaTitulo=Console.ReadLine();
                    mostraSalvar=true;
                   break;
                   case "3":
                   //Altera Ano
                   Console.WriteLine("Digite o ano de inicio da serie:");
                    entradaAno=int.Parse(Console.ReadLine());
                     mostraSalvar=true;
                   break;
                   case "4":
                   //Altera Descrição
                    Console.WriteLine("Digite a descrição da serie:");
                    entradaDescricao=Console.ReadLine();
                     mostraSalvar=true;
                   break;
                   case "S":
                    Serie atualizaSerie=new Serie(id: indiceSerie,
                            genero: (Genero)entradaGenero,
                            titulo: entradaTitulo,
                            ano: entradaAno,
                            descricao: entradaDescricao);
                    repositorio.Atualiza(indiceSerie,atualizaSerie);
                    serie=repositorio.RetornaPorId(indiceSerie);
                    mostraSalvar=false;
                   break;
                   default:
                   //Processa opção invalida
                   opçãoInvalida();
                   break;

               }
               opcaoMenu=opcaoMenuAlteracao(serie,mostraSalvar);
              
           }
        }

        private static string opcaoMenuAlteracao(Serie serie, bool mostraSalvar)
        {
            string opcao;
            //Mostra Menu de alteração
            Console.Clear();
            Console.WriteLine($"1- Genero: {serie.retornaGenero()}");
            Console.WriteLine($"2- Titulo: {serie.retornaTitulo()}");
            Console.WriteLine($"3- Ano: {serie.retornaAno()}");
            Console.WriteLine($"4- Descrição: {serie.retornaDescricao()}");
            Console.WriteLine("Entre com o numero do item a ser alterado,");
            if(mostraSalvar)Console.WriteLine("S para Salvar");
            Console.WriteLine("X para desistir");
            opcao=Console.ReadLine();
            return opcao.ToUpper();
        }

        private static void ExcluirSerie()
        {
            //Exclui (desativa Serie) serie permanece no repositorio
            //só será fisicamente excluida em caso de Pack
            Serie serie=leSerie();
            if(serie==null)return;
            //Verifica se ja foi excluida anteriormente
            if(serie.retornaExcluido()){
                Console.WriteLine("Serie já foi excluida anteriormente."+
                         Environment.NewLine+"Caso deseje reativar use a opção reativar");
            } else {
                //Confirma Exclusão
                Console.WriteLine($"Essa ação excluira a serie {serie.retornaTitulo()}" +
                                Environment.NewLine);
                if (Resposta_SN() == "N") return;
                repositorio.Exclui(serie.retornaId());
            }
        }

        private static void SalvarSeries()
        {
            Console.WriteLine("Essao operação sobreescreve dados anteriores" +
                                Environment.NewLine );
            if (Resposta_SN() == "N") return;
            repositorio.Save();
        }

        private static void CarregarSeries()
        {
            Console.WriteLine("Você perdara alerações realizadas nessa secção" +
                                Environment.NewLine );
            if (Resposta_SN() == "N") return;
            repositorio.Load();
        }

        private static string Resposta_SN(){
            string resposta = "";
            while (resposta != "S" && resposta != "N"){   
                Console.WriteLine("Deseja continuar (S ou N)?");
                resposta = Console.ReadLine().ToUpper();
            }
            return resposta;
        }

        private static void InserirSerie()
        {
           Console.WriteLine("Inserir nova serie");
           foreach(int i in Enum.GetValues(typeof(Genero))){
               Console.WriteLine("{0}-{1}",i,Enum.GetName(typeof(Genero),i));
           }
           Console.WriteLine("Digite o genero entre as opções acima:");
           int entradaGenero=int.Parse(Console.ReadLine());

           Console.WriteLine("Digite o titulo da serie:");
           string entradaTitulo=Console.ReadLine();

           Console.WriteLine("Digite o ano de inicio da serie:");
           int entradaAno=int.Parse(Console.ReadLine());

           Console.WriteLine("Digite a descrição da serie:");
           string entradaDescricao=Console.ReadLine();

           Serie novaSerie=new Serie(id: repositorio.ProximoId(),
                                     genero: (Genero)entradaGenero,
                                     titulo: entradaTitulo,
                                     ano: entradaAno,
                                     descricao: entradaDescricao);
            repositorio.Insere(novaSerie);
        }

        private static void ListaSeries()
        {
            Console.WriteLine("Listar Series");
            var lista = repositorio.Lista();
            if (lista.Count == 0)
            {
                Console.WriteLine("Nenhuma serie cadastrada");
                return;
            }
            foreach (var serie in lista)
            {
                var excluido = serie.retornaExcluido();
                Console.WriteLine("#ID {0}: -{1} - {2}", serie.retornaId(), serie.retornaTitulo(), (excluido ? "Excluido" : "Ativo"));
            }
            Continuar();
        }

        private static void Continuar()
        {
            Console.WriteLine();
            Console.WriteLine("Tecle Enter para continuar");
            Console.ReadLine();
        }
        private static Serie leSerie(){
            Serie serie=null;
            int id=-1;
            do{
                Console.Write("Digite o id da serie: ");
               try {
                    string s=Console.ReadLine();
                    
                    if(!string.IsNullOrEmpty(s)){
                        id=int.Parse(s);
                    } else {
                        continue;
                    }
               }
               catch (FormatException e)
                {
                    continue;
                }
                
                serie=repositorio.RetornaPorId(id);
                if(serie==null){
                    Console.WriteLine("Id invalido.");
                    Console.WriteLine("Tecle Enter para continuar.");
                    Console.WriteLine("X para sair");
                    string resp=Console.ReadLine().ToUpper();
                    if(resp=="X") break;
                }
            } while(serie==null);
            
            return serie;
        }

        private static string ObterOpcaoUsuario(){
            //Pega opção do usuario
            Console.WriteLine();
            Console.WriteLine("DIO Series a seu dispor!!!");
            Console.WriteLine("Informe opção desejada:");

            Console.WriteLine("1- Listar series");
            Console.WriteLine("2- Inserir nova serie");
            Console.WriteLine("3- Atualizar serie");
            Console.WriteLine("4- Excluir serie");
            Console.WriteLine("5- Visualizar serie");
            Console.WriteLine("6- Reativar serie");
            Console.WriteLine("C- Limpar Tela");
            Console.WriteLine("L- Carregar arquivo");
            Console.WriteLine("S- Salvar arquivo");
            Console.WriteLine("P- Compactar arquivo");
            Console.WriteLine("X- Sair");

            string opcaoUsuario=Console.ReadLine().ToUpper();
            Console.WriteLine();
            return opcaoUsuario;

        }
    }
}
