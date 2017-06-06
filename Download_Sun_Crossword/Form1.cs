using Firebase.Database;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Download_Sun_Crossword
{
    public partial class Form1 : Form
    {
        private string sunCrosswordDownloadFolder = "C:/Users/tonyh/Source/Repos/react-sun-crossword/react-sun-crossword/src/sunCrosswordJsons/";
        private string convertedCrosswordFolderPath = "C:/Users/tonyh/Source/Repos/react-sun-crossword/react-sun-crossword/src/convertedSunCrosswordJsons/";
        private string firebaseAddress = "https://react-sun-crossword.firebaseio.com/";
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateSunCrosswords();
        }

        private void UpdateSunCrosswords()
        {
            var sunCrosswords = GetNewSunCrosswords();
            SaveSunCrosswords(sunCrosswords);
            var crosswords = ConvertSunCrosswords(sunCrosswords);
            SaveConvertedSunCrosswords(crosswords);
            UploadCrosswordsToFirebase(crosswords);
        }
        private List<CrosswordModelJson> ConvertSunCrosswords(IEnumerable<SunCrosswordJson> sunCrosswords)
        {
            return sunCrosswords.Select(sc => SunCrosswordConverter.Convert(sc)).ToList();
        }
        private void SaveSunCrosswords(IEnumerable<SunCrosswordJson> sunCrosswords)
        {
            foreach (var sc in sunCrosswords)
            {
                File.WriteAllText(sunCrosswordDownloadFolder + sc.data.copy.id + ".json", JsonConvert.SerializeObject(sc));
            }
        }
        private void SaveConvertedSunCrosswords(IEnumerable<CrosswordModelJson> crosswords)
        {
            foreach (var cw in crosswords)
            {
                File.WriteAllText(convertedCrosswordFolderPath + cw.id + ".json", JsonConvert.SerializeObject(cw));
            }
        }
        private List<SunCrosswordJson> GetNewSunCrosswords()
        {
            var sunCrosswords = DownloadSunCrosswords();
            var existingSunCrosswordIds = GetExistingSunCrosswordIds();
            return sunCrosswords.Where(sc => !existingSunCrosswordIds.Contains(sc.data.copy.id)).ToList();
        }
        private List<string> GetExistingSunCrosswordIds()
        {
            return Directory.GetFiles(sunCrosswordDownloadFolder).Where(f => f.EndsWith(".json")).Select(f => {
                var lastSeparatorIndex = f.LastIndexOf("/");
                var fileName = f.Substring(lastSeparatorIndex + 1);
                fileName = fileName.Replace(".json", "");
                return fileName;
            }).ToList();
        }
        private List<SunCrosswordJson> DownloadSunCrosswords()
        {
            WebClient wc = new WebClient();
            var cwJson = wc.DownloadString("http://feeds.thesun.co.uk/puzzles/feed/thesun-two_speed-766bab0b8f95a693734fcd1aede660a0.tablet.jsonp");
            cwJson = cwJson.Substring(14, cwJson.Length - 16);
            var result = JsonConvert.DeserializeObject<CrosswordJson>(cwJson);
            var crosswordDownloads = result.data.Select(d =>
            {
                var parts = d.url.Split(new string[] { @"/" }, StringSplitOptions.None);
                var fileName = parts[parts.Length - 2];
                return new { downloadUrl = d.url + "/data.json", fileName = fileName };

            }).ToList();
            return crosswordDownloads.Select(crosswordDownload =>
            {
                var crosswordJson = wc.DownloadString(crosswordDownload.downloadUrl);
                return JsonConvert.DeserializeObject<SunCrosswordJson>(crosswordJson);
            }).ToList();

        }


        //for Linq purposes - for when require stats on existing crosswords
        private List<SunCrosswordJson> GetExistingSunCrosswords()
        {
            var existingScs = Directory.GetFiles(sunCrosswordDownloadFolder).Select(f => JsonConvert.DeserializeObject<SunCrosswordJson>(File.ReadAllText(f))).ToList();
            return existingScs;
        }
        private FirebaseClient GetAuthenticatedDatabaseClient()
        {
            return new Firebase.Database.FirebaseClient(firebaseAddress, new Firebase.Database.FirebaseOptions
            {
                AuthTokenAsyncFactory = () =>
                {
                    return GetAccesTokenAsync();
                }
                    ,
                AsAccessToken = true
            });
        }
        private Task<string> GetAccesTokenAsync()
        {
            var scopes = new[] { "https://www.googleapis.com/auth/firebase.database", "https://www.googleapis.com/auth/identitytoolkit", "https://www.googleapis.com/auth/userinfo.email" };
            var sac = ServiceAccountCredentialEx.FromServiceAccountDataWithScopesTokenServerUrl(new FileStream(@"C:\Users\tonyh\Documents\visual studio 2017\Projects\DownloadSun\DownloadSun\Service_Account_Details.json", FileMode.Open), scopes, "https://www.googleapis.com/oauth2/v4/token");
            return sac.GetAccessTokenForRequestAsync();
        }


        private void UploadCrosswordsToFirebase(IEnumerable<CrosswordModelJson> crosswords)
        {
            foreach (var cw in crosswords)
            {
                UploadCrosswordToFirebase(cw);
            }
        }
        //now using SerializeObject - should test this now works
        private void UploadCrosswordToFirebase(CrosswordModelJson crosswordModel)
        {
            var lookup = new CrosswordModelLookupJson { datePublished = crosswordModel.datePublished, id = crosswordModel.id, title = crosswordModel.title };
            var firebase = GetAuthenticatedDatabaseClient();
            firebase.Child("crosswords/" + crosswordModel.id).PutAsync(JsonConvert.SerializeObject(crosswordModel)).Wait();
            firebase.Child("crosswordLookups/" + lookup.id).PutAsync(JsonConvert.SerializeObject(lookup)).Wait();
        }

        private void ConvertAndSaveConvertedSunCrossword(SunCrosswordJson sunCrossword)
        {
            var convertedCrossword = SunCrosswordConverter.Convert(sunCrossword);
            File.WriteAllText(convertedCrosswordFolderPath + convertedCrossword.id + ".json", JsonConvert.SerializeObject(convertedCrossword));
        }

    }
    public static class ServiceAccountCredentialEx
    {
        public static ServiceAccountCredential FromServiceAccountDataWithScopesTokenServerUrl(Stream credentialData, IEnumerable<string> scopes, string tokenServerUrl)
        {
            var parameters = NewtonsoftJsonSerializer.Instance.Deserialize<JsonCredentialParameters>(credentialData);
            var initializer = new ServiceAccountCredential.Initializer(parameters.ClientEmail, tokenServerUrl).FromPrivateKey(parameters.PrivateKey);
            initializer.Scopes = scopes;
            return new ServiceAccountCredential(initializer);
        }
    }
}
