using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using fourplaces.Models;
using MonkeyCache.SQLite;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace fourplaces
{
    public class Service
    {

        HttpClient httpClient;
        public static string URL_RACINE { get; set; }
        public static string URL_LOGIN { get; set; }
        public static string URL_REFRESH { get; set; }
        public static string URL_ME { get; set; }
        public static string URL_REGISTER { get; set; }

        public static string URL_UPDATE_PROFILE { get; set; }
        public static string URL_UPDATE_PASSWORD { get; set; }

        public static string URL_LIST_PLACES { get; set; }
        public static string URL_GET_PLACE { get; set; }

        public static string URL_CREATE_PLACE { get; set; }
        public static string URL_CREATE_COMMENT { get; set; }

        public static string URL_CREATE_IMAGE { get; set; }
        public static string URL_GET_IMAGE { get; set; }


        public Service()
        {
            httpClient = new HttpClient();

            URL_RACINE = "https://td-api.julienmialon.com";
            URL_LOGIN = "/auth/login"; // POST login with email/password
            URL_REFRESH = "/auth/refresh"; // POST refresh a token
            URL_ME = "/me"; // GET User profile
            URL_REGISTER = "/auth/register"; // POST register a user

            URL_UPDATE_PROFILE = "/me"; // PATCH Update profile (firstname, lastname)
            URL_UPDATE_PASSWORD = "/me/password"; // PATCH Update password

            URL_LIST_PLACES = "/places"; // GET List of places
            URL_GET_PLACE = "/places/"; // GET place detail

            URL_CREATE_PLACE = "/places"; //POST Create a place
            URL_CREATE_COMMENT = "/comments"; //POST Add a comment

            URL_CREATE_IMAGE = "/images"; //POST upload an image
            URL_GET_IMAGE = "/images/"; //GET retrieve image data*/

        }



        public async Task<bool> Connexion(string email, string password)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(URL_RACINE + URL_LOGIN, string.Empty));
                LoginRequest user = new LoginRequest(email, password);
                var json = JsonConvert.SerializeObject(user);
                var contentRequest = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    var contentResponse = await response.Content.ReadAsStringAsync();
                    Response<LoginResult> res = JsonConvert.DeserializeObject<Response<LoginResult>>(contentResponse);
                    App.SESSION_LOGIN = res.Data;
                    if (await GetUtilisateur())
                    {
                        return true;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> GetUtilisateur()
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(URL_RACINE + URL_ME, string.Empty));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Response<UserItem> res = JsonConvert.DeserializeObject<Response<UserItem>>(content);
                    App.SESSION_PROFIL = res.Data;
                    Console.WriteLine("ID UIma" + res.Data.ImageId);
                    return true;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool actualiser = await Actualiser();
                        if (actualiser)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                            response = await httpClient.GetAsync(uri);
                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                Response<UserItem> res = JsonConvert.DeserializeObject<Response<UserItem>>(content);
                                App.SESSION_PROFIL = res.Data;
                                return true;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> Inscription(string email, string firstName, string fastName, string password)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(URL_RACINE + URL_REGISTER, string.Empty));
                RegisterRequest user = new RegisterRequest(email, firstName, fastName, password);
                string json = JsonConvert.SerializeObject(user);
                var contentRequest = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    var contentResponse = await response.Content.ReadAsStringAsync();
                    Response<LoginResult> res = JsonConvert.DeserializeObject<Response<LoginResult>>(contentResponse);
                    Console.WriteLine("avant barrel");
                    App.SESSION_LOGIN = res.Data;
                    Console.WriteLine("apres barrel");
                    if (await GetUtilisateur())
                    {
                        Console.WriteLine("Get User ok");
                        return true;
                    }
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }



        public async Task<bool> GetImage(int id)
        {
            try
            {
                HttpClient oHttpClient = new HttpClient();
                var response = await oHttpClient.GetAsync("https://td-api.julienmialon.com/images/" + id);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> Actualiser()
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(URL_RACINE + URL_REFRESH, string.Empty));
                RefreshRequest temp = new RefreshRequest(Barrel.Current.Get<LoginResult>(key: "Login").RefreshToken);
                string data = JsonConvert.SerializeObject(temp);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    var contentResponse = await response.Content.ReadAsStringAsync();
                    Response<LoginResult> res = JsonConvert.DeserializeObject<Response<LoginResult>>(contentResponse);
                    App.SESSION_LOGIN = res.Data;
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<ListeLieux> GetListLieux()
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(URL_RACINE + URL_LIST_PLACES, string.Empty));
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Response<List<PlaceItemSummary>> res = JsonConvert.DeserializeObject<Response<List<PlaceItemSummary>>>(content);
                    return new ListeLieux(res.Data);
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }

        public async Task<PlaceItem> GetLieu(int id)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(URL_RACINE + URL_GET_PLACE + id, string.Empty));
                var response = await httpClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Response<PlaceItem> res = JsonConvert.DeserializeObject<Response<PlaceItem>>(content);
                    return res.Data;
                }
            }
            catch (Exception e)
            {
                return null;
            }
            return null;
        }





        public async Task<bool> EditUtilisateur(string firstname, string lastName, int? imageId)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(URL_RACINE + URL_UPDATE_PROFILE, string.Empty));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                UpdateProfileRequest temp = new UpdateProfileRequest(firstname, lastName, imageId);
                string data = JsonConvert.SerializeObject(temp);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
                HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
                requestMessage.Content = contentRequest;
                var response = await httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    if (await GetUtilisateur())
                    {
                        return true;
                    }
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool actualiser = await Actualiser();
                        if (actualiser)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                            response = await httpClient.SendAsync(requestMessage);
                            if (response.IsSuccessStatusCode)
                            {
                                if (await GetUtilisateur())
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> EditMdp(string oldMdp, string mdp)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(URL_RACINE + URL_UPDATE_PASSWORD, string.Empty));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                UpdatePasswordRequest temp = new UpdatePasswordRequest(oldMdp, mdp);
                string data = JsonConvert.SerializeObject(temp);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");

                HttpRequestMessage requestMessage = new HttpRequestMessage(new HttpMethod("PATCH"), uri);
                requestMessage.Content = contentRequest;

                var response = await httpClient.SendAsync(requestMessage);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool actualiser = await Actualiser();
                        if (actualiser)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                            response = await httpClient.SendAsync(requestMessage);
                            if (response.IsSuccessStatusCode)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Commenter(string commentaire, int? placeId)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(URL_RACINE + URL_GET_PLACE + placeId + URL_CREATE_COMMENT, string.Empty));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                CreateCommentRequest temp = new CreateCommentRequest(commentaire);
                string data = JsonConvert.SerializeObject(temp);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool actualiser = await Actualiser();
                        if (actualiser)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                            response = await httpClient.PostAsync(uri, contentRequest);
                            if (response.IsSuccessStatusCode)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> AjouterLieu(string title, string description, int imageId, double Latitude, double longitude)
        {
            try
            {
                httpClient = new HttpClient();
                var uri = new Uri(string.Format(URL_RACINE + URL_CREATE_PLACE, string.Empty));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                CreatePlaceRequest temp = new CreatePlaceRequest(title, description, imageId, Latitude, longitude);
                string data = JsonConvert.SerializeObject(temp);
                var contentRequest = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(uri, contentRequest);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool actualiser = await Actualiser();
                        if (actualiser)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                            response = await httpClient.PostAsync(uri, contentRequest);
                            if (response.IsSuccessStatusCode)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<MediaFile> ChoisirImage()
        {
            await CrossMedia.Current.Initialize();
            if (CrossMedia.Current.IsPickPhotoSupported)
            {
                MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                return photo;
            }
            return null;
        }


        public async Task<MediaFile> PrendrePhoto()
        {
            try
            {
                await CrossMedia.Current.Initialize();
                if (CrossMedia.Current.IsCameraAvailable && CrossMedia.Current.IsTakePhotoSupported)
                {
                    var media = new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = $"{DateTime.UtcNow}.jpg"
                    };

                    var file = await CrossMedia.Current.TakePhotoAsync(media);
                    return file;
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
            return null;
        }

        public async Task<int?> ChargerImage(bool temp)
        {
            try
            {
                var uri = new Uri(string.Format(URL_RACINE + URL_CREATE_IMAGE, string.Empty));
                MediaFile file;
                if (temp)
                {
                    file = await ChoisirImage();
                }
                else
                {
                    file = await PrendrePhoto();
                }
                httpClient = new HttpClient();
                byte[] imageData = File.ReadAllBytes(file.Path);
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, uri);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);

                MultipartFormDataContent requestContent = new MultipartFormDataContent();
                var imageContent = new ByteArrayContent(imageData);
                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                requestContent.Add(imageContent, "file", "file.jpg");
                request.Content = requestContent;
                HttpResponseMessage response = await httpClient.SendAsync(request);
                string result = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {
                    Response<ImageItem> res = JsonConvert.DeserializeObject<Response<ImageItem>>(result);
                    return res.Data.Id;
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        bool actualiser = await Actualiser();
                        if (actualiser)
                        {
                            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", App.SESSION_LOGIN.AccessToken);
                            response = await httpClient.SendAsync(request);
                            result = await response.Content.ReadAsStringAsync();
                            if (response.IsSuccessStatusCode)
                            {
                                Response<ImageItem> res = JsonConvert.DeserializeObject<Response<ImageItem>>(result);
                                return res.Data.Id;
                            }
                        }
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Position> Localisation()
        {
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 50;
                if (CrossGeolocator.IsSupported && CrossGeolocator.Current.IsGeolocationEnabled && CrossGeolocator.Current.IsGeolocationAvailable)
                {
                    return await locator.GetPositionAsync();
                }
                return await locator.GetLastKnownLocationAsync();
            }
            catch (Exception e)
            {
                return null;
            }
        }

    }
}
