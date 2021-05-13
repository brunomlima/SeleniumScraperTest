using System;
using System.Collections.Generic;
using System.Text;

namespace SeleniumScraperTest
{
    public interface ICotacao
    {
        string Pesquisa();
        void GravaArquivo(string caminho);
        string CriarURL();
    }
}
