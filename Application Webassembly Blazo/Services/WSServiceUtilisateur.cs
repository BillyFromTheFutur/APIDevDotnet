using Application_Webassembly_Blazo.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Application_Webassembly_Blazo.Services
{
    public class WSServiceUtilisateur:IService<Utilisateur>
    {
        private readonly HttpClient httpClient;
        public WSServiceUtilisateur()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://localhost:5247/api/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public Task<bool> DeleteAsync(string? nomControleur, Utilisateur? utilisateur)
        {
            throw new NotImplementedException();
        }
        public async Task<List<Utilisateur>?> GetAllAsync(string? nomControleur)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<List<Utilisateur>>(nomControleur);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<Utilisateur?> GetByStringAsync(string? nomControleur, string? str)
        {
            try
            {
                return await httpClient.GetFromJsonAsync<Utilisateur>(nomControleur+ "/GetByEmail/" + str);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<bool> PutAsync(string nomControleur, Utilisateur user)
        {
            try
            {
                var result = await httpClient.PutAsJsonAsync<Utilisateur>(nomControleur, user);
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
