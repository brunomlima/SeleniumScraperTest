using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SeleniumScraperTest
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Logo
            Console.WriteLine(" _____ ______   _   _  _                                  ");
            Console.WriteLine("|_   _|| ___ \\ | | | |(_)                                 ");
            Console.WriteLine("  | |  | |_/ / | | | | _   __ _   __ _   ___  _ __   ___  ");
            Console.WriteLine("  | |  | ___ \\ | | | || | / _` | / _` | / _ \\| '_ \\ / __| ");
            Console.WriteLine("  | |  | |_/ / \\ \\_/ /| || (_| || (_| ||  __/| | | |\\__ \\ ");
            Console.WriteLine("  \\_/  \\____/   \\___/ |_| \\__,_| \\__, | \\___||_| |_||___/ ");
            Console.WriteLine("                                 __/ |                   ");
            Console.WriteLine("                                |___/                    ");
            #endregion
            #region Inputs
            Console.WriteLine("Informe o e-mail para receber a cotação: ");
            string sEmail = "bruno@tbviagens.com.br";
            string line = Console.ReadLine();
            while (!Regex.IsMatch(line, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
            {
                Console.WriteLine("Email inválido, por favor digite novamente.");
                line = Console.ReadLine();
            }
            sEmail = line;

            int QtdDias = 15;
            Console.WriteLine("Informe a quantidade de dias para adicionar a data de ida e volta para pesquisa: ");
            //QtdDias = Convert.ToInt32(Console.ReadLine());
            line = Console.ReadLine();
            while (!int.TryParse(line, out QtdDias) || QtdDias > 30)
            {
                Console.WriteLine("Numero inválido, por favor digite novamente.");
                line = Console.ReadLine();
            }
            DateTime dtIda = new DateTime(2021, 04, 27);
            Console.WriteLine("Informe a data de Data Ida conforme exemplo: 31/12/2021 ");
            //dtIda = Convert.ToDateTime(Console.ReadLine());
            line = Console.ReadLine();
            DateTime now = DateTime.Now;
            while (!DateTime.TryParseExact(line, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtIda) || (dtIda < now))
            {
                Console.WriteLine("Data inválida, por favor digite novamente.");
                line = Console.ReadLine();
            }

            DateTime dtVolta = new DateTime(2021, 05, 15);
            Console.WriteLine("Informe a data de Data Volta conforme exemplo: 31/12/2021 ");
            //dtVolta = Convert.ToDateTime(Console.ReadLine());
            line = Console.ReadLine();
            while (!DateTime.TryParseExact(line, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dtVolta) || (dtVolta < now))
            {
                Console.WriteLine("Data inválida, por favor digite novamente.");
                line = Console.ReadLine();
            }

            String sOrigem = "SAO";
            Console.WriteLine("Informe o aeroporto de origem conforme exemplo: SAO, GRU ");
            //sOrigem = Console.ReadLine();
            line = Console.ReadLine();
            while (!Regex.IsMatch(line, "^[A-Z]{3}$"))
            {
                Console.WriteLine("Origem inválida, por favor digite novamente.");
                line = Console.ReadLine();
            }
            sOrigem = line;

            String sDestino = "MIA";
            Console.WriteLine("Informe o aeroporto de destino conforme exemplo: RIO, SDU ");
            //sDestino = Console.ReadLine();
            line = Console.ReadLine();
            while (!Regex.IsMatch(line, "^[A-Z]{3}$"))
            {
                Console.WriteLine("Origem inválida, por favor digite novamente.");
                line = Console.ReadLine();
            }
            sDestino = line;

            int QtdAdt = 2;
            Console.WriteLine("Informe a quantidade de adtos exemplo: 1.");
            //QtdAdt = Convert.ToInt32(Console.ReadLine());
            line = Console.ReadLine();
            while (!int.TryParse(line, out QtdAdt))
            {
                Console.WriteLine("Numero inválido, por favor digite novamente.");
                line = Console.ReadLine();
            }

            int QtdChd = 1;
            Console.WriteLine("Informe a quantidade de chds exemplo: 0.");
            //QtdChd = Convert.ToInt32(Console.ReadLine());
            line = Console.ReadLine();
            while (!int.TryParse(line, out QtdChd))
            {
                Console.WriteLine("Numero inválido, por favor digite novamente.");
                line = Console.ReadLine();
            }

            bool Direto = true;
            string vooDireto = "N";
            Console.WriteLine("Informe se deseja voo direto : S ou N ");
            vooDireto = Console.ReadLine();
            Direto = vooDireto.ToUpper() == "S";
            #endregion
            #region Gera arquivo
            string nomedoarquivo = CriaNomeArquivo(sOrigem, sDestino, QtdAdt, QtdChd);
            var directory = Environment.CurrentDirectory;
            string caminho = Path.Combine(directory, nomedoarquivo);

            CriaTituloArquivo(caminho);

            for (int i = 0; i < QtdDias; i++)
            {
                CotacaoCVC cvc = new CotacaoCVC();
                cvc.Origem = sOrigem;
                cvc.Destino = sDestino;
                cvc.DataIda = dtIda;
                cvc.DataVolta = dtVolta;
                cvc.Adulto = QtdAdt;
                cvc.Crianca = QtdAdt;
                cvc.VooDireto = Direto;
                cvc.Pesquisa();
                cvc.GravaArquivo(caminho);

                CotacaoDecolar decolar = new CotacaoDecolar();
                decolar.Origem = sOrigem;
                decolar.Destino = sDestino;
                decolar.DataIda = dtIda;
                decolar.DataVolta = dtVolta;
                decolar.Adulto = QtdAdt;
                decolar.Crianca = QtdAdt;
                decolar.VooDireto = Direto;
                decolar.Pesquisa();
                decolar.GravaArquivo(caminho);

                CotacaoViajanet viajanet = new CotacaoViajanet();
                viajanet.Origem = sOrigem;
                viajanet.Destino = sDestino;
                viajanet.DataIda = dtIda;
                viajanet.DataVolta = dtVolta;
                viajanet.Adulto = QtdAdt;
                viajanet.Crianca = QtdAdt;
                viajanet.VooDireto = Direto;
                viajanet.Pesquisa();
                viajanet.GravaArquivo(caminho);

                dtIda = dtIda.AddDays(1);
                dtVolta = dtVolta.AddDays(1);

            }
            #endregion
            #region Envio de email
            Email eml = new Email();
            Console.WriteLine(eml.EnviarEmailComAnexo(sEmail, "TBViagens","<h1>Envio de cotação</h1>","Segue cotação conforme solicitado.",caminho ));
            #endregion
        }
        private static string CriaNomeArquivo(string origem, string destino,  int adulto, int crianca)
        {
            string DataGeracao = DateTime.Now.ToString("yyyyMMdd_hhmmtt");
            string NomeArquivo;
            NomeArquivo = "Cotacao_" + DataGeracao + string.Format("_{0}_{1}_A{2}_C{3}.csv", origem, destino, adulto, crianca);
            return NomeArquivo;
        }
        public static void CriaTituloArquivo(string caminho)
        {
            using (StreamWriter w = File.AppendText(caminho))
            {
                w.WriteLine("Site;Data Ida;Data Volta;Qtd Adt;QtdChd;Direto;*;Compania;Origem;Hora Embarque;Destino;Hora Desembarque;*;Compania;Origem;Hora Embarque;Destino;Hora Desembarque;Valor Total;Link;Erro;");
            }
        }
    }
}
