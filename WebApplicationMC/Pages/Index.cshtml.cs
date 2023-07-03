using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace microOndas
{
    [BindProperties]
    public class MicroOndasModel : PageModel
    {
        public int Tempo { get; set; }
        public string? ProgressoDeAquecimentoString { get; set; }
        public int Potencia { get; set; } = 10;
        public string? TempoExibicao { get; set; }
        public bool AquecimentoEmProgresso { get; set; }
        public int TempoRestante { get; set; }

        public IActionResult OngetIniciarAquecimento()
        {
            if (AquecimentoEmProgresso)
            {
                TempoRestante += 30;
                return RedirectToPage();
            }

            if (Tempo < 1 || Tempo > 120)
            {
                ModelState.AddModelError(string.Empty, "Por favor, insira um tempo válido (entre 1 segundo e 2 minutos).");
                return Page();
            }

            if (Potencia < 1 || Potencia > 10)
            {
                ModelState.AddModelError(string.Empty, "Por favor, insira um valor de Potência válido (entre 1 e 10).");
                return Page();
            }

            ProgressoDeAquecimentoString = GerarProgressoDeAquecimentoString();

            TempoExibicao = FormatarTempoExibicao(Tempo);

            AquecimentoEmProgresso = true;

            TempoRestante = Tempo;

            return RedirectToPage();
        }

        public IActionResult OngetIniciarAquecimentoRapido()
        {
            if (AquecimentoEmProgresso)
            {
                TempoRestante += 30;
                return RedirectToPage();
            }

            Tempo = 30;
            Potencia = 10;

            return OngetIniciarAquecimento();
        }

        public IActionResult OngetParar()
        {
            AquecimentoEmProgresso = false;
            TempoRestante = 0;
            TempoExibicao = null;
            ProgressoDeAquecimentoString = null;

            return RedirectToPage();
        }

        private string GerarProgressoDeAquecimentoString()
        {
            string progresso = string.Empty;
            int caracteresPorSegundo = 10 - Potencia + 1;

            for (int i = 0; i < Tempo; i++)
            {
                progresso += ".";
                if ((i + 1) % caracteresPorSegundo == 0)
                    progresso += " ";
            }

            progresso += " Aquecimento concluído";

            return progresso;
        }

        private string FormatarTempoExibicao(int tempo)
        {
            int minutos = tempo / 60;
            int segundos = tempo % 60;

            return $"{minutos}:{segundos:D2}";
        }
    }
}
