using System.Threading.Tasks;
using Xamarin.Essentials;

namespace XFTestLibrary.Services
{
    public interface IStorage
    {
        Task<string> PushImageAsync(FileResult fileResult);
    }
}
