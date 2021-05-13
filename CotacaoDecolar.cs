using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumScraperTest
{
    class CotacaoDecolar : ICotacao
    {
        private string site = "Decolar";
        private string origem;
        private string destino;
        private DateTime dataida;
        private DateTime datavolta;
        private int adulto;
        private int crianca;
        private bool voodireto;

        public string Origem { get => origem; set => origem = value; }
        public string Destino { get => destino; set => destino = value; }
        public DateTime DataIda { get => dataida; set => dataida = value; }
        public DateTime DataVolta { get => datavolta; set => datavolta = value; }
        public int Adulto { get => adulto; set => adulto = value; }
        public int Crianca { get => crianca; set => crianca = value; }
        public bool VooDireto { get => voodireto; set => voodireto = value; }

        public string Pesquisa()
        {
            try
            {
                using (IWebDriver driver = new FirefoxDriver())
                {
                    string cotacao = site + ";";
                    cotacao = cotacao + dataida.ToShortDateString() + ";";
                    cotacao = cotacao + datavolta.ToShortDateString() + ";";
                    cotacao = cotacao + adulto + ";";
                    cotacao = cotacao + crianca + ";";
                    cotacao = cotacao + voodireto.ToString() + ";*;";

                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                    driver.Navigate().GoToUrl(CriarURL());
                    Console.WriteLine("------------------------------------------------------------------------");
                    Console.WriteLine("URL:" + CriarURL());

                    IWebElement ErrosSite = driver.FindElement(By.ClassName("results-container"));
                    string[] stringSeparatorsErro = new string[] { "\r\n" };
                    string[] erros = ErrosSite.Text.Split(stringSeparatorsErro, StringSplitOptions.None);
                    var contaErros = 0;
                    foreach (var erro in erros)
                    {
                        contaErros++;
                        if (contaErros == 12)
                        {
                            Console.WriteLine(contaErros.ToString() + " ERRO: " + erro.ToString());
                            return "Decolar;;;;;;*;;;;;;*;;;;;;;" + CriarURL() + "; " + erro.ToString() + ";";
                        }
                    }

                    wait.Until(webDriver => webDriver.FindElement(By.ClassName("flight-price-label")).Displayed);

                    IReadOnlyList<IWebElement> listaRotas = driver.FindElements(By.ClassName("route-info"));
                    var ContaAeroporto = 0;
                    var AeroportoOrigemIda = "";
                    var AeroportoDestinoIda = "";
                    var AeroportoOrigemVolta = "";
                    var AeroportoDestinoVolta = "";
                    foreach (var rota in listaRotas)
                    {
                        ContaAeroporto++;
                        switch (ContaAeroporto)
                        {
                            case 1:
                                var ContaAeroportoIda = 0;
                                string[] stringSeparatorsIda = new string[] { "\r\n" };
                                string[] linesIda = rota.Text.Split(stringSeparatorsIda, StringSplitOptions.None);
                                foreach (string s in linesIda)
                                {
                                    ContaAeroportoIda++;
                                    switch (ContaAeroportoIda)
                                    {
                                        case 1:
                                            AeroportoOrigemIda = s;
                                            break;
                                        case 3:
                                            AeroportoDestinoIda = s;
                                            break;
                                    }
                                    //Console.WriteLine(ContaAeroportoIda.ToString() + ") texto: " + s);
                                }
                                break;
                            case 2:
                                var ContaAeroportoVolta = 0;
                                string[] stringSeparatorsVolta = new string[] { "\r\n" };
                                string[] linesVolta = rota.Text.Split(stringSeparatorsVolta, StringSplitOptions.None);
                                foreach (string s in linesVolta)
                                {
                                    ContaAeroportoVolta++;
                                    switch (ContaAeroportoVolta)
                                    {
                                        case 1:
                                            AeroportoOrigemVolta = s;
                                            break;
                                        case 3:
                                            AeroportoDestinoVolta = s;
                                            break;
                                    }
                                    //Console.WriteLine(ContaAeroportoVolta.ToString() + ") texto: " + s);
                                }
                                break;
                        }
                    }

                    IReadOnlyList<IWebElement> listaInerarios = driver.FindElements(By.ClassName("itinerary-container"));
                    var ContaIterario = 0;
                    var CompaniaAereaIda = "";
                    var HoraEmbarqueIda = "";
                    var HoraDesembarqueIda = "";
                    var CompaniaAereaVolta = "";
                    var HoraEmbarqueVolta = "";
                    var HoraDesembarqueVolta = "";
                    foreach (var itinerario in listaInerarios)
                    {
                        ContaIterario++;
                        switch (ContaIterario)
                        {
                            case 1:
                                var ContaItinerarioIda = 0;
                                string[] stringSeparatorsItinerarioIda = new string[] { "\r\n" };
                                string[] linesItinerarioIda = itinerario.Text.Split(stringSeparatorsItinerarioIda, StringSplitOptions.None);
                                foreach (string s in linesItinerarioIda)
                                {
                                    ContaItinerarioIda++;
                                    switch (ContaItinerarioIda)
                                    {
                                        case 1:
                                            CompaniaAereaIda = s;
                                            break;
                                        case 2:
                                            HoraEmbarqueIda = s;
                                            break;
                                        case 4:
                                            HoraDesembarqueIda = s;
                                            break;
                                    }
                                    //Console.WriteLine(ContaItinerarioIda.ToString() + ") texto: " + s);
                                }
                                break;
                            case 2:
                                var ContaItinerarioVolta = 0;
                                string[] stringSeparatorsItinerarioVolta = new string[] { "\r\n" };
                                string[] linesItinerarioVolta = itinerario.Text.Split(stringSeparatorsItinerarioVolta, StringSplitOptions.None);
                                foreach (string s in linesItinerarioVolta)
                                {
                                    ContaItinerarioVolta++;
                                    switch (ContaItinerarioVolta)
                                    {
                                        case 1:
                                            CompaniaAereaVolta = s;
                                            break;
                                        case 2:
                                            HoraEmbarqueVolta = s;
                                            break;
                                        case 4:
                                            HoraDesembarqueVolta = s;
                                            break;
                                    }
                                    //Console.WriteLine(ContaItinerarioVolta.ToString() + ") texto: " + s);
                                }
                                break;
                        }
                    }

                    IReadOnlyList<IWebElement> listaPrecos = driver.FindElements(By.ClassName("flight-price-label"));
                    var PrecoTotal = "";
                    var ContaPreco = 0;
                    foreach (var preco in listaPrecos)
                    {
                        ContaPreco++;
                        if (ContaPreco == 5)
                        {
                            PrecoTotal = preco.Text;
                        }
                    }
                    Console.WriteLine("-------------------------- PRIMEIRO TRECHO-------------------------------");
                    Console.WriteLine("Compania aerea : " + CompaniaAereaIda);
                    cotacao = cotacao + CompaniaAereaIda + ";";
                    Console.WriteLine("Aeroporto origem : " + AeroportoOrigemIda);
                    cotacao = cotacao + AeroportoOrigemIda + ";";
                    Console.WriteLine("Hora embarque : " + HoraEmbarqueIda);
                    cotacao = cotacao + HoraEmbarqueIda + ";";
                    Console.WriteLine("Aeroporto destino: " + AeroportoDestinoIda);
                    cotacao = cotacao + AeroportoDestinoIda + ";";
                    Console.WriteLine("Hora desembarque : " + HoraDesembarqueIda);
                    cotacao = cotacao + HoraDesembarqueIda + ";*;";
                    Console.WriteLine("-------------------------- SEGUNDO TRECHO-------------------------------");
                    Console.WriteLine("Compania aerea : " + CompaniaAereaVolta);
                    cotacao = cotacao + CompaniaAereaVolta + ";";
                    Console.WriteLine("Aeroporto origem : " + AeroportoOrigemVolta);
                    cotacao = cotacao + AeroportoOrigemVolta + ";";
                    Console.WriteLine("Hora embarque : " + HoraEmbarqueVolta);
                    cotacao = cotacao + HoraEmbarqueVolta + ";";
                    Console.WriteLine("Aeroporto destino: " + AeroportoDestinoVolta);
                    cotacao = cotacao + AeroportoDestinoVolta + ";";
                    Console.WriteLine("Hora desembarque : " + HoraDesembarqueVolta);
                    cotacao = cotacao + HoraDesembarqueVolta + ";";
                    Console.WriteLine("------------------------------------------------------------------------");
                    Console.WriteLine("Preço Final: " + PrecoTotal);
                    cotacao = cotacao + PrecoTotal + ";";
                    cotacao = cotacao + CriarURL() + ";";
                    Console.WriteLine("------------------------------------------------------------------------");
                    return cotacao;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "Erro ao efetuar cotação na " + site + " - Data Ida: " + dataida.ToShortDateString() + " e Data Volta: " + datavolta.ToShortDateString());
                return site + ";;;;;;*;;;;;;*;;;;;;;; " + "Erro ao efetuar cotação na " + site + " - Data Ida: " + dataida.ToShortDateString() + " e Data Volta: " + datavolta.ToShortDateString() + ";";
            }
        }

        public void GravaArquivo(string caminho)
        {
            using (StreamWriter w = File.AppendText(caminho))
            {
                w.WriteLine(Pesquisa());
            }
        }

        public string CriarURL()
        {
            var UrlNova = string.Format("https://www.decolar.com/shop/flights/results/roundtrip/{0}/{1}/{2}/{3}/{4}/{5}/0/NA/NA/NA/NA/NA?from=SB&di={4}-{5}", origem, destino, dataida.ToString("yyyy-MM-dd"), datavolta.ToString("yyyy-MM-dd"), adulto, crianca);
            return UrlNova;
        }
    }
}
