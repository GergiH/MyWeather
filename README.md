
# MyWeather

A basic application to ingest Weather/Forecast information from a WeatherBit.io endpoint and display the selected cities' forecasts of the user. Cities can be added, removed, changed or favourited.

## Running the application locally

### Prerequisites

- Visual Studio (preferrably 2019 or 2022) installed
- [WeatherBit.io](https://www.weatherbit.io/) registration, and API key

1. Open the solution in Visual Studio
2. Right-click on the `MyWeather` project > `Manage User Secrets`
3. Set the `WeatherBitApiKey` property in the secrets with your API key, like ex.:
```json
{
    "WeatherBitApiKey": "exampleapikey01234"
}
```

### Trying the application out

As the application uses an InMemoryDb database to be easily testable, by default the it only gets the first `100` cities so it won't grow too big in memory usage. This behaviour can be adjusted in `/MyWeather/Data/Seed/InitialSeed.cs` by changing either of the following lines:

```csharp
bool addAll = false;  // set this to 'true' to get all Cities
int maxRecordCount = 100;  // keep the above 'false' and set this to any preferred number
```

Simply run the application with Visual Studio.

#### Side-notes

To use a different database type it can be changed in `Startup.cs` but then the missing Migrations (which are not required for InMemoryDb) should be added. More info at: https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/hands-on-labs/aspnet-mvc-4-entity-framework-scaffolding-and-migrations

Additionally the application can be deployed under IIS and should work the same way.

## Possible improvements / changes

### Using AJAX

If it was desirable to handle action while staying on the same page, using JQuery and AJAX could be an option. Ex.:

```javascript
<script>
    $("#buttonId").click(function ()
    {
         $.ajax({
            url: "/Controller/Action",
            type: "post",
            data: { value1: x, value2: y },
            success: function (response) {
                // do something with the response ...
            },
            error: function(jqXHR, textStatus, errorThrown) {
               console.log(textStatus, errorThrown); // log the error to the browser console for debugging
            }
        });
    });
</script>
```

### Timed ingestions with CronJobs

Using a separate solution/project it would be possible to automatically ingest all cities' data and push them into the database periodically, so we'd only need to query the database from the application as we'd always have up-to-data data in there.

### Role based authentication

Separating user privileges into roles and managing actions based on what roles the user has. More info at: https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-3.1
