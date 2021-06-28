using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProcessamentoFila.Domain.DTO;

namespace ProcessamentoFila.Application.Interfaces.Services
{
    public interface IMoedaAppService
    {
        Task<bool> AdicionarMoedaFilaAsync(List<MoedaDto> request);
        Task<DadosMoedaResponse> PegarMoedaFilaAsync();
    }
}
