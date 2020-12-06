using StakerGuard.Services.Dtos;
using System.Threading.Tasks;

namespace StakerGuard.Services
{
    public interface IEth2Service
    {
        public Task<ValidatorStatus> CheckValidator(string publicKey);
    }
}