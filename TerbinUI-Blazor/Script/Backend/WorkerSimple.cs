
namespace TerbinUI_Blazor.Script.Backend
{
    public class WorkerSimple : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {

                await Task.Delay(1000, stoppingToken);
            }
        }

        private float sum(float[] eNum)
        {
            float resultado = 0;
            for (int i = 0; i < eNum.Length; i++)
            {
                resultado += eNum[i];
            }
            return resultado;
        }
    }
}
