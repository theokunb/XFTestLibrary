using Firebase.Storage;
using System.Threading.Tasks;
using Xamarin.Essentials;
using XFTestLibrary.Services;

namespace XFTestLibrary.Models
{
    public class FireBaseStorage : IStorage
    {
        public FireBaseStorage(string link)
        {
            storage = new FirebaseStorage(link);
        }

        private readonly FirebaseStorage storage;


        public async Task<string> PushImageAsync(FileResult fileResult)
        {
           return await storage.Child("Covers").Child(fileResult.FileName).PutAsync(await fileResult.OpenReadAsync());
        }
    }
}
