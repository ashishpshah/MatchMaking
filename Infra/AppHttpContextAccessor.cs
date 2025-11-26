using Microsoft.AspNetCore.DataProtection;

namespace MatchMaking.Infra
{
	public static class AppHttpContextAccessor
	{
		private static IHttpContextAccessor _httpContextAccessor;
		private static string _contentRootPath;
		private static string _webRootPath;
		private static IDataProtector _dataProtector;
		private static IConfiguration _iConfig;
		private static IHttpClientFactory _clientFactory;
		private static string _authAPIUrl;
		private static string _connectionString;

		public static void Configure(IHttpContextAccessor httpContextAccessor, IHostEnvironment env_Host, IWebHostEnvironment env_Web, IDataProtectionProvider provider, IConfiguration iConfig, IHttpClientFactory clientFactory)
		{
			_httpContextAccessor = httpContextAccessor;
			_contentRootPath = env_Host.ContentRootPath;
			_webRootPath = env_Web.WebRootPath;
			_dataProtector = provider.CreateProtector("20250409095731001");
			_iConfig = iConfig;
			_clientFactory = clientFactory;

			ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
			string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
			configurationBuilder.AddJsonFile(path, optional: false);
			IConfigurationRoot configurationRoot = configurationBuilder.Build();

			_connectionString = configurationRoot.GetSection("ConnectionStrings").GetSection("DataConnection").Value;
		}

		public static string EncrKey => "20251125095731001";
		public static string DataConnectionString => _connectionString;
		public static HttpContext AppHttpContext => _httpContextAccessor.HttpContext;
		public static HttpClient AppHttpClient => _clientFactory.CreateClient();
		public static IConfiguration AppConfiguration => _iConfig;

		public static string AppBaseUrl => $"{AppHttpContext.Request.Scheme}://{AppHttpContext.Request.Host}{AppHttpContext.Request.PathBase}";
		public static string ContentRootPath => $"{_contentRootPath}";
		public static string WebRootPath => $"{_webRootPath}";
		public static bool IsLogActive_Info => Convert.ToBoolean(AppHttpContextAccessor.AppConfiguration.GetSection("IsLogActive_Info").Value);
		public static bool IsLogActive_Error => Convert.ToBoolean(AppHttpContextAccessor.AppConfiguration.GetSection("IsLogActive_Error").Value);

		public static int LoggedUserId => AppHttpContext.Session.GetInt32(SessionKey.KEY_USER_ID).HasValue
			? AppHttpContext.Session.GetInt32(SessionKey.KEY_USER_ID).Value : 0;

		public static string Protect(string str) => $"{_dataProtector.Protect(str)}";
		public static string UnProtect(string str) => $"{_dataProtector.Unprotect(str)}";

		//public static HttpClient AppHttpClient_AuthAPI()
		//{
		//	HttpClient client = _clientFactory.CreateClient();
		//	client.BaseAddress = new Uri(_authAPIUrl);
		//	return client;
		//}

	}
}
