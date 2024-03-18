using System.Text.Json.Nodes;

namespace BlazorWinFormsGenericToolKit.Services
{
    public interface IConfigServices
    {
        void UpdateConfig(JsonNode config);
        JsonNode GetConfig();
        public void ClearConfig();
        void SetDefaultNavPath(string navPath);

	}
}