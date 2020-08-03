namespace tabuleiro
{
    /// <summary>
    /// classe responsavel por definir a posicao de uma determinada peca no tabuleiro
    /// </summary>
    class Posicao
    {

        public int linha { get; set; }
        public int coluna { get; set; }

        public Posicao(int linha, int coluna)
        {
            this.linha = linha;
            this.coluna = coluna;
        }

        /// <summary>
        /// Metodo que define quantas linhas e colunas tera o tabuleiro
        /// </summary>
        /// <param name="linha"></param>
        /// <param name="coluna"></param>
        public void definirValores(int linha, int coluna)
        {
            this.linha = linha;
            this.coluna = coluna;
        }

        public override string ToString()
        {
            return linha
                + ", "
                + coluna;
        }
    }
}
