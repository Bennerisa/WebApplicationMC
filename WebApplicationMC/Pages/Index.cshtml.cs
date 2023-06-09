using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class MicroOndasModel : PageModel
{
    [BindProperty]
    public int Minutos { get; set; }

    [BindProperty]
    public int Segundos { get; set; }

    [BindProperty]
    public string? ProgressodeAquecimentoString { get; set; }

    public int Potencia { get; set; } = 10;

    public string? TempoExibicao { get; set; }

    public bool AquecimentoemProgresso { get; set; }

    public int TempoRestante { get; set; }

    public IActionResult OnPostIniciarAquecimento()
    {
        if (AquecimentoemProgresso)
        {
            TempoRestante += 30;
            return Page();
        }

        if (Segundos < 1) Segundos = 1;
        if (Minutos < 0) Minutos = 0;
        if (Segundos > 60 && Segundos < 100)
        {
            Minutos = Segundos / 60;
            Segundos = Segundos % 60;
        }
        else if (Segundos >= 100)
        {
            Minutos = Segundos / 100;
            Segundos = Segundos % 100;
        }

        if (Potencia < 1 || Potencia > 10)
        {
            ModelState.AddModelError(string.Empty, "Por favor, insira um valor de Potência válido (entre 1 e 10)");
            return Page();
        }

        if (Minutos == 0 && (Segundos < 1 || Segundos > 120))
        {
            ModelState.AddModelError(string.Empty, "Por favor insira um tempo válido (entre 1 segundo e 2 minutos).");
            return Page();
        }

        ProgressodeAquecimentoString = GenerateProgressodeAquecimentoString();

        TempoExibicao = $"{Minutos}:{Segundos:D2}";

        AquecimentoemProgresso = true;

        TempoRestante = Minutos * 60 + Segundos;

        return Page();
    }

    public IActionResult OnPostParar()
    {
        AquecimentoemProgresso = false;
        TempoRestante = 0;
        TempoExibicao = null;
        ProgressodeAquecimentoString = null;

        return Page();
    }

    private string GenerateProgressodeAquecimentoString()
    {
        string progresso = string.Empty;
        int caracteresPorSegundo = 10 - Potencia + 1;
        int totalCaracteres = Minutos * 60 + Segundos;

        for (int i = 0; i < totalCaracteres; i++)
        {
            progresso += ".";
            if ((i + 1) % caracteresPorSegundo == 0)
                progresso += " ";
        }

        progresso += " Aquecimento concluído";

        return progresso;
    }
}
