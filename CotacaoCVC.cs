using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace SeleniumScraperTest
{
    class CotacaoCVC : ICotacao
    {
        private string site = "CVC";
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
                    wait.Until(webDriver => webDriver.FindElement(By.XPath("//*[@id='cards-list']/div[1]/div/div[1]/div[1]/div/div[1]/div[1]/h6/span[2]")).Displayed);

                    var companiaAerea = driver.FindElement(By.XPath("//*[@id='cards-list']/div[1]/div/div[1]/div[1]/div/div[1]/div[1]/h6/span[2]"));
                    cotacao = cotacao + companiaAerea.Text + ";";
                    Console.WriteLine("Compania aerea : " + companiaAerea.Text);

                    var aeroportoOrigem = driver.FindElement(By.XPath("//*[@id='cards-list']/div[1]/div/div[1]/div[1]/div/div[2]/div/div[1]/h5/span[1]/span"));
                    cotacao = cotacao + aeroportoOrigem.Text + ";";
                    Console.WriteLine("Aeroporto origem : " + aeroportoOrigem.Text);

                    var horaembarque = driver.FindElement(By.XPath("//*[@id='cards-list']/div[1]/div/div[1]/div[1]/div/div[2]/div/div[1]/h5/span[2]"));
                    cotacao = cotacao + horaembarque.Text + ";";
                    Console.WriteLine("Hora embarque : " + horaembarque.Text);

                    var aeroportoDestino = driver.FindElement(By.XPath("//*[@id='cards-list']/div[1]/div/div[1]/div[1]/div/div[2]/div/div[2]/h5/span[1]/span"));
                    cotacao = cotacao + aeroportoDestino.Text + ";";
                    Console.WriteLine("Aeroporto destino: " + aeroportoDestino.Text);

                    var horadesembarque = driver.FindElement(By.XPath("//*[@id='cards-list']/div[1]/div/div[1]/div[1]/div/div[2]/div/div[2]/h5/span[2]"));
                    cotacao = cotacao + horadesembarque.Text + ";*;";
                    Console.WriteLine("Hora desembarque : " + horadesembarque.Text);

                    Console.WriteLine("-------------------------- SEGUNDO TRECHO-------------------------------");
                    companiaAerea = driver.FindElement(By.XPath("//*[@id='cards-list']/div[1]/div/div[1]/div[2]/div/div[1]/div[1]/h6/span[2]"));
                    cotacao = cotacao + companiaAerea.Text + ";";
                    Console.WriteLine("Compania aerea : " + companiaAerea.Text);

                    aeroportoOrigem = driver.FindElement(By.XPath("//*[@id='cards-list']/div[1]/div/div[1]/div[2]/div/div[2]/div/div[1]/h5/span[1]/span"));
                    cotacao = cotacao + aeroportoOrigem.Text + ";";
                    Console.WriteLine("Aeroporto origem : " + aeroportoOrigem.Text);

                    horaembarque = driver.FindElement(By.XPath("//*[@id='cards-list']/div[1]/div/div[1]/div[2]/div/div[2]/div/div[1]/h5/span[2]"));
                    cotacao = cotacao + horaembarque.Text + ";";
                    Console.WriteLine("Hora embarque : " + horaembarque.Text);


                    aeroportoDestino = driver.FindElement(By.XPath("//*[@id='cards-list']/div[1]/div/div[1]/div[2]/div/div[2]/div/div[2]/h5/span[1]/span"));
                    cotacao = cotacao + aeroportoDestino.Text + ";";
                    Console.WriteLine("Aeroporto destino: " + aeroportoDestino.Text);

                    horadesembarque = driver.FindElement(By.XPath("//*[@id='cards-list']/div[1]/div/div[1]/div[2]/div/div[2]/div/div[2]/h5/span[2]"));
                    cotacao = cotacao + horadesembarque.Text + ";";
                    Console.WriteLine("Hora desembarque : " + horadesembarque.Text);

                    Console.WriteLine("------------------------------------------------------------------------");
                    var valor = driver.FindElement(By.XPath("//*[@id='cards-list']/div[1]/div/div[2]/div[1]/div[1]/div/div/div[3]/span"));
                    cotacao = cotacao + valor.Text + ";";
                    Console.WriteLine("Preço final: " + valor.Text);
                    Console.WriteLine("------------------------------------------------------------------------");
                    cotacao = cotacao + CriarURL() + ";";
                    return cotacao;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e + "Erro ao efetuar cotação na "+ site + " - Data Ida: " + dataida.ToShortDateString() + " e Data Volta: " + datavolta.ToShortDateString());
                return site + ";;;;;;*;;;;;;*;;;;;;;; " + "Erro ao efetuar cotação na "+ site + " - Data Ida: " + dataida.ToShortDateString() + " e Data Volta: " + datavolta.ToShortDateString() + ";";
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
            var UrlNova = string.Format("https://www.cvc.com.br/passagens/v2/search/{0}/{1}?Date1={2}&Date2={3}&ADT={4}&CHD={5}&INF=0&CLA=eco&DIRETO={6}", origem, destino, dataida.ToString("yyyy-MM-dd"), datavolta.ToString("yyyy-MM-dd"), adulto, crianca, voodireto);
            return UrlNova;
        }
    }
}
