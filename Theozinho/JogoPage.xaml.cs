namespace Theozinho;

public partial class JogoPage : ContentPage
{
    const int aberturaMinima = 200;
    const int gravidade = 30;
    const int tempoEntreFrames = 50;
    bool estaMorto = false;
    double larguraJanela = 0;
    double alturaJanela = 0;
    int velocidade = 20;
    const int maxTempoPulando = 3;
    int tempoPulando = 0;
    bool estaPulando = false;
    const int forcaPulo = 60;
    const int tamanhoMinimoPassagem = 200;

    public JogoPage()
    {
        InitializeComponent();
    }

    void AplicaPulo()
    {
        ImgTheo.TranslationY -= forcaPulo;
        tempoPulando++;
        if (tempoPulando >= maxTempoPulando)
        {
            estaPulando = false;
            tempoPulando = 0;
        }

    }
    void AplicaGravidade()
    {
        ImgTheo.TranslationY += gravidade;
    }

    async Task Desenhar()
    {
        while (!estaMorto)
        {
            if (estaPulando)
                AplicaPulo();
            else
                AplicaGravidade();
            await Task.Delay(tempoEntreFrames);
            GerenciaCanos();
            if (VerificaColisao())
            {
                estaMorto = true;
                frameGameOver.IsVisible = true;
                break;
            }
            await Task.Delay(tempoEntreFrames);
        }




    }
    protected override void OnSizeAllocated(double w, double h)
    {
        base.OnSizeAllocated(w, h);
        larguraJanela = w;
        alturaJanela = h;
    }

    void GerenciaCanos()
    {
        CanoCima.TranslationX -= velocidade;
        CanoBaixo.TranslationX -= velocidade;
        if (CanoBaixo.TranslationX <= -larguraJanela)
        {
            CanoBaixo.TranslationX = 4;
            CanoCima.TranslationX = 4;
            var alturaMax = -100;
			var alturaMin = CanoBaixo.HeightRequest;
			CanoCima.TranslationY = Random.Shared.Next((int)alturaMin, (int)alturaMax);
			CanoBaixo.TranslationY = CanoCima.TranslationY + aberturaMinima + CanoBaixo.HeightRequest;

        }
    }

    void GameOverCliclado(object s, TappedEventArgs a)
    {
        frameGameOver.IsVisible = false;
        Inicializar();
        Desenhar();
    }

    
    void Inicializar()
    {
        estaMorto = false;
        ImgTheo.TranslationY = 0;
    }
    bool VerificaColisao()
    {
        if (!estaMorto)
        {
            return (!estaMorto && (VerificaColisaoChao() || VerificaColisaoTeto() || VerificaColisaoCano()));
            {
                return true;
            }

        }
        return false;
    }

    bool VerificaColisaoCano()
   {
    if (VerificaColisaoCanoBaixo() || VerificaColisaoCanoCima())
      return true;
    else
      return false;
   }

    bool VerificaColisaoTeto()
    {
        var minY = -alturaJanela / 2;
        if (ImgTheo.TranslationY <= minY)
            return true;
        else
            return false;
    }
    bool VerificaColisaoChao()
    {
        var maxY = alturaJanela / 2 - FundoJogo.HeightRequest;
        if (ImgTheo.TranslationY >= maxY)
            return true;
        else
            return false;

    }
    void Clicado(object s, TappedEventArgs a)
    {
        estaPulando = true;
    }
    bool VerificaColisaoCanoCima()
   {
    var posicaoHorizontalPardal = (larguraJanela - 50) - (ImgTheo.WidthRequest / 2);
    var posicaoVerticalPardal   = (alturaJanela / 2) - (ImgTheo.HeightRequest / 2) + ImgTheo.TranslationY;

    if (
         posicaoHorizontalPardal >= Math.Abs(CanoCima.TranslationX) - CanoCima.WidthRequest &&
         posicaoHorizontalPardal <= Math.Abs(CanoCima.TranslationX) + CanoCima.WidthRequest &&
         posicaoVerticalPardal   <= CanoCima.HeightRequest + CanoCima.TranslationY
       )
      return true;
    else
      return false;
  }
     bool VerificaColisaoCanoBaixo()
    {
    var posicaoHorizontalPardal = larguraJanela - 50 - ImgTheo.WidthRequest / 2;
    var posicaoVerticalPardal   = (alturaJanela / 2) + (ImgTheo.HeightRequest / 2) + ImgTheo.TranslationY;

    var yMaxCano = CanoBaixo.HeightRequest + CanoBaixo.TranslationY + tamanhoMinimoPassagem;

    if (
         posicaoHorizontalPardal >= Math.Abs(CanoBaixo.TranslationX) - CanoBaixo.WidthRequest &&
         posicaoHorizontalPardal <= Math.Abs(CanoBaixo.TranslationX) + CanoBaixo.WidthRequest &&
         posicaoVerticalPardal   >= yMaxCano
       )
      return true;
    else
      return false;
  }
}

