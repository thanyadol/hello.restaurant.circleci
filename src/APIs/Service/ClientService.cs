using System;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Newtonsoft.Json;

//logg
using Serilog;

using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace cvx.lct.vot.api.APIs.Services
{
    public interface IClientService
    {
        Task<IEnumerable<VesselAsync>> ListVesselAsync();
    }

    public class ClientService : IClientService
    {
        private readonly HttpClient _httpClient;

        //private string _remoteServiceBaseUrl; // = "https://localhost:5001";
        private string _remoteLCTServiceBaseUrl;

        private static TelemetryClient _telemetry { get; set; }

        private readonly IConfiguration _configuration;

        public ClientService(HttpClient httpClient, IConfiguration configuration)
        {
            // _httpClient = httpClient;
            _configuration = configuration;

            //var credentialsCache = new CredentialCache { { uri, "NTLM", CredentialCache.DefaultNetworkCredentials } };
            //var handler = new HttpClientHandler { Credentials = credentialsCache };

            // Create an HttpClientHandler object and set to use default credentials
            HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            //handler.UseProxy = false;

            // handler.Credentials = new NetworkCredential("svc-bkkhq-surface-dv", "svc-bkkhq-surface-dv", "CT");

            // Create an HttpClient object
            _httpClient = new HttpClient(handler);

            //_remoteServiceBaseUrl = _configuration["ApiSettings:RemoteServiceBaseUrl"];
            _remoteLCTServiceBaseUrl = _configuration["LctConfig:Host"];

            //for proxy authen
            /* HttpClientHandler handler = new HttpClientHandler();
            handler.UseDefaultCredentials = true;
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            //handler.UseProxy = false;

            // Omit this part if you don't need to authenticate with the web server:
            handler.DefaultProxyCredentials = System.Net.CredentialCache.DefaultNetworkCredentials;

            _httpClient = new HttpClient(handler: handler, disposeHandler: true);
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "token");
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            _remoteLCTServiceBaseUrl = _configuration["LctConfig:Host"];*/
        }

        //
        // Summary:
        //   list all status vessel from LCT
        //
        // Returns:
        //    vessel list
        //
        public async Task<IEnumerable<VesselAsync>> ListVesselAsync()
        {
            var url = _configuration["LctConfig:VesselUrl"];
            //LCT enpoint
            var uri = new Uri(_remoteLCTServiceBaseUrl + url);

            //call
            //ar result = await _httpClient.GetStringAsync(uri);

            // Act
            var result = new HttpResponseMessage();

            try
            {
                result = await _httpClient.GetAsync(uri);
                result.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new ClientNotSuccessException((int)result.StatusCode, "HTTPGET", uri.ToString(), ex.Message);
            }

            var httpReponse = await result.Content.ReadAsStringAsync();
            var vessels = JsonConvert.DeserializeObject<VesselListAsync>(httpReponse);

            var entities = vessels.vesselInfoList.Select(s => new VesselAsync
            {
                Id = s.VesselId,
                Name = s.VesselName,
                TypeId = s.VesselTypeId,
                Type = s.VesselType,
                Status = s.VesselStatus,
                StatusId = s.VesselStatusId,
                MaxSpeed = s.MaxSpeed,
                Speed = s.Speed,
                //Date = null,
                Specfication = new VesselSpecAsync
                {
                    VesselId = s.VesselId,
                    Barite = s.VesselSpecification.Barite,
                    BaritePerTank = s.VesselSpecification.BaritePerTank,
                    BariteTank = s.VesselSpecification.BariteTank,
                    BhiCement = s.VesselSpecification.BhiCement,
                    BhiCementPerTank = s.VesselSpecification.BhiCementPerTank,
                    BhiCementTank = s.VesselSpecification.BhiCementTank,
                    DieselTankCapacity = s.VesselSpecification.ActualFuelTanksTotal,
                    EffectiveDeadWeight = s.VesselSpecification.EffectiveDeadWeight,
                    EffectiveDeckSpace = s.VesselSpecification.EffectiveDeckSpace,
                    EffectiveDrillWater = s.VesselSpecification.EffectiveDrillWater,
                    EffectivePotWater = s.VesselSpecification.EffectivePotWater,
                    HCement = s.VesselSpecification.HCement,
                    HCementPerTank = s.VesselSpecification.HCementPerTank,
                    HCementTank = s.VesselSpecification.HCementTank,
                    Saraline = s.VesselSpecification.Saraline,
                    SaralinePerTank = s.VesselSpecification.SaralinePerTank,
                    SaralineTank = s.VesselSpecification.SaralineTank
                }
            });

            return entities;
        }

        
    }
}