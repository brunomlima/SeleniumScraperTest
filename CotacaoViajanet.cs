using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumScraperTest
{
    class CotacaoViajanet : ICotacao
    {
        private string site = "Viajanet";
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
                    Console.WriteLine("-------------------------- PRIMEIRO TRECHO-------------------------------");
                    wait.Until(webDriver => webDriver.FindElement(By.XPath("//*[@id='detail-price']/div/span[2]")).Displayed);

                    var companiaAerea = driver.FindElement(By.XPath("//*[@id='option0000']/label/div[2]/div[1]/div/div/span"));
                    cotacao = cotacao + companiaAerea.Text + ";";
                    Console.WriteLine("Compania aerea : " + companiaAerea.Text);

                    var aeroportoOrigem = driver.FindElement(By.XPath("//*[@id='segment-flight00']/div[1]/div[2]/div[2]/div[1]/span[1]"));
                    cotacao = cotacao + aeroportoOrigem.Text + ";";
                    Console.WriteLine("Aeroporto origem : " + aeroportoOrigem.Text);

                    var horaembarque = driver.FindElement(By.XPath("//*[@id='option0000']/label/div[2]/div[1]"));
                    string[] stringSeparatorsHoraEmbarque = new string[] { "\r\n" };
                    string[] HoraEmbarques = horaembarque.Text.Split(stringSeparatorsHoraEmbarque, StringSplitOptions.None);
                    var contaHoraEmbarque = 0;
                    foreach (var horaEmb in HoraEmbarques)
                    {
                        contaHoraEmbarque++;
                        if (contaHoraEmbarque == 1)
                        {
                            cotacao = cotacao + horaEmb.ToString() + ";";
                            Console.WriteLine("Hora embarque : " + horaEmb.ToString());
                        }
                    }
                    var aeroportoDestino = driver.FindElement(By.XPath("//*[@id='segment-flight00']/div[1]/div[2]/div[2]/div[2]/span[1]"));
                    cotacao = cotacao + aeroportoDestino.Text + ";";
                    Console.WriteLine("Aeroporto destino: " + aeroportoDestino.Text);

                    var horadesembarque = driver.FindElement(By.XPath("//*[@id='option0000']/label/div[2]/div[3]/div[1]/strong"));
                    cotacao = cotacao + horadesembarque.Text + ";*;";
                    Console.WriteLine("Hora desembarque : " + horadesembarque.Text);

                    Console.WriteLine("-------------------------- SEGUNDO TRECHO-------------------------------");
                    companiaAerea = driver.FindElement(By.XPath("//*[@id='option0100']/label/div[2]/div[1]/div/div/span"));
                    cotacao = cotacao + companiaAerea.Text + ";";
                    Console.WriteLine("Compania aerea : " + companiaAerea.Text);

                    aeroportoOrigem = driver.FindElement(By.XPath("//*[@id='segment-flight01']/div[1]/div[2]/div[2]/div[1]/span[1]"));
                    cotacao = cotacao + aeroportoOrigem.Text + ";";
                    Console.WriteLine("Aeroporto origem : " + aeroportoOrigem.Text);

                    horaembarque = driver.FindElement(By.XPath("//*[@id='option0100']/label/div[2]/div[1]"));
                    string[] stringSeparatorsHoraEmbarque1 = new string[] { "\r\n" };
                    string[] HoraEmbarques1 = horaembarque.Text.Split(stringSeparatorsHoraEmbarque1, StringSplitOptions.None);
                    contaHoraEmbarque = 0;
                    foreach (var horaEmb in HoraEmbarques1)
                    {
                        contaHoraEmbarque++;
                        if (contaHoraEmbarque == 1)
                        {
                            cotacao = cotacao + horaEmb.ToString() + ";";
                            Console.WriteLine("Hora embarque : " + horaEmb.ToString());
                        }
                    }

                    aeroportoDestino = driver.FindElement(By.XPath("//*[@id='segment-flight01']/div[1]/div[2]/div[2]/div[2]/span[1]"));
                    cotacao = cotacao + aeroportoDestino.Text + ";";
                    Console.WriteLine("Aeroporto destino: " + aeroportoDestino.Text);

                    horadesembarque = driver.FindElement(By.XPath("//*[@id='option0100']/label/div[2]/div[3]/div[1]/strong"));
                    cotacao = cotacao + horadesembarque.Text + ";";
                    Console.WriteLine("Hora desembarque : " + horadesembarque.Text);

                    Console.WriteLine("------------------------------------------------------------------------");
                    var valor = driver.FindElement(By.XPath("//*[@id='detail-price']/div/span[2]"));
                    cotacao = cotacao + valor.Text + ";";
                    Console.WriteLine("Preço final: " + valor.Text);
                    Console.WriteLine("------------------------------------------------------------------------");
                    cotacao = cotacao + CriarURL() + ";";
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
            var UrlNova = string.Format("https://www.viajanet.com.br/busca/passagens/voos#/{0}/{1}/RT/{2}/{3}/-/-/-/{4}/{5}/0/-/-/-/-", origem, destino, dataida.ToString("dd-MM-yyyy"), datavolta.ToString("dd-MM-yyyy"), adulto, crianca);
            return UrlNova;
        }
    }
}
