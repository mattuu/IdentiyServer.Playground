<Query Kind="Statements">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Data.Entity.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.Expressions.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.Parallel.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Linq.Queryable.dll</Reference>
  <NuGetReference>IdentityModel</NuGetReference>
  <Namespace>IdentityModel</Namespace>
  <Namespace>IdentityModel.Client</Namespace>
  <Namespace>IdentityModel.Internal</Namespace>
  <Namespace>IdentityModel.Jwk</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System.Data.Objects.SqlClient</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Text.Encodings.Web</Namespace>
  <Namespace>System.Text.Unicode</Namespace>
</Query>

var disco = await DiscoveryClient.GetAsync("http://localhost:5000");
if (disco.IsError)
{
	Console.WriteLine($"DISCO ERROR! {disco.Error}");
	return;
}

var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
var tokenResponse = await tokenClient.RequestClientCredentialsAsync("api1");

if (tokenResponse.IsError)
{
	Console.WriteLine(tokenResponse.Error);
	return;
}

// call api
var client = new HttpClient();
client.SetBearerToken(tokenResponse.AccessToken);

var response = await client.GetAsync("http://localhost:5010/api/identity");
if (!response.IsSuccessStatusCode)
{
	Console.WriteLine($"API Identity call: {response.StatusCode}");
}
else
{
	var content = await response.Content.ReadAsStringAsync();
	Console.WriteLine(JArray.Parse(content));
	content.Dump();
}

response = await client.GetAsync("http://localhost:5010/api/values");
if (!response.IsSuccessStatusCode)
{
	Console.WriteLine($"API Identity call: {response.StatusCode}");
}
else
{
	var content = await response.Content.ReadAsStringAsync();
	Console.WriteLine(JArray.Parse(content));
	content.Dump();
}
