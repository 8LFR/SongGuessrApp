using System.Net.Http.Headers;

namespace SongGuessr.HttpClientUtilities;

public class HttpRequestMessageBuilder
{
    private HttpRequestMessage _httpRequestMessage;

    private HttpRequestMessageBuilder()
    {
        _httpRequestMessage = new HttpRequestMessage();
    }

    public static HttpRequestMessageBuilder Create()
    {
        return new HttpRequestMessageBuilder();
    }

    public HttpRequestMessageBuilder WithHttpMethod(HttpMethod method)
    { 
        _httpRequestMessage.Method = method;
        return this;
    }

    public HttpRequestMessageBuilder WithRequestUrl(string url)
    {
        _httpRequestMessage.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);
        return this;
    }

    public HttpRequestMessageBuilder WithAuthorizationTokenHeader(string bearerToken)
    {
        _httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
        return this;
    }

    public HttpRequestMessage Build()
    {
        return _httpRequestMessage;
    }
}
