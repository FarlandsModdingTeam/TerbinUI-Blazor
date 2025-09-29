using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TerbinUI_Blazor.Script.Backend
{
    public class PruebasWoeker
    {
    }

    public class SumWorker : BackgroundService
    {
        /*
        public async Task IniciarAsync()
        {
            using var server = new NamedPipeServerStream
            (
                "SumaPipe",
                PipeDirection.InOut,
                1,
                PipeTransmissionMode.Byte,
                PipeOptions.Asynchronous
            );
            while (true)
            {
                await server.WaitForConnectionAsync();

                byte[] buffer = new byte[2]; // Espera dos bytes (sumando y sumando)
                int bytesRead = await server.ReadAsync(buffer, 0, buffer.Length);

                byte resultado = (byte)(buffer[0] + buffer[1]);
                await server.WriteAsync(new byte[] { resultado }, 0, 1);
            }
        }
        */

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var server = new NamedPipeServerStream(
                    "SumaPipe",
                    PipeDirection.InOut,
                    1,
                    PipeTransmissionMode.Byte,
                    PipeOptions.Asynchronous
                );

                await server.WaitForConnectionAsync(stoppingToken);

                byte[] buffer = new byte[2];
                int bytesRead = await server.ReadAsync(buffer, 0, buffer.Length, stoppingToken);

                byte resultado = (byte)(buffer[0] + buffer[1]);
                await server.WriteAsync(new byte[] { resultado }, 0, 1, stoppingToken);

                server.Disconnect();
            }
        }
    }

}
